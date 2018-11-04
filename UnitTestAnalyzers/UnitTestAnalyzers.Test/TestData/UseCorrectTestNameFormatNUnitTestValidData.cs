namespace UnitTestAnalyzers.Test.TestData
{
    using System.Collections;
    using System.Collections.Generic;

    internal class UseCorrectTestNameFormatNUnitTestValidData : IEnumerable<object[]>
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
TestSettingsData.NUnitTestSettingsSimpleRegex
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
TestSettingsData.NUnitTestSettingsSimpleRegex
            };

            // A NUnit Test attribute is present (fully qualified name).
            yield return new object[]
            {
                @"
class ClassName
{
[UnitTestAnalyzers.Test.Attributes.Test]
public void Test1() {}
}",
TestSettingsData.NUnitTestSettingsSimpleRegex
            };

            // A non-NUnit TestCase attribute is present (fully qualified name).
            yield return new object[]
            {
                @"
class ClassName
{
[UnitTestAnalyzers.Test.Attributes.TestCase]
public void Test1() {}
}",
TestSettingsData.NUnitTestSettingsSimpleRegex
            };

            // A non-NUnit Test attribute is present (using statement).
            yield return new object[]
            {
                @"using UnitTestAnalyzers.Test.Attributes;
class ClassNameTests
{
[Test]
public void Test1() {}
}",
TestSettingsData.NUnitTestSettingsSimpleRegex
            };

            // A non-NUnit TestCase attribute is present (using statement).
            yield return new object[]
            {
                @"using UnitTestAnalyzers.Test.Attributes;
class ClassNameTests
{
[TestCase]
public void Test1() {}
}",
TestSettingsData.NUnitTestSettingsSimpleRegex
            };

            // A NUnit Test attribute is present (using statement) and the method name has the expected format.
            // The provided settings does not contain a regex therefore the default format is enforced.
            yield return new object[]
            {
                @"using NUnit.Framework;
class ClassName
{
[Test]
public void TestOne_UsingFoo_ThrowsResult() {}
}",
TestSettingsData.NUnitSettings
            };

            // A NUnit TestCase attribute is present (using statement) and the method name has the expected format.
            // The provided settings does not contain a regex therefore the default format is enforced.
            yield return new object[]
            {
                @"using NUnit.Framework;
class ClassName
{
[TestCase]
public void TestOne_UsingFoo_ThrowsResult() {}
}",
TestSettingsData.NUnitSettings
            };

            // A NUnit Test attribute is present (fully qualified named) and the method name has the expected format.
            // The provided settings does not contain a regex therefore the default format is enforced.
            yield return new object[]
            {
                @"
class ClassName
{
[NUnit.Framework.Test]
public void TestOne_WhenFoo_ExpectsResult() {}
}",
TestSettingsData.NUnitSettings
            };

            // A NUnit TestCase attribute is present (fully qualified named) and the method name has the expected format.
            // The provided settings does not contain a regex therefore the default format is enforced.p
            yield return new object[]
            {
                @"
class ClassName
{
[NUnit.Framework.TestCase]
public void TestOne_WhenFoo_ExpectsResult() {}
}",
TestSettingsData.NUnitSettings
            };

            // A NUnit Test attribute is present (using statement) and the method name has the expected format.
            //  Additionally a settings is provided redefining the test name regex.
            yield return new object[]
            {
                @"using NUnit.Framework;
class ClassName
{
[Test]
public void TestOne() {}
}",
TestSettingsData.NUnitTestSettingsSimpleRegex
            };

            // A NUnit TestCase attribute is present (using statement) and the method name has the expected format.
            //  Additionally a settings is provided redefining the test name regex.
            yield return new object[]
            {
                @"using using NUnit.Framework;
class ClassName
{
[TestCase]
public void TestOne() {}
}",
TestSettingsData.NUnitTestSettingsSimpleRegex
            };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
