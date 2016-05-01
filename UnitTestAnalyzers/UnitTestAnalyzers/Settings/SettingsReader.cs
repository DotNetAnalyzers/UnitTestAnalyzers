namespace UnitTestAnalyzers.Settings
{
    using System.Threading;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Text;
    using Newtonsoft.Json;
    using ObjectModel;

    /// <summary>
    /// Reads the unit tests analyzer settings.
    /// </summary>
    internal static class SettingsReader
    {
        /// <summary>
        /// Gets the settings.
        /// </summary>
        /// <param name="additionalText">The additional text.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The <see cref="AnalyzersSettings"/></returns>
        internal static AnalyzersSettings GetSettings(AdditionalText additionalText, CancellationToken cancellationToken)
        {
            SourceText text = additionalText.GetText(cancellationToken);
            try
            {
                SettingsFile settingsRoot = JsonConvert.DeserializeObject<SettingsFile>(text.ToString());

                return settingsRoot.Settings;
            }
            catch (JsonException)
            {
                // Error deserializing the settings file, will return the default settings instead.
            }

            return new AnalyzersSettings();
        }
    }
}
