namespace UnitTestAnalyzers.Test.TestData
{
    using System.Collections;
    using System.Collections.Generic;

    internal class UseCorrectTestNameFormatMSTestValidData : IEnumerable<object[]>
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
TestSettingsData.NoSettings
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
TestSettingsData.NoSettings
            };

            // A non-MSTest TestMethod attribute is present (fully qualified name).
            yield return new object[]
            {
                @"
class ClassName
{
[UnitTestAnalyzers.Test.Attributes.TestMethod]
public void Test1() {}
}",
TestSettingsData.NoSettings
            };

            // A non-MSTest TestMethod attribute is present (using statement).
            yield return new object[]
            {
                @"using UnitTestAnalyzers.Test.Attributes;
class ClassNameTests
{
[TestMethod]
public void Test1() {}
}",
TestSettingsData.NoSettings
            };

            // A MSTest TestMethod attribute is present (using statement) and the method name has the expected format.
            yield return new object[]
            {
                @"using Microsoft.VisualStudio.TestTools.UnitTesting;
class ClassName
{
[TestMethod]
public void TestOne_WhenFoo_ExpectsResult() {}
}",
TestSettingsData.NoSettings
            };

            // A MSTest TestMethod attribute is present (fully qualified named) and the method name has the expected format.
            yield return new object[]
            {
                @"
class ClassName
{
[Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod]
public void TestOne_WhenFoo_ExpectsResult() {}
}",
TestSettingsData.NoSettings
            };

            // A MSTest TestMethod attribute is present (using statement) and the method name has the expected format.
            //  Additionally a settings file is provided that does not redefine the test method name regex.
            yield return new object[]
            {
                @"using Microsoft.VisualStudio.TestTools.UnitTesting;
class ClassName
{
[TestMethod]
public void TestOne_WhenFoo_ExpectsResult() {}
}",
TestSettingsData.MSTestSettings
            };

            // A MSTest TestMethod attribute is present (using statement) and the method name has the expected format.
            //  Additionally an invalid test settings is provided, therefore the default settings should be used.
            yield return new object[]
            {
                @"using Microsoft.VisualStudio.TestTools.UnitTesting;
class ClassName
{
[TestMethod]
public void TestOne_WhenFoo_ExpectsResult() {}
}",
TestSettingsData.InvalidSettings
            };

            // A MSTest TestMethod attribute is present (using statement) and the method name has the expected format.
            //  Additionally a settings is provided redefining the test name regex.
            yield return new object[]
            {
                @"using Microsoft.VisualStudio.TestTools.UnitTesting;
class ClassName
{
[TestMethod]
public void TestOne() {}
}",
TestSettingsData.MSTestSettingsSimpleRegex
            };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
