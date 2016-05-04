namespace UnitTestAnalyzers.Test.TestData
{
    using Newtonsoft.Json;

    internal static class TestSettingsData
    {
        internal const string TestMethodNameFormatSimpleRegex = "\"\\\\b[a-zA-Z]{4,}\\\\b\"";
        internal const string TestMethodNameSimpleFormatJson = "\"(a-zA-Z)[4...n]\"";

        internal const string XunitSettings = @"{
                ""settings"" : {
                ""unitTestFramework"" : ""Xunit""
                }
              }";

        internal const string InvalidSettings = @"
                ""settings"" : {
                ""unitTestFramework"" , ""MSTest""
                }
              }";

        internal const string MSTestSettings = @"{
                ""settings"" : {
                ""unitTestFramework"" : ""MSTest""
                }
              }";

        internal static string[] TestMethodNameFormatSimpleRegexExamples => new[] { "TestMethodOne", "TestMethodTwo" };

        internal static string TestMethodNameSimpleFormat => TestMethodNameSimpleFormatJson.Trim('"');

        internal static string MSTestSettingsSimpleRegex =>
                    $@"{{
                ""settings"" : {{
                ""unitTestFramework"" : ""MSTest"",
                ""testMethodNameFormat"" : {TestMethodNameSimpleFormatJson},
                ""testMethodNameFormatRegex"" : {{""Pattern"" : {TestMethodNameFormatSimpleRegex} }},
                ""testMethodNameFormatExamples"" : {JsonConvert.SerializeObject(TestMethodNameFormatSimpleRegexExamples)}
                 }}
              }}";

        internal static string XunitTestSettingsSimpleRegex =>
            $@"{{
                ""settings"" : {{
                ""unitTestFramework"" : ""Xunit"",
                ""testMethodNameFormat"" : {TestMethodNameSimpleFormatJson},
                ""testMethodNameFormatRegex"" : {{""Pattern"" : {TestMethodNameFormatSimpleRegex} }},
                ""testMethodNameFormatExamples"" : {JsonConvert.SerializeObject(TestMethodNameFormatSimpleRegexExamples)}
                 }}
              }}";

        internal static string XunitTestSettingsSimpleRegexNoFormat =>
                    $@"{{
                ""settings"" : {{
                ""unitTestFramework"" : ""Xunit"",
                ""testMethodNameFormatRegex"" : {{""Pattern"" : {TestMethodNameFormatSimpleRegex} }},
                ""testMethodNameFormatExamples"" : {JsonConvert.SerializeObject(TestMethodNameFormatSimpleRegexExamples)}
                 }}
              }}";

        internal static string MSTestSettingsSimpleRegexNoFormat =>
                    $@"{{
                ""settings"" : {{
                ""unitTestFramework"" : ""MSTest"",
                ""testMethodNameFormatRegex"" : {{""Pattern"" : {TestMethodNameFormatSimpleRegex} }},
                ""testMethodNameFormatExamples"" : {JsonConvert.SerializeObject(TestMethodNameFormatSimpleRegexExamples)}
                 }}
              }}";

        internal static string MSTestSettingsSimpleRegexNoSampleProvided =>
                    $@"{{
                ""settings"" : {{
                ""unitTestFramework"" : ""MSTest"",
                ""testMethodNameFormat"" : {TestMethodNameSimpleFormatJson},
                ""testMethodNameFormatRegex"" : {{""Pattern"" : {TestMethodNameFormatSimpleRegex} }},
                 }}
              }}";

        internal static string NoSettings => string.Empty;
    }
}
