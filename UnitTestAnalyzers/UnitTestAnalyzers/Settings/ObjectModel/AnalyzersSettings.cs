namespace UnitTestAnalyzers.Settings.ObjectModel
{
    using System.Collections.Immutable;
    using System.Text.RegularExpressions;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    /// <summary>
    /// Represents the settings file used by the analyzer.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class AnalyzersSettings
    {
        /// <summary>
        /// The format of the test name describing a matching example for the <see cref="DefaultRegex"/>.
        /// </summary>
        public const string DefaultFormat = "(MemberUnderTestName)_(When[a-zA-Z])[0...n]_(Using[a-zA-Z])[0...n]_([Expects|Throws][a-zA-Z])";

        /// <summary>
        /// String used in the diagnostic warning produced by <see cref="UseCorrectTestNameFormatAnalyzer"/> when no examples are provided
        /// in the analyzer settings file.
        /// </summary>
        public static readonly string NoTestMethodNameExamplesProvided = "NO TEST NAME EXAMPLES PROVIDED!";

        /// <summary>
        /// The default regex used to validate test name formats.
        /// This is the regex that will be used when one is not provided in the analyzer settings file.
        /// </summary>
        internal static readonly Regex DefaultRegex = new Regex($@"\b(?=(?:.*?_){{2,}})(?<{MemberUnderTestRegexGroupName}>[a-zA-Z]+)(?:_When[A-Z]+[a-zA-Z0-9]+)*(?:_Using[A-Z]+[a-zA-Z]+)*(?:_Expects|_Throws)[A-Z]+[a-zA-Z]+\b");

        private const string MemberUnderTestRegexGroupName = "memberUnderTestName";

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
        private ImmutableArray<string>.Builder testMethodNameFormatExamples;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnalyzersSettings"/> class.
        /// </summary>
        [JsonConstructor]
        internal AnalyzersSettings()
        {
            this.unitTestFramework = UnitTestFramework.MSTest;
            this.testMethodNameFormatExamples = ImmutableArray<string>.Empty.ToBuilder();
            this.testMethodNameFormatRegex = DefaultRegex;
            this.testMethodNameFormat = DefaultFormat;
        }

        /// <summary>
        /// Gets the test method name examples that are valid for the default regex.
        /// <seealso cref="DefaultRegex"/>
        /// </summary>
        public static string[] TestMethodNameFormatDefaultRegexExamples => new[] { "TestMethod_WhenFoo_ExpectsResult", "TestMethod_UsingFoo_ThrowsError" };

        /// <summary>
        /// Gets the unit test framework.
        /// </summary>
        internal UnitTestFramework UnitTestFramework => this.unitTestFramework;

        /// <summary>
        /// Gets the test method name format regex.
        /// </summary>
        internal Regex TestMethodNameFormatRegex => this.testMethodNameFormatRegex;

        /// <summary>
        /// Gets the test method name format.
        /// </summary>
        internal string TestMethodNameFormat => this.testMethodNameFormat;

        /// <summary>
        /// Gets the test method name format examples.
        /// </summary>
        internal ImmutableArray<string>.Builder TestMethodNameFormatExamples => this.testMethodNameFormatExamples;
    }
}
