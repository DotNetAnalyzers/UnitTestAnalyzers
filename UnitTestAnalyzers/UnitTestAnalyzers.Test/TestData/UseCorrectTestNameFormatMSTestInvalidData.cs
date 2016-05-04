namespace UnitTestAnalyzers.Test.TestData
{
    using System.Collections;
    using System.Collections.Generic;
    using Settings.ObjectModel;

    internal class UseCorrectTestNameFormatMSTestInvalidData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            // A MSTest TestMethod attribute is present (using statement) and the method name does not have the expected format.
            yield return new object[]
            {
                @"using Microsoft.VisualStudio.TestTools.UnitTesting;
class ClassName
{
[TestMethod]
public void Test1() {}
}",
"Test1", // Diagnostic message paramater
5,  // Violation line
13,  // Violation column.
TestSettingsData.MSTestSettingsSimpleRegex,
TestSettingsData.TestMethodNameSimpleFormat,
TestSettingsData.TestMethodNameFormatSimpleRegexExamples
            };

            // A MSTest TestMethod attribute is present (fully qualified name) and the method name does not have the expected format.
            yield return new object[]
            {
                @"
class ClassNameTests
{
[Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod]
public void Test1() {}
}",
"Test1",
5,
13,
TestSettingsData.MSTestSettingsSimpleRegex,
TestSettingsData.TestMethodNameSimpleFormat,
TestSettingsData.TestMethodNameFormatSimpleRegexExamples
            };

            // A MSTest TestMethod attribute is grouped along with another attribute and the method name does not have the expected format.
            yield return new object[]
            {
                @"using Microsoft.VisualStudio.TestTools.UnitTesting;
class ClassNameTests
{
[MyAttribute, TestMethod]
public void Test1() {}
}",
"Test1",
5,
13,
TestSettingsData.MSTestSettingsSimpleRegex,
TestSettingsData.TestMethodNameSimpleFormat,
TestSettingsData.TestMethodNameFormatSimpleRegexExamples
            };

            // A MSTest TestMethod attribute is present along with another attribute and the method name does not have the expected format.
            yield return new object[]
            {
                @"using Microsoft.VisualStudio.TestTools.UnitTesting;
class ClassNameTests
{
[MyAttribute]
[TestMethod]
public void Test1() {}
}",
"Test1",
6,
13,
TestSettingsData.MSTestSettingsSimpleRegex,
TestSettingsData.TestMethodNameSimpleFormat,
TestSettingsData.TestMethodNameFormatSimpleRegexExamples
            };

            // A MSTest TestMethod is present and the method name does not have the expected format.
            //  A configuration settings is not provided therefore the default settings should be used.
            yield return new object[]
            {
                @"using Microsoft.VisualStudio.TestTools.UnitTesting;
class ClassName
{
[TestMethod]
public void TestOne() {}
}",
"TestOne",
5,
13,
TestSettingsData.NoSettings,
AnalyzersSettings.DefaultFormat,
AnalyzersSettings.TestMethodNameFormatDefaultRegexExamples
            };

            // A MSTest TestMethod is present and the method name does not have the expected format.
            //  A configuration settings is provided without a regex therefore the default regex should be used.
            yield return new object[]
            {
                @"using Microsoft.VisualStudio.TestTools.UnitTesting;
class ClassName
{
[TestMethod]
public void TestOne() {}
}",
"TestOne",
5,
13,
TestSettingsData.MSTestSettings,
AnalyzersSettings.DefaultFormat,
AnalyzersSettings.TestMethodNameFormatDefaultRegexExamples
            };

            // A MSTest TestMethod is present and the method name does not have the expected format.
            //  An invalid configuration settings is provided therefore the default settings should be used.
            yield return new object[]
            {
                @"using Microsoft.VisualStudio.TestTools.UnitTesting;
class ClassName
{
[TestMethod]
public void TestOne() {}
}",
"TestOne",
5,
13,
TestSettingsData.InvalidSettings,
AnalyzersSettings.DefaultFormat,
AnalyzersSettings.TestMethodNameFormatDefaultRegexExamples
            };

            // A MSTest TestMethod is present and the method name does not have the expected format.
            //  No format is provided as part of the settings therefore the default format should be used in the message.
            yield return new object[]
            {
                @"using Microsoft.VisualStudio.TestTools.UnitTesting;
class ClassName
{
[TestMethod]
public void Test1() {}
}",
"Test1",
5,
13,
TestSettingsData.MSTestSettingsSimpleRegexNoFormat,
AnalyzersSettings.DefaultFormat,
TestSettingsData.TestMethodNameFormatSimpleRegexExamples
            };

            // A MSTest TestMethod is present and the method name does not have the expected format.
            //  No examples are provided as part of the settings therefore the default example should be found in the message.
            yield return new object[]
            {
                @"using Microsoft.VisualStudio.TestTools.UnitTesting;
class ClassName
{
[TestMethod]
public void Test1() {}
}",
"Test1",
5,
13,
TestSettingsData.MSTestSettingsSimpleRegexNoSampleProvided,
TestSettingsData.TestMethodNameSimpleFormat,
new[] { AnalyzersSettings.NoTestMethodNameExamplesProvided }
            };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
