namespace UnitTestAnalyzers.Test.TestData
{
    using Newtonsoft.Json;

    internal static class TestSettingsData
    {
       internal const string MSTestSettings = @"{
                ""settings"" : {
                ""unitTestFramework"" : ""MSTest""
                }
              }";

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

        internal static string NoSettings => string.Empty;
    }
}
