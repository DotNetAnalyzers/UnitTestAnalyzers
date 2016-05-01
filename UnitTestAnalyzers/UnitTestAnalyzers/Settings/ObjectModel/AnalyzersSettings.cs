namespace UnitTestAnalyzers.Settings.ObjectModel
{
    using System.Collections.Immutable;
    using System.Text.RegularExpressions;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    [JsonObject(MemberSerialization.OptIn)]
    internal class AnalyzersSettings
    {
        internal const string MemberUnderTestRegexGroupName = "memberUnderTestName";

        internal static Regex DefaultRegex = new Regex($@"\b(?=(?:.*?_){{2,}})(?<{MemberUnderTestRegexGroupName}>[a-zA-Z]+)(?:_When[A-Z]+[a-zA-Z0-9]+)*(?:_Using[A-Z]+[a-zA-Z]+)*(?:_Expects|_Throws)[A-Z]+[a-zA-Z]+\b");

        internal static string DefaultFormat = "(MemberUnderTestName)_(When[a-zA-Z])[0...n]_(Using[a-zA-Z])[0...n]_([Expects|Throws][a-zA-Z])";

        /// <summary>
        /// This is the backing field for the <see cref="UnitTestFramework"/> property.
        /// </summary>
        [JsonProperty("unitTestFramework", DefaultValueHandling = DefaultValueHandling.Include)]
        [JsonConverter(typeof(StringEnumConverter))]
        private UnitTestFramework unitTestFramework;

        /// <summary>
        /// This is the backing field for the <see cref="TestMethodNameFormatRegex"/> property.
        /// </summary>
        [JsonProperty("testMethodNameFormatRegex", DefaultValueHandling = DefaultValueHandling.Include)]
        private Regex testMethodNameFormatRegex;

        /// <summary>
        /// This is the backing field for the <see cref="TestMethodNameFormat"/> property.
        /// </summary>
        [JsonProperty("testMethodNameFormat", DefaultValueHandling = DefaultValueHandling.Include)]
        private string testMethodNameFormat;

        /// <summary>
        /// This is the backing field for the <see cref="TestMethodNameFormatExamples"/> property.
        /// </summary>
        [JsonProperty("testMethodNameFormatExamples", DefaultValueHandling = DefaultValueHandling.Include)]
        internal ImmutableArray<string>.Builder TestMethodNameFormatExamplesValue;

        [JsonConstructor]
        internal AnalyzersSettings()
        {
            this.unitTestFramework = UnitTestFramework.MSTest;
            this.TestMethodNameFormatExamplesValue = ImmutableArray<string>.Empty.ToBuilder();
            this.testMethodNameFormatRegex = DefaultRegex;
            this.testMethodNameFormat = DefaultFormat;
        }

        public UnitTestFramework UnitTestFramework => this.unitTestFramework;

        public Regex TestMethodNameFormatRegex => this.testMethodNameFormatRegex;

        public string TestMethodNameFormat => this.testMethodNameFormat;

        public ImmutableArray<string> TestMethodNameFormatExamples => this.TestMethodNameFormatExamplesValue.ToImmutable();
    }
}
