namespace UnitTestAnalyzers.Test.TestData
{
    using System.Collections;
    using System.Collections.Generic;

    internal class UseUnitTestsSuffixMSTestInvalidData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            // A MSTest TestClass attribute is present (using statement) and the class name does not have the expected suffix.
            yield return new object[]
            {
                @"using Microsoft.VisualStudio.TestTools.UnitTesting;
[TestClass]
class ClassName
{}",
"ClassName", // Diagnostic message paramater
3,  // Violation line
7,  // Violation column.
TestSettingsData.NoSettings
            };

            // A MSTest TestClass attribute is present (fully qualified name) and the class name does not have the expected suffix.
            yield return new object[]
            {
                @"
[Microsoft.VisualStudio.TestTools.UnitTesting.TestClass]
class ClassNameTests
{}",
"ClassNameTests",
3,
7,
TestSettingsData.NoSettings
            };

            // A MSTest TestClass attribute is grouped along with another attribute and the class name does not have the suffix.
            yield return new object[]
            {
                @"using Microsoft.VisualStudio.TestTools.UnitTesting;
[MyAttribute, TestClass]
class ClassNameFunctionalTests
{}",
"ClassNameFunctionalTests",
3,
7,
TestSettingsData.NoSettings
            };

            // A MSTest TestClass attribute is present along with another attribute and the class name does not have the expected suffix.
            yield return new object[]
            {
                @"using Microsoft.VisualStudio.TestTools.UnitTesting;
[MyAttribute]
[TestClass]
class ClassNameUnitTest
{}",
"ClassNameUnitTest",
4,
7,
TestSettingsData.NoSettings
            };

            // A MSTest TestClass attribute is present along with another attribute and the class name does not have the expected suffix.
            // Additionally a configuration settings is provided to indicate the test framework to be used.
            yield return new object[]
            {
                @"using Microsoft.VisualStudio.TestTools.UnitTesting;
[MyAttribute]
[TestClass]
class ClassNameUnitTest
{}",
"ClassNameUnitTest",
4,
7,
TestSettingsData.MSTestSettings
            };

            // A MSTest TestClass attribute is present along with another attribute and the class name does not have the expected suffix.
            // Additionally a configuration settings is provided with a bad format. The analyzer should default to the MsTest framework in that case.
            yield return new object[]
            {
                @"using Microsoft.VisualStudio.TestTools.UnitTesting;
[MyAttribute]
[TestClass]
class ClassNameUnitTest
{}",
"ClassNameUnitTest",
4,
7,
TestSettingsData.InvalidSettings
            };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
