namespace UnitTestAnalyzers.Test.TestData
{
    using System.Collections;
    using System.Collections.Generic;

    internal class UseCorrectTestNameFormatXunitTestValidData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            // No method attribute is present.
            yield return new object[]
            {
                @"
class ClassName
{
public void Test1() {}
}",
TestSettingsData.XunitTestSettingsSimpleRegex
            };

            // A random method attribute is present.
            yield return new object[]
            {
                @"
class ClassName
{
[MyAttribute]
public void Test1() {}
}",
TestSettingsData.XunitTestSettingsSimpleRegex
            };

            // A non-Xunit Fact attribute is present (fully qualified name).
            yield return new object[]
            {
                @"
class ClassName
{
[UnitTestAnalyzers.Test.Attributes.Fact]
public void Test1() {}
}",
TestSettingsData.XunitTestSettingsSimpleRegex
            };

            // A non-Xunit Theory attribute is present (fully qualified name).
            yield return new object[]
            {
                @"
class ClassName
{
[UnitTestAnalyzers.Test.Attributes.Theory]
public void Test1() {}
}",
TestSettingsData.XunitTestSettingsSimpleRegex
            };

            // A non-Xunit Fact attribute is present (using statement).
            yield return new object[]
            {
                @"using UnitTestAnalyzers.Test.Attributes;
class ClassNameTests
{
[Fact]
public void Test1() {}
}",
TestSettingsData.XunitTestSettingsSimpleRegex
            };

            // A non-Xunit Theory attribute is present (using statement).
            yield return new object[]
            {
                @"using UnitTestAnalyzers.Test.Attributes;
class ClassNameTests
{
[Theory]
public void Test1() {}
}",
TestSettingsData.XunitTestSettingsSimpleRegex
            };

            // A Xunit Fact attribute is present (using statement) and the method name has the expected format.
            // The provided settings does not contain a regex therefore the default format is enforced.
            yield return new object[]
            {
                @"using Xunit;
class ClassName
{
[Fact]
public void TestOne_UsingFoo_ThrowsResult() {}
}",
TestSettingsData.XunitSettings
            };

            // A Xunit Theory attribute is present (using statement) and the method name has the expected format.
            // The provided settings does not contain a regex therefore the default format is enforced.
            yield return new object[]
            {
                @"using Xunit;
class ClassName
{
[Theory]
public void TestOne_UsingFoo_ThrowsResult() {}
}",
TestSettingsData.XunitSettings
            };

            // A Xunit Fact attribute is present (fully qualified named) and the method name has the expected format.
            // The provided settings does not contain a regex therefore the default format is enforced.
            yield return new object[]
            {
                @"
class ClassName
{
[Xunit.Fact]
public void TestOne_WhenFoo_ExpectsResult() {}
}",
TestSettingsData.XunitSettings
            };

            // A Xunit Theory attribute is present (fully qualified named) and the method name has the expected format.
            // The provided settings does not contain a regex therefore the default format is enforced.p
            yield return new object[]
            {
                @"
class ClassName
{
[Xunit.Theory]
public void TestOne_WhenFoo_ExpectsResult() {}
}",
TestSettingsData.XunitSettings
            };

            // A Xunit Fact attribute is present (using statement) and the method name has the expected format.
            //  Additionally a settings is provided redefining the test name regex.
            yield return new object[]
            {
                @"using Microsoft.VisualStudio.TestTools.UnitTesting;
class ClassName
{
[TestMethod]
public void TestOne() {}
}",
TestSettingsData.XunitTestSettingsSimpleRegex
            };

            // A Xunit Theory attribute is present (using statement) and the method name has the expected format.
            //  Additionally a settings is provided redefining the test name regex.
            yield return new object[]
            {
                @"using Microsoft.VisualStudio.TestTools.UnitTesting;
class ClassName
{
[Theory]
public void TestOne() {}
}",
TestSettingsData.XunitTestSettingsSimpleRegex
            };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
