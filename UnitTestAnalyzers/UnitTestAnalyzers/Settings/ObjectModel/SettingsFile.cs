namespace UnitTestAnalyzers.Settings.ObjectModel
{
    using Newtonsoft.Json;

    /// <summary>
    /// Represents the settings file containing the <see cref="AnalyzersSettings"/>.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class SettingsFile
    {
        /// <summary>
        /// This is the backing field for the <see cref="Settings"/> property.
        /// </summary>
        [JsonProperty("settings", DefaultValueHandling = DefaultValueHandling.Ignore)]
        private readonly AnalyzersSettings settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsFile"/> class
        /// during JSON deserialization.
        /// </summary>
        [JsonConstructor]
        private SettingsFile()
        {
            this.settings = new AnalyzersSettings();
        }

        /// <summary>
        /// Gets the settings.
        /// </summary>
        /// <value>
        /// The settings.
        /// </value>
        internal AnalyzersSettings Settings => this.settings;
    }
}
