namespace UnitTestAnalyzers.Settings
{
    using System;
    using System.Collections.Immutable;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Diagnostics;
    using ObjectModel;

    /// <summary>
    /// Provides the Analyzer settings retrieving it from the file unittescodeanalysis.json
    /// and caches the value for a quicker subsequent access.
    /// </summary>
    internal static class SettingsProvider
    {
        private const string SettingsFileName = "unittestcodeanalysis.json";

        private static Tuple<WeakReference<Compilation>, StrongBox<AnalyzersSettings>> settingsCache = Tuple.Create(new WeakReference<Compilation>(null), new StrongBox<AnalyzersSettings>(null));

#pragma warning disable RS1012 // Start action has no registered actions.
        /// <summary>
        /// Gets the settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>The <see cref="AnalyzersSettings"/></returns>
        internal static AnalyzersSettings GetSettings(CompilationStartAnalysisContext context)
#pragma warning restore RS1012 // Start action has no registered actions.
        {
            StrongBox<AnalyzersSettings> cachedSettings = GetOrCreateCachedSettings(context.Compilation);
            AnalyzersSettings settings = cachedSettings.Value;
            if (settings == null)
            {
                ImmutableArray<AdditionalText> additionalFiles = context.Options?.AdditionalFiles ?? ImmutableArray.Create<AdditionalText>();

                foreach (AdditionalText additionalFile in additionalFiles)
                {
                    if (string.Equals(Path.GetFileName(additionalFile.Path), SettingsFileName, StringComparison.OrdinalIgnoreCase))
                    {
                        settings = SettingsReader.GetSettings(additionalFile, context.CancellationToken);
                        break;
                    }
                }

                // If no additional settings file was found use the default settings.
                settings = settings ?? new AnalyzersSettings();

                // If the defaul regex is the one being used (when a regex is not provided in the analyzer settings file then
                // use the default examples
                if (settings.TestMethodNameFormatRegex.Equals(AnalyzersSettings.DefaultRegex))
                {
                    settings.TestMethodNameFormatExamples.AddRange(AnalyzersSettings.TestMethodNameFormatDefaultRegexExamples);
                }

                cachedSettings.Value = settings;
            }

            return settings;
        }

        private static StrongBox<AnalyzersSettings> GetOrCreateCachedSettings(Compilation compilation)
        {
            var currentSettingsCache = settingsCache;

            Compilation cachedCompilation;
            if (!currentSettingsCache.Item1.TryGetTarget(out cachedCompilation) || cachedCompilation != compilation)
            {
                var replacementCache = Tuple.Create(new WeakReference<Compilation>(compilation), new StrongBox<AnalyzersSettings>(null));
                while (true)
                {
                    var prior = Interlocked.CompareExchange(ref settingsCache, replacementCache, currentSettingsCache);

                    if (prior == currentSettingsCache)
                    {
                        currentSettingsCache = replacementCache;
                        break;
                    }

                    currentSettingsCache = prior;
                    if (currentSettingsCache.Item1.TryGetTarget(out cachedCompilation) && cachedCompilation == compilation)
                    {
                        break;
                    }
                }
            }

            return currentSettingsCache.Item2;
        }
    }
}
