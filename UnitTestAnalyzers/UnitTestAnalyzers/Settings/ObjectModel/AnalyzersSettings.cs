namespace UnitTestAnalyzers.Settings.ObjectModel
{
    using System.Text.RegularExpressions;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    /// <summary>
    /// Represents the settings file used by the analyzer.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class AnalyzersSettings
    {
        /// <summary>
        /// This is the backing field for the <see cref="UnitTestFramework"/> property.
        /// </summary>
        [JsonProperty("unitTestFramework", DefaultValueHandling = DefaultValueHandling.Include)]
        [JsonConverter(typeof(StringEnumConverter))]
        private UnitTestFramework unitTestFramework;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnalyzersSettings"/> class.
        /// </summary>
        [JsonConstructor]
        internal AnalyzersSettings()
        {
            this.unitTestFramework = UnitTestFramework.MSTest;
        }

        /// <summary>
        /// Gets the unit test framework.
        /// </summary>
        /// <value>
        /// The unit test framework.
        /// </value>
        internal UnitTestFramework UnitTestFramework => this.unitTestFramework;
    }
}
