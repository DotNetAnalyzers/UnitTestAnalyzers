namespace UnitTestAnalyzers.Test.TestData
{
    using System.Collections;
    using System.Collections.Generic;

    internal class UseUnitTestsSuffixNUnitTestValidData : IEnumerable<string[]>
    {
        public IEnumerator<string[]> GetEnumerator()
        {
            // No method in class.
            yield return new[] { @"class ClassName {}", TestSettingsData.NUnitSettings };

            // No test method (Test or TestCase) in class. A method is present without any attribute.
            yield return new[]
            {
                @"
class ClassName
{
public void SomeMethod() {}
}",
TestSettingsData.NUnitSettings
            };

            // No test method (Test or TestCase) in class. A method is present without some attribute.
            yield return new[]
            {
                @"
class ClassName
{
[MyAttribute]
public void SomeMethod() {}
}",
TestSettingsData.NUnitSettings
            };

            // A non-NUnit Test attribute present (fully qualified name).
            yield return new[]
            {
                @"
class ClassName
{
[UnitTestAnalyzers.Test.Attributes.Test]
public void MyMethod() {}
}",
TestSettingsData.NUnitSettings
            };

            // A non-NUnit TestCase attribute present (fully qualified name).
            yield return new[]
            {
                @"
class ClassName
{
[UnitTestAnalyzers.Test.Attributes.TestCase]
public void MyMethod() {}
}",
TestSettingsData.NUnitSettings
            };

            // A non-NUnit Test attribute present (using statement).
            yield return new[]
            {
                @"using UnitTestAnalyzers.Test.Attributes;
class ClassName
{
[Test]
public void MyMethod() {}
}",
TestSettingsData.NUnitSettings
            };

            // A non-NUnit TestCase attribute present (using statement).
            yield return new[]
            {
                @"using UnitTestAnalyzers.Test.Attributes;
class ClassName
{
[TestCase]
public void MyMethod() {}
}",
TestSettingsData.NUnitSettings
            };

            // A NUnit Test method is present (fully qualified name) and the class name has the expected suffix.
            yield return new[]
            {
                @"
class ClassNameUnitTests
{
[NUnit.Framework.Test]
public void TestMethod() {}
}",
TestSettingsData.NUnitSettings
            };

            // A NUnit TestCase method is present (fully qualified name) and the class name has the expected suffix.
            yield return new[]
            {
                @"
class ClassNameUnitTests
{
[NUnit.Framework.TestCase]
public void TestMethod() {}
}",
TestSettingsData.NUnitSettings
            };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
