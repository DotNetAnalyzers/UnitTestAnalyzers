namespace UnitTestAnalyzers.Test.TestData
{
    using System.Collections;
    using System.Collections.Generic;

    internal class TestNameMustIncludeMemberUnderTestAnalyzerNUnitTestInvalidData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            // A test method targeting a constructor does not have its first part matching the name of the constructor under test.
            yield return new object[]
            {
                @"using NUnit.Framework;
class RegexUnitTests
{
[Test]
public void TestCtor_UsingNullString_ThrowsArgumentNullException() 
{
    Regex r = new Regex(null);
}
}",
"TestCtor_UsingNullString_ThrowsArgumentNullException", // Diagnostic message paramater
"TestCtor", // Diagnostic message parameter
5,  // Violation line
13,  // Violation column.
TestSettingsData.NUnitSettings
            };

            // A test method targeting a property does not have its first part matching the name of the property under test.
            yield return new object[]
            {
                @"using NUnit.Framework;
class RegexUnitTests
{
[Test]
public void TestProperty_WhenOptionsIsNotProvided_ExpectsRetrievedOptionsEmpty() 
{
    Regex r = new Regex(""regex"");
    var options = r.Options;
}
}",
"TestProperty_WhenOptionsIsNotProvided_ExpectsRetrievedOptionsEmpty",
"TestProperty",
5,
13,
TestSettingsData.NUnitSettings
            };

            // A test method targeting a method does not have its first part matching the name of the method under test.
            yield return new object[]
            {
                @"using NUnit.Framework;
class RegexUnitTests
{
[Test]
public void TestMethod_UsingNonMatchingString_ExpectsFalse() 
{
    Regex r = new Regex(""regex"");
    var result = r.IsMatch(""Foo"");
}
}",
"TestMethod_UsingNonMatchingString_ExpectsFalse",
"TestMethod",
5,
13,
TestSettingsData.NUnitSettings
            };

            // A test method targeting a method does not have its first part matching the name of the method under test.
            // Instead that part matches the name of a variable within the test body.
            yield return new object[]
            {
                @"using NUnit.Framework;
class RegexUnitTests
{
[Test]
public void result_UsingNonMatchingString_ExpectsFalse() 
{
    Regex r = new Regex(""regex"");
    var result = r.IsMatch(""Foo"");
}
}",
"result_UsingNonMatchingString_ExpectsFalse",
"result",
5,
13,
TestSettingsData.NUnitSettings
            };

            // A test method targeting a method does not have its first part matching the name of the method under test.
            // Instead that part matches the name of a parameter within the test body.
            yield return new object[]
            {
                @"using NUnit.Framework;
class RegexUnitTests
{
[Test]
public void Foo_UsingNonMatchingString_ExpectsFalse() 
{
    Regex r = new Regex(""regex"");
    var result = r.IsMatch(""Foo"");
}
}",
"Foo_UsingNonMatchingString_ExpectsFalse",
"Foo",
5,
13,
TestSettingsData.NUnitSettings
            };

            // A test method targeting a method does not have its first part matching the name of the method under test.
            // Instead that part matches a comment withing the method body.
            yield return new object[]
            {
                @"using NUnit.Framework;
class RegexUnitTests
{
[Test]
public void Init_UsingNonMatchingString_ExpectsFalse() 
{
    // Init.
    Regex r = new Regex(""regex"");
    var result = r.IsMatch(""Foo"");
}
}",
"Init_UsingNonMatchingString_ExpectsFalse",
"Init",
5,
13,
TestSettingsData.NUnitSettings
            };

            // A test method name not have its member under test part matching the name of the method under test.
            // The analyzer settings provides a new regex that defines a group named 'memberUnderTestName'.
            yield return new object[]
            {
                @"using NUnit.Framework;
class RegexUnitTests
{
[Test]
public void TestFoo_Method() 
{
    Regex r = new Regex(""regex"");
    var result = r.IsMatch(""Foo"");
}
}",
"TestFoo_Method",
"Method",
5,
13,
TestSettingsData.NUnitTestSettingsMemberUnderTestRegexFormat
            };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
