namespace UnitTestAnalyzers.Test.TestData
{
    using Newtonsoft.Json;

    internal static class TestSettingsData
    {
        internal const string TestMethodNameFormatSimpleRegex = "\"\\\\b[a-zA-Z]{4,}\\\\b\"";

        internal const string TestMethodNameMemberUnderTestRegex = "\"\\\\b[a-zA-Z]{4,}_(?<memberUnderTestName>[a-zA-Z]+)\\\\b\"";

        internal const string TestMethodNameSimpleFormatJson = "\"(a-zA-Z)[4...n]\"";

        internal static string TestMethodNameSimpleFormat = TestMethodNameSimpleFormatJson.Trim('"');

        internal const string NoSampleProvided = "NO SAMPLE PROVIDED!";

        internal static string[] TestMethodNameFormatSimpleRegexExamples = {"TestMethodOne", "TestMethodTwo"};

        internal static string[] TestMethodNameFormatDefaultRegexExamples = {"TestMethod_WhenFoo_ExpectsResult", "TestMethod_UsingFoo_ThrowsError"};

        internal const string TestMethodNameDefaultFormat =  "(MemberUnderTestName)_(When[a-zA-Z])[0...n]_(Using[a-zA-Z])[0...n]_([Expects|Throws][a-zA-Z])";

        internal static string MSTestSettings = @"{
                ""settings"" : {
                ""unitTestFramework"" : ""MSTest""
                }
              }";

        internal static string MSTestSettingsSimpleRegex =
            $@"{{
                ""settings"" : {{
                ""unitTestFramework"" : ""MSTest"",
                ""testMethodNameFormat"" : {TestMethodNameSimpleFormatJson},
                ""testMethodNameFormatRegex"" : {{""Pattern"" : {TestMethodNameFormatSimpleRegex} }},
                ""testMethodNameFormatExamples"" : {JsonConvert.SerializeObject(TestMethodNameFormatSimpleRegexExamples)}
                 }}
              }}";

        internal static string MSTestSettingsSimpleRegexNoFormat =
            $@"{{
                ""settings"" : {{
                ""unitTestFramework"" : ""MSTest"",
                ""testMethodNameFormatRegex"" : {{""Pattern"" : {TestMethodNameFormatSimpleRegex} }},
                ""testMethodNameFormatExamples"" : {JsonConvert.SerializeObject(TestMethodNameFormatSimpleRegexExamples)}
                 }}
              }}";

        internal static string MSTestSettingsSimpleRegexNoSampleProvided =
                    $@"{{
                ""settings"" : {{
                ""unitTestFramework"" : ""MSTest"",
                ""testMethodNameFormat"" : {TestMethodNameSimpleFormatJson},
                ""testMethodNameFormatRegex"" : {{""Pattern"" : {TestMethodNameFormatSimpleRegex} }},
                 }}
              }}";

        internal static string MSTestSettingsMemberUnderTestRegexFormat =
            $@"{{
                ""settings"" : {{
                ""unitTestFramework"" : ""MSTest"",
                ""testMethodNameFormatRegex"" : {{""Pattern"" : {TestMethodNameMemberUnderTestRegex} }}
                 }}
              }}";


        internal static string XunitTestSettingsSimpleRegex =
            $@"{{
                ""settings"" : {{
                ""unitTestFramework"" : ""Xunit"",
                ""testMethodNameFormat"" : {TestMethodNameSimpleFormatJson},
                ""testMethodNameFormatRegex"" : {{""Pattern"" : {TestMethodNameFormatSimpleRegex} }},
                ""testMethodNameFormatExamples"" : {JsonConvert.SerializeObject(TestMethodNameFormatSimpleRegexExamples)}
                 }}
              }}";

        internal static string XunitTestSettingsSimpleRegexNoFormat =
            $@"{{
                ""settings"" : {{
                ""unitTestFramework"" : ""Xunit"",
                ""testMethodNameFormatRegex"" : {{""Pattern"" : {TestMethodNameFormatSimpleRegex} }},
                ""testMethodNameFormatExamples"" : {JsonConvert.SerializeObject(TestMethodNameFormatSimpleRegexExamples)}
                 }}
              }}";

        internal static string XunitTestSettingsMemberUnderTestRegexFormat =
            $@"{{
                ""settings"" : {{
                ""unitTestFramework"" : ""Xunit"",
                ""testMethodNameFormatRegex"" : {{""Pattern"" : {TestMethodNameMemberUnderTestRegex} }}
                 }}
              }}";

        internal static string XunitSettings = @"{
                ""settings"" : {
                ""unitTestFramework"" : ""Xunit""
                }
              }";

        internal static string InvalidSettings = @"
                ""settings"" : {
                ""unitTestFramework"" , ""MSTest""
                }
              }";

        internal static string NoSettings = string.Empty;
    }
}
