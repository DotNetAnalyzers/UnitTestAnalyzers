namespace UnitTestAnalyzers.Settings.ObjectModel
{
    using Newtonsoft.Json;

    [JsonObject(MemberSerialization.OptIn)]
    internal class SettingsFile
    {
        /// <summary>
        /// This is the backing field for the <see cref="Settings"/> property.
        /// </summary>
        [JsonProperty("settings", DefaultValueHandling = DefaultValueHandling.Ignore)]
        private AnalyzersSettings settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsFile"/> class
        /// during JSON deserialization.
        /// </summary>
        [JsonConstructor]
        protected SettingsFile()
        {
            this.settings = new AnalyzersSettings();
        }

        public AnalyzersSettings Settings
        {
            get
            {
                return this.settings;
            }
        }
    }
}
