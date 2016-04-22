namespace UnitTestAnalyzers.Test.TestData
{
    using System.Collections;
    using System.Collections.Generic;

    internal class UseUnitTestsSuffixMSTestValidData : IEnumerable<string[]>
    {
        public IEnumerator<string[]> GetEnumerator()
        {
            // No class attribute present.
            yield return new[] {@"class ClassName {}", TestSettingsData.NoSettings};

            // A random class attribute present.
            yield return new[] {@"
[MyAttribute]
class ClassName
{}",
TestSettingsData.NoSettings};

            // Multiple random class attributes present.
            yield return new[] {@"
[MyAttributeOne]
[MyAttributeTwo]
class ClassName
{}",
TestSettingsData.NoSettings};

            // A non-MSTest TestClass attribute present (fully qualified name).
            yield return new[] {@"
[UnitTestAnalyzers.Test.Attributes.TestClass]
class ClassName
{}",
TestSettingsData.NoSettings};

            //  A non-MSTest TestClass attribute present (using statement).
            yield return new[] {@"using UnitTestAnalyzers.Test.Attributes;
[TestClass]
class ClassName
{}",
TestSettingsData.NoSettings};

            //  A MSTest TestClass attribute is present (using statement) and the class name has the expected suffix.
            yield return new[] {@"using Microsoft.VisualStudio.TestTools.UnitTesting;
[TestClass]
class ClassNameUnitTests
{}",
TestSettingsData.NoSettings};

            //  A MSTest TestClass attribute is present (fully qualified name) and the class name has the expected suffix.
            yield return new[] {@"
[Microsoft.VisualStudio.TestTools.UnitTesting.TestClass]
class ClassNameUnitTests
{}",
TestSettingsData.NoSettings};

            //  A MSTest TestClass attribute is present (fully qualified name) and the class name has the expected suffix.
            // Additionally a configuration settings is provided indicating that the test framework is MsTest.
            yield return new[] {@"
[Microsoft.VisualStudio.TestTools.UnitTesting.TestClass]
class ClassNameUnitTests
{}",
TestSettingsData.MSTestSettings};

            //  A MSTest TestClass attribute is present (fully qualified name) and the class name has the expected suffix.
            // Additionally a configuration settings with bad format is provided. The analyzer should default the framework to MsTest.
            yield return new[] {@"
[Microsoft.VisualStudio.TestTools.UnitTesting.TestClass]
class ClassNameUnitTests
{}",
TestSettingsData.InvalidSettings};

            //  A MSTest TestClass attribute is grouped along with another attribute and the class name has the expected suffix.
            yield return new[] {@"using Microsoft.VisualStudio.TestTools.UnitTesting;
[MyAttribute, TestClass]
class ClassNameUnitTests
{}",
TestSettingsData.NoSettings};

            //  A MSTest TestClass attribute is present along with another attribute and the class name has the expected suffix.
            yield return new[] {@"using Microsoft.VisualStudio.TestTools.UnitTesting;
[MyAttribute]
[TestClass]
class ClassNameUnitTests
{}",
TestSettingsData.NoSettings};

        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
