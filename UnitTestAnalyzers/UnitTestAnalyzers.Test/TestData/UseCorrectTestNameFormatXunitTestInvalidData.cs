namespace UnitTestAnalyzers.Test.TestData
{
    using System.Collections;
    using System.Collections.Generic;
    using Settings.ObjectModel;

    internal class UseCorrectTestNameFormatXunitTestInvalidData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            // A Xunit Fact attribute is present (using statement) and the method name does not have the expected format.
            yield return new object[]
            {
                @"using Xunit;
class ClassName
{
[Fact]
public void Test1() {}
}",
"Test1", // Diagnostic message paramater
5,  // Violation line
13,  // Violation column.
TestSettingsData.XunitTestSettingsSimpleRegex,
TestSettingsData.TestMethodNameSimpleFormat,
TestSettingsData.TestMethodNameFormatSimpleRegexExamples
            };

            // A Xunit Theory attribute is present (using statement) and the method name does not have the expected format.
            yield return new object[]
            {
                @"using Xunit;
class ClassName
{
[Theory]
public void Test1() {}
}",
"Test1",
5,
13,
TestSettingsData.XunitTestSettingsSimpleRegex,
TestSettingsData.TestMethodNameSimpleFormat,
TestSettingsData.TestMethodNameFormatSimpleRegexExamples
            };

         // A Xunit Fact attribute is present (fully qualified name) and the method name does not have the expected format.
            yield return new object[]
            {
                @"
class ClassName
{
[Xunit.Fact]
public void Test1() {}
}",
"Test1",
5,
13,
TestSettingsData.XunitTestSettingsSimpleRegex,
TestSettingsData.TestMethodNameSimpleFormat,
TestSettingsData.TestMethodNameFormatSimpleRegexExamples
            };

            // A Xunit Theory attribute is present (fully qualified name) and the method name does not have the expected format.
            yield return new object[]
            {
                @"
class ClassName
{
[Xunit.Theory]
public void Test1() {}
}",
"Test1",
5,
13,
TestSettingsData.XunitTestSettingsSimpleRegex,
TestSettingsData.TestMethodNameSimpleFormat,
TestSettingsData.TestMethodNameFormatSimpleRegexExamples
            };

            // A Xunit Fact attribute is grouped along with another attribute and the method name does not have the expected format.
            yield return new object[]
            {
                @"using Xunit;
class ClassNameTests
{
[MyAttribute, Fact]
public void Test1() {}
}",
"Test1",
5,
13,
TestSettingsData.XunitTestSettingsSimpleRegex,
TestSettingsData.TestMethodNameSimpleFormat,
TestSettingsData.TestMethodNameFormatSimpleRegexExamples
            };

            // A Xunit Theory attribute is grouped along with another attribute and the method name does not have the expected format.
            yield return new object[]
            {
                @"using Xunit;
class ClassNameTests
{
[MyAttribute, Theory]
public void Test1() {}
}",
"Test1",
5,
13,
TestSettingsData.XunitTestSettingsSimpleRegex,
TestSettingsData.TestMethodNameSimpleFormat,
TestSettingsData.TestMethodNameFormatSimpleRegexExamples
            };

            // A Xunit Fact attribute is present along with another attribute and the method name does not have the expected format.
            yield return new object[]
            {
                @"using Xunit;
class ClassNameTests
{
[MyAttribute]
[Fact]
public void Test1() {}
}",
"Test1",
6,
13,
TestSettingsData.XunitTestSettingsSimpleRegex,
TestSettingsData.TestMethodNameSimpleFormat,
TestSettingsData.TestMethodNameFormatSimpleRegexExamples
            };

            // A Xunit Theory attribute is present along with another attribute and the method name does not have the expected format.
            yield return new object[]
            {
                @"using Xunit;
class ClassNameTests
{
[MyAttribute]
[Theory]
public void Test1() {}
}",
"Test1",
6,
13,
TestSettingsData.XunitTestSettingsSimpleRegex,
TestSettingsData.TestMethodNameSimpleFormat,
TestSettingsData.TestMethodNameFormatSimpleRegexExamples
            };

            // A Xunit Fact attribute is present and the method name does not have the expected format.
            //  A configuration settings is provided without a regex therefore the default regex should be used.
            yield return new object[]
            {
                @"using Xunit;
class ClassName
{
[Fact]
public void TestOne() {}
}",
"TestOne",
5,
13,
TestSettingsData.XunitSettings,
AnalyzersSettings.DefaultFormat,
AnalyzersSettings.TestMethodNameFormatDefaultRegexExamples
            };

            // A Xunit Theory attribute is present and the method name does not have the expected format.
            //  A configuration settings is provided without a regex therefore the default regex should be used.
            yield return new object[]
            {
                @"using Xunit;
class ClassName
{
[Theory]
public void TestOne() {}
}",
"TestOne",
5,
13,
TestSettingsData.XunitSettings,
AnalyzersSettings.DefaultFormat,
AnalyzersSettings.TestMethodNameFormatDefaultRegexExamples
            };

            // A Xunit Theory attribute is present and the method name does not have the expected format.
            //  A configuration settings is provided without format therefore the default format should be found in the message.
            yield return new object[]
            {
                @"using Xunit;
class ClassName
{
[Theory]
public void Test1() {}
}",
"Test1",
5,
13,
TestSettingsData.XunitTestSettingsSimpleRegexNoFormat,
AnalyzersSettings.DefaultFormat,
TestSettingsData.TestMethodNameFormatSimpleRegexExamples
            };
       }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
