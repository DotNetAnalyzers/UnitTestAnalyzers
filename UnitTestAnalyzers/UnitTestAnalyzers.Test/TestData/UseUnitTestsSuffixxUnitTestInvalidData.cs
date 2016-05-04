namespace UnitTestAnalyzers.Test.TestData
{
    using System.Collections;
    using System.Collections.Generic;

    internal class UseUnitTestsSuffixXunitTestInvalidData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            // The class contains an xUnit Fact method (using namespace) and the class name does not have the expected suffix.
            yield return new object[]
            {
                @"using Xunit;
class ClassName
{
[Fact]
public void TestMethod() {}
}",
"ClassName", // Diagnostic message parameter
2,  // Violation line
7,  // Violation column.
TestSettingsData.XunitSettings
            };

            // The class contains an xUnit Theory method (using namespace) and the class name does not have the expected suffix.
            yield return new object[]
            {
                @"using Xunit;
class ClassName
{
[Theory]
public void TestMethod() {}
}",
"ClassName",
2,
7,
TestSettingsData.XunitSettings
            };

            // The class contains an xUnit Fact method (fully qualified name) and the class name does not have the expected suffix.
            yield return new object[]
            {
                @"
class ClassName
{
[Xunit.Fact]
public void TestMethod() {}
}",
"ClassName",
2,
7,
TestSettingsData.XunitSettings
            };

            // The class contains an xUnit Theory method (fully qualified name) and the class name does not have the expected suffix.
            yield return new object[]
            {
                @"
class ClassName
{
[Xunit.Theory]
public void TestMethod() {}
}",
"ClassName",
2,
7,
TestSettingsData.XunitSettings
            };

            // A Fact attribute is grouped along with another attribute and the class name does not have the expected suffix.
            yield return new object[]
            {
                @"using Xunit;
class ClassNameFunctionalTests
{
[Fact, MyAttribute]
public void TestMethod() {}
}",
"ClassNameFunctionalTests",
2,
7,
TestSettingsData.XunitSettings
            };

            // A Theory attribute is grouped along with another attribute and the class name does not have the expected suffix.
            yield return new object[]
            {
                @"using Xunit;
class ClassNameFunctionalTests
{
[Theory, MyAttribute]
public void TestMethod() {}
}",
"ClassNameFunctionalTests",
2,
7,
TestSettingsData.XunitSettings
            };

            // A Fact attribute is present along with another attribute and the class name does not have the expected suffix.
            yield return new object[]
            {
                @"using Xunit;
class ClassNameFunctionalTests
{
[Fact]
[MyAttribute]
public void TestMethod() {}
}",
"ClassNameFunctionalTests",
2,
7,
TestSettingsData.XunitSettings
            };

            // A Theory attribute is present along with another attribute and the class name does not have the expected suffix.
            yield return new object[]
            {
                @"using Xunit;
class ClassNameFunctionalTests
{
[Theory]
[MyAttribute]
public void TestMethod() {}
}",
"ClassNameFunctionalTests",
2,
7,
TestSettingsData.XunitSettings
            };

            // A class contains other method in addition to a Fact method and the class name does not have the expected suffix.
            yield return new object[]
            {
                @"using Xunit;
class ClassNameFunctionalTests
{
public void AnotherMethod() {}
[Fact]
public void TestMethod() {}
}",
"ClassNameFunctionalTests",
2,
7,
TestSettingsData.XunitSettings
            };

            // A class contains other method in addition to a Theory method and the class name does not have the expected suffix.
            yield return new object[]
            {
                @"using Xunit;
class ClassNameFunctionalTests
{
[Theory]
[MyAttribute]
public void TestMethod() {}

public void AnotherMethod() {}
}",
"ClassNameFunctionalTests",
2,
7,
TestSettingsData.XunitSettings
            };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
