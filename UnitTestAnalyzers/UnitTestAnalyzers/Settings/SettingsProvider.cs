using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestAnalyzers.Settings
{
    using System.Collections.Immutable;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Diagnostics;
    using ObjectModel;

    internal static class SettingsProvider
    {
        private const string SettingsFileName = "unittestcodeanalysis.json";

        private static Tuple<WeakReference<Compilation>, StrongBox<AnalyzersSettings>> settingsCache = Tuple.Create(new WeakReference<Compilation>(null), new StrongBox<AnalyzersSettings>(null));

        public static StrongBox<AnalyzersSettings> GetOrCreateCachedSettings(Compilation compilation)
        {
            var currentSettingsCache = settingsCache;

            Compilation cachedCompilation;
            if (!currentSettingsCache.Item1.TryGetTarget(out cachedCompilation) || cachedCompilation != compilation)
            {
                var replacementCache = Tuple.Create(new WeakReference<Compilation>(compilation), new StrongBox<AnalyzersSettings>(null));
                while (true)
                {
                    var prior = Interlocked.CompareExchange(ref settingsCache, replacementCache, currentSettingsCache);

                    // If they are the same it means this thread succeeded on replacing with replacementCache, update currentSettingsCache to replacementCache (which is the same is the new settingsCache)
                    if (prior == currentSettingsCache)
                    {
                        currentSettingsCache = replacementCache;
                        break;
                    }

                    // If it got here it means this thread did not succeed when doing the Exchange which means another thread did it or else this one would have ssucceeded
                    // prior still points to the global settingsCache, make currentSettingsCache point to that as well and check the compilation within it
                    // if it is the same as this compilation then it confirms the other thread succeeded for this same compilation then break it, is all good. If it is not then try again (while true).
                    // A simpler implementation of this logic could be done with a do/while without this second check below
                    // it would be less performant though since likely all the other threads are operating for the same compilation. If we did not do the check below
                    // we would keep looping until the current thread is the one doing the replacement which is unecessary since they are all done for the same compilation object.
                    currentSettingsCache = prior;
                    if (currentSettingsCache.Item1.TryGetTarget(out cachedCompilation) && cachedCompilation == compilation)
                    {
                        break;
                    }
                }

                // Simpler implementation
                /*do
                {
                    currentSettingsCache = settingsCache;
                } while (Interlocked.CompareExchange(ref settingsCache, replacementCache, currentSettingsCache) != currentSettingsCache);
                currentSettingCache = settingsCache;
            */
            }

            return currentSettingsCache.Item2;
        }
 
        internal static AnalyzersSettings GetSettings(CompilationStartAnalysisContext context)
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

                if (settings.TestMethodNameFormatRegex.Equals(AnalyzersSettings.DefaultRegex))
                {
                    settings.testMethodNameFormatExamples.AddRange(new[] {"TestMethod_WhenFoo_ExpectsResult", "TestMethod_UsingFoo_ThrowsError"});
                }

                cachedSettings.Value = settings;
            }

            return settings;
        }
    }
}
