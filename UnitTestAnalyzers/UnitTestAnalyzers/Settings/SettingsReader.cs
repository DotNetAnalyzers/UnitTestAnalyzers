namespace UnitTestAnalyzers.Settings
{
    using System.Threading;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Text;
    using Newtonsoft.Json;
    using ObjectModel;

    internal static class SettingsReader
    {
        public static AnalyzersSettings GetSettings(AdditionalText additionalText, CancellationToken cancellationToken)
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
