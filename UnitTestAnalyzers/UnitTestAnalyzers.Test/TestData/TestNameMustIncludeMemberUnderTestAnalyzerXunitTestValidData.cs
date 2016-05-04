﻿namespace UnitTestAnalyzers.Test.TestData
{
    using System.Collections;
    using System.Collections.Generic;

    internal class TestNameMustIncludeMemberUnderTestAnalyzerXunitTestValidData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            // The test method name does not match the Regex pattern in the member under test group.
            yield return new object[]
            {
                @"using Xunit;
class RegexUnitTests
{
[Fact]
public void TestCtor!_UsingNullString_ThrowsArgumentNullException() 
{
    Regex r = new Regex(null);
}
}",
TestSettingsData.XunitSettings
            };

            // The test method with empty body.
            yield return new object[]
            {
                @"using Xunit;
class RegexUnitTests
{
[Theory]
public void TestMethod_UsingNullString_ThrowsArgumentNullException() 
{
}
}",
TestSettingsData.XunitSettings
            };

            // The test method with body containing comments only.
            yield return new object[]
            {
                @"using Xunit;
class RegexUnitTests
{
[Fact]
public void TestMethod_UsingNullString_ThrowsArgumentNullException() 
{
// Comment.
}
}",
TestSettingsData.XunitSettings
            };

            // The test method with body containing a variable declaration only.
            yield return new object[]
            {
                @"using Xunit;
class RegexUnitTests
{
[Theory]
public void TestMethod_UsingNullString_ThrowsArgumentNullException() 
{
    string str = ""foo""
}
}",
TestSettingsData.XunitSettings
            };

            // The test method name does not match the Regex pattern in another part of the name.
            yield return new object[]
            {
                @"using Xunit;
class RegexUnitTests
{
[Fact]
public void TestCtor_NullString_ThrowsArgumentNullException() 
{
    Regex r = new Regex(null);
}
}",
TestSettingsData.XunitSettings
            };

            // A test method targeting a constructor hase its first part matching the name of the constructor under test.
            yield return new object[]
            {
                @"using Xunit;
class RegexUnitTests
{
[Fact]
public void Regex_UsingNullString_ThrowsArgumentNullException() 
{
    Regex r = new Regex(null);
}
}",
TestSettingsData.XunitSettings
            };

            // A test method targeting a property has its first part matching the name of the property under test.
            yield return new object[]
            {
                @"using Xunit;
class RegexUnitTests
{
[Theory]
public void Options_WhenOptionsIsNotProvided_ExpectsRetrievedOptionsEmpty() 
{
    Regex r = new Regex(""regex"");
    var options = r.Options;
}
}",
TestSettingsData.XunitSettings
            };

            // A test method targeting a method has its first part matching the name of the method under test.
            yield return new object[]
            {
                @"using Xunit;
class RegexUnitTests
{
[Fact]
public void IsMatch_UsingNonMatchingString_ExpectsFalse() 
{
    Regex r = new Regex(""regex"");
    var result = r.IsMatch(""Foo"");
}
}",
TestSettingsData.NoSettings
            };

            // A test method name has its member under test part matching the name of the method under test.
            // The analyzer settings provides a new regex that defines a group named 'memberUnderTestName'.
            yield return new object[]
            {
                @"using Xunit;
class RegexUnitTests
{
[Theory]
public void TestFoo_IsMatch() 
{
    Regex r = new Regex(""regex"");
    var result = r.IsMatch(""Foo"");
}
}",
TestSettingsData.XunitTestSettingsMemberUnderTestRegexFormat
            };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
