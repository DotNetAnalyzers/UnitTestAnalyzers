namespace UnitTestAnalyzers.Test.TestData
{
    using System.Collections;
    using System.Collections.Generic;
    using Settings.ObjectModel;

    internal class UseCorrectTestNameFormatNUnitTestInvalidData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            // A NUnit Test attribute is present (using statement) and the method name does not have the expected format.
            yield return new object[]
            {
                @"using NUnit.Framework;
class ClassName
{
[Test]
public void Test1() {}
}",
"Test1", // Diagnostic message paramater
5,  // Violation line
13,  // Violation column.
TestSettingsData.NUnitTestSettingsSimpleRegex,
TestSettingsData.TestMethodNameSimpleFormat,
TestSettingsData.TestMethodNameFormatSimpleRegexExamples
            };

            // A NUnit TestCase attribute is present (using statement) and the method name does not have the expected format.
            yield return new object[]
            {
                @"using NUnit.Framework;
class ClassName
{
[TestCase]
public void Test1() {}
}",
"Test1",
5,
13,
TestSettingsData.NUnitTestSettingsSimpleRegex,
TestSettingsData.TestMethodNameSimpleFormat,
TestSettingsData.TestMethodNameFormatSimpleRegexExamples
            };

            // A NUnit Test attribute is present (fully qualified name) and the method name does not have the expected format.
            yield return new object[]
            {
                @"
class ClassName
{
[NUnit.Framework.Test]
public void Test1() {}
}",
"Test1",
5,
13,
TestSettingsData.NUnitTestSettingsSimpleRegex,
TestSettingsData.TestMethodNameSimpleFormat,
TestSettingsData.TestMethodNameFormatSimpleRegexExamples
            };

            // A NUnit.Framework TestCase attribute is present (fully qualified name) and the method name does not have the expected format.
            yield return new object[]
            {
                @"
class ClassName
{
[NUnit.Framework.TestCase]
public void Test1() {}
}",
"Test1",
5,
13,
TestSettingsData.NUnitTestSettingsSimpleRegex,
TestSettingsData.TestMethodNameSimpleFormat,
TestSettingsData.TestMethodNameFormatSimpleRegexExamples
            };

            // A NUNit Test attribute is grouped along with another attribute and the method name does not have the expected format.
            yield return new object[]
            {
                @"using NUnit.Framework;
class ClassNameTests
{
[MyAttribute, Test]
public void Test1() {}
}",
"Test1",
5,
13,
TestSettingsData.NUnitTestSettingsSimpleRegex,
TestSettingsData.TestMethodNameSimpleFormat,
TestSettingsData.TestMethodNameFormatSimpleRegexExamples
            };

            // A NUnit TestCase attribute is grouped along with another attribute and the method name does not have the expected format.
            yield return new object[]
            {
                @"using NUnit.Framework;
class ClassNameTests
{
[MyAttribute, TestCase]
public void Test1() {}
}",
"Test1",
5,
13,
TestSettingsData.NUnitTestSettingsSimpleRegex,
TestSettingsData.TestMethodNameSimpleFormat,
TestSettingsData.TestMethodNameFormatSimpleRegexExamples
            };

            // A NUnit Test attribute is present along with another attribute and the method name does not have the expected format.
            yield return new object[]
            {
                @"using NUnit.Framework;
class ClassNameTests
{
[MyAttribute]
[Test]
public void Test1() {}
}",
"Test1",
6,
13,
TestSettingsData.NUnitTestSettingsSimpleRegex,
TestSettingsData.TestMethodNameSimpleFormat,
TestSettingsData.TestMethodNameFormatSimpleRegexExamples
            };

            // A NUNit TestCase attribute is present along with another attribute and the method name does not have the expected format.
            yield return new object[]
            {
                @"using NUnit.Framework;
class ClassNameTests
{
[MyAttribute]
[TestCase]
public void Test1() {}
}",
"Test1",
6,
13,
TestSettingsData.NUnitTestSettingsSimpleRegex,
TestSettingsData.TestMethodNameSimpleFormat,
TestSettingsData.TestMethodNameFormatSimpleRegexExamples
            };

            // A NUnit Test attribute is present and the method name does not have the expected format.
            //  A configuration settings is provided without a regex therefore the default regex should be used.
            yield return new object[]
            {
                @"using NUnit.Framework;
class ClassName
{
[Test]
public void TestOne() {}
}",
"TestOne",
5,
13,
TestSettingsData.NUnitSettings,
AnalyzersSettings.DefaultFormat,
AnalyzersSettings.TestMethodNameFormatDefaultRegexExamples
            };

            // A NUnit TestCase attribute is present and the method name does not have the expected format.
            //  A configuration settings is provided without a regex therefore the default regex should be used.
            yield return new object[]
            {
                @"using NUnit.Framework;
class ClassName
{
[TestCase]
public void TestOne() {}
}",
"TestOne",
5,
13,
TestSettingsData.NUnitSettings,
AnalyzersSettings.DefaultFormat,
AnalyzersSettings.TestMethodNameFormatDefaultRegexExamples
            };

            // A NUnit TestCase attribute is present and the method name does not have the expected format.
            //  A configuration settings is provided without format therefore the default format should be found in the message.
            yield return new object[]
            {
                @"using NUnit.Framework;
class ClassName
{
[TestCase]
public void Test1() {}
}",
"Test1",
5,
13,
TestSettingsData.NUnitTestSettingsSimpleRegexNoFormat,
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
