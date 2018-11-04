namespace UnitTestAnalyzers.Test.TestData
{
    using System.Collections;
    using System.Collections.Generic;

    internal class UseUnitTestsSuffixNUnitTestInvalidData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            // The class contains an NUnit Test method (using namespace) and the class name does not have the expected suffix.
            yield return new object[]
            {
                @"using NUnit.Framework;
class ClassName
{
[Test]
public void TestMethod() {}
}",
"ClassName", // Diagnostic message parameter
2,  // Violation line
7,  // Violation column.
TestSettingsData.NUnitSettings
            };

            // The class contains an NUnit TestCase method (using namespace) and the class name does not have the expected suffix.
            yield return new object[]
            {
                @"using NUnit.Framework;
class ClassName
{
[TestCase]
public void TestMethod() {}
}",
"ClassName",
2,
7,
TestSettingsData.NUnitSettings
            };

            // The class contains an NUnit Test method (fully qualified name) and the class name does not have the expected suffix.
            yield return new object[]
            {
                @"
class ClassName
{
[NUnit.Framework.Test]
public void TestMethod() {}
}",
"ClassName",
2,
7,
TestSettingsData.NUnitSettings
            };

            // The class contains an NUnit TestCase method (fully qualified name) and the class name does not have the expected suffix.
            yield return new object[]
            {
                @"
class ClassName
{
[NUnit.Framework.TestCase]
public void TestMethod() {}
}",
"ClassName",
2,
7,
TestSettingsData.NUnitSettings
            };

            // A Test attribute is grouped along with another attribute and the class name does not have the expected suffix.
            yield return new object[]
            {
                @"using NUnit.Framework;
class ClassNameFunctionalTests
{
[Test, MyAttribute]
public void TestMethod() {}
}",
"ClassNameFunctionalTests",
2,
7,
TestSettingsData.NUnitSettings
            };

            // A TestCase attribute is grouped along with another attribute and the class name does not have the expected suffix.
            yield return new object[]
            {
                @"using NUnit.Framework;
class ClassNameFunctionalTests
{
[TestCase, MyAttribute]
public void TestMethod() {}
}",
"ClassNameFunctionalTests",
2,
7,
TestSettingsData.NUnitSettings
            };

            // A Test attribute is present along with another attribute and the class name does not have the expected suffix.
            yield return new object[]
            {
                @"using NUnit.Framework;
class ClassNameFunctionalTests
{
[Test]
[MyAttribute]
public void TestMethod() {}
}",
"ClassNameFunctionalTests",
2,
7,
TestSettingsData.NUnitSettings
            };

            // A TestCase attribute is present along with another attribute and the class name does not have the expected suffix.
            yield return new object[]
            {
                @"using NUnit.Framework;
class ClassNameFunctionalTests
{
[TestCase]
[MyAttribute]
public void TestMethod() {}
}",
"ClassNameFunctionalTests",
2,
7,
TestSettingsData.NUnitSettings
            };

            // A class contains other method in addition to a Test method and the class name does not have the expected suffix.
            yield return new object[]
            {
                @"using NUnit.Framework;
class ClassNameFunctionalTests
{
public void AnotherMethod() {}
[Test]
public void TestMethod() {}
}",
"ClassNameFunctionalTests",
2,
7,
TestSettingsData.NUnitSettings
            };

            // A class contains other method in addition to a TestCase method and the class name does not have the expected suffix.
            yield return new object[]
            {
                @"using NUnit.Framework;
class ClassNameFunctionalTests
{
[TestCase]
[MyAttribute]
public void TestMethod() {}

public void AnotherMethod() {}
}",
"ClassNameFunctionalTests",
2,
7,
TestSettingsData.NUnitSettings
            };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
