namespace UnitTestAnalyzers.Test.TestData
{
    using System.Collections;
    using System.Collections.Generic;

    internal class UseUnitTestsSuffixXunitTestValidData : IEnumerable<string[]>
    {
        public IEnumerator<string[]> GetEnumerator()
        {
            // No method in class.
            yield return new[] { @"class ClassName {}", TestSettingsData.XunitSettings };

            // No test method (Fact or Theory) in class. A method is present without any attribute.
            yield return new[]
            {
                @"
class ClassName
{
public void SomeMethod() {}
}",
TestSettingsData.XunitSettings
            };

            // No test method (Fact or Theory) in class. A method is present without some attribute.
            yield return new[]
            {
                @"
class ClassName
{
[MyAttribute]
public void SomeMethod() {}
}",
TestSettingsData.XunitSettings
            };

            // A non-xUnit Fact attribute present (fully qualified name).
            yield return new[]
            {
                @"
class ClassName
{
[UnitTestAnalyzers.Test.Attributes.Fact]
public void MyMethod() {}
}",
TestSettingsData.XunitSettings
            };

            // A non-xUnit Theory attribute present (fully qualified name).
            yield return new[]
            {
                @"
class ClassName
{
[UnitTestAnalyzers.Test.Attributes.Theory]
public void MyMethod() {}
}",
TestSettingsData.XunitSettings
            };

            // A non-xUnit Fact attribute present (using statement).
            yield return new[]
            {
                @"using UnitTestAnalyzers.Test.Attributes;
class ClassName
{
[Fact]
public void MyMethod() {}
}",
TestSettingsData.XunitSettings
            };

            // A non-xUnit Theory attribute present (using statement).
            yield return new[]
            {
                @"using UnitTestAnalyzers.Test.Attributes;
class ClassName
{
[Theory]
public void MyMethod() {}
}",
TestSettingsData.XunitSettings
            };

            // A xUnit Fact method is present (fully qualified name) and the class name has the expected suffix.
            yield return new[]
            {
                @"
class ClassNameUnitTests
{
[Xunit.Fact]
public void TestMethod() {}
}",
TestSettingsData.XunitSettings
            };

            // A xUnit Theory method is present (fully qualified name) and the class name has the expected suffix.
            yield return new[]
            {
                @"
class ClassNameUnitTests
{
[Xunit.Theory]
public void TestMethod() {}
}",
TestSettingsData.XunitSettings
            };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
