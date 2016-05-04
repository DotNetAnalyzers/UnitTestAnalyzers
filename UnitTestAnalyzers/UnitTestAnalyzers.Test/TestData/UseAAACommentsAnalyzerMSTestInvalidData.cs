namespace UnitTestAnalyzers.Test.TestData
{
    using System.Collections;
    using System.Collections.Generic;

    internal class UseAAACommentsAnalyzerMSTestInvalidData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            // A test method does not include any of the ArrangeActAssert  comments.
            yield return new object[]
            {
                @"using Microsoft.VisualStudio.TestTools.UnitTesting;
class RegexUnitTests
{
[TestMethod]
public void Regex_UsingNullString_ThrowsArgumentNullException() 
{
    Regex r = new Regex(null);
}
}",
"Regex_UsingNullString_ThrowsArgumentNullException", // Diagnostic message paramater
5,  // Violation line
13,  // Violation column.
TestSettingsData.NoSettings,
false,
false
            };

            // A test method has only the // Arrange. comment.
            yield return new object[]
            {
                @"using Microsoft.VisualStudio.TestTools.UnitTesting;
class RegexUnitTests
{
[TestMethod]
public void Regex_UsingNullString_ThrowsArgumentNullException() 
{
    // Arrange.
    string str = null;

    Regex r = new Regex(str);
}
}",
"Regex_UsingNullString_ThrowsArgumentNullException",
5,
13,
TestSettingsData.NoSettings,
false,
true
            };

            // A test method has only the // Act. comment.
            yield return new object[]
            {
                @"using Microsoft.VisualStudio.TestTools.UnitTesting;
class RegexUnitTests
{
[TestMethod]
public void Regex_UsingNullString_ThrowsArgumentNullException() 
{
    string str = null;

    // Act.
    Regex r = new Regex(str);
}
}",
"Regex_UsingNullString_ThrowsArgumentNullException",
5,
13,
TestSettingsData.NoSettings,
false,
false
            };

            // A test method has only the // Assert. comment.
            yield return new object[]
            {
                @"using Microsoft.VisualStudio.TestTools.UnitTesting;
class RegexUnitTests
{
[TestMethod]
public void Regex_UsingNullString_ThrowsArgumentNullException() 
{
    string str = null;

    Regex r = new Regex(str);
 
    // Assert.x
}
}",
"Regex_UsingNullString_ThrowsArgumentNullException",
5,
13,
TestSettingsData.NoSettings,
false,
false
            };

            // A test method does not have the // Arrange. comment.
            yield return new object[]
            {
                @"using Microsoft.VisualStudio.TestTools.UnitTesting;
class RegexUnitTests
{
[TestMethod]
public void Regex_UsingNullString_ThrowsArgumentNullException() 
{
    string str = null;

    // Act.
    Regex r = new Regex(str);

    // Assert.
}
}",
"Regex_UsingNullString_ThrowsArgumentNullException",
5,
13,
TestSettingsData.NoSettings,
false,
false
            };

            // A test method does not have the // Act. comment.
            yield return new object[]
            {
                @"using Microsoft.VisualStudio.TestTools.UnitTesting;
class RegexUnitTests
{
[TestMethod]
public void Regex_UsingNullString_ThrowsArgumentNullException() 
{
    // Arrange.
    string str = null;

    Regex r = new Regex(null);

   // Assert.
}
}",
"Regex_UsingNullString_ThrowsArgumentNullException",
5,
13,
TestSettingsData.NoSettings,
false,
true
            };

            // A test method does not include the // Assert. comment.
            yield return new object[]
            {
                @"using Microsoft.VisualStudio.TestTools.UnitTesting;
class RegexUnitTests
{
[TestMethod]
public void Regex_UsingNullString_ThrowsArgumentNullException() 
{
    // Arrange.
    string str = null;

   // Act.
    Regex r = new Regex(null);
}
}",
"Regex_UsingNullString_ThrowsArgumentNullException",
5,
13,
TestSettingsData.NoSettings,
false,
true
            };

            // A test method has multiple // Arrange. comments
            yield return new object[]
            {
                @"using Microsoft.VisualStudio.TestTools.UnitTesting;
class RegexUnitTests
{
[TestMethod]
public void Regex_UsingNullString_ThrowsArgumentNullException() 
{
    // Arrange.
    string str = null;

    // Arrange.

    // Act.
    Regex r = new Regex(null);

   // Assert.
}
}",
"Regex_UsingNullString_ThrowsArgumentNullException",
5,
13,
TestSettingsData.NoSettings,
false,
true
            };

            // A test method has multiple // Act. comments
            yield return new object[]
            {
                @"using Microsoft.VisualStudio.TestTools.UnitTesting;
class RegexUnitTests
{
[TestMethod]
public void Regex_UsingNullString_ThrowsArgumentNullException() 
{
    // Arrange.
    string str = null;

    // Act.
    Regex r = new Regex(null);

   // Act.
   // Assert.
}
}",
"Regex_UsingNullString_ThrowsArgumentNullException",
5,
13,
TestSettingsData.NoSettings,
false,
true
            };

            // A test method has multiple // Assert. comments
            yield return new object[]
            {
                @"using Microsoft.VisualStudio.TestTools.UnitTesting;
class RegexUnitTests
{
[TestMethod]
public void Regex_UsingNullString_ThrowsArgumentNullException() 
{
    // Arrange.
    string str = null;

    // Act.
    Regex r = new Regex(null);

   // Assert.

   // Assert.
}
}",
"Regex_UsingNullString_ThrowsArgumentNullException",
5,
13,
TestSettingsData.NoSettings,
false,
true
            };

            // A test method has the ArrangeActAssert comments in the wrong order.
            yield return new object[]
            {
                @"using Microsoft.VisualStudio.TestTools.UnitTesting;
class RegexUnitTests
{
[TestMethod]
public void Regex_UsingNullString_ThrowsArgumentNullException() 
{
    // Arrange.
    string str = null;

    // Assert.
    Regex r = new Regex(null);

   // Act.
}
}",
"Regex_UsingNullString_ThrowsArgumentNullException",
5,
13,
TestSettingsData.NoSettings,
false,
true
            };

            // A test method has the ArrangeActAssert comments in the wrong order.
            yield return new object[]
            {
                @"using Microsoft.VisualStudio.TestTools.UnitTesting;
class RegexUnitTests
{
[TestMethod]
public void Regex_UsingNullString_ThrowsArgumentNullException() 
{
    // Act.
    string str = null;

    // Arrange.
    Regex r = new Regex(null);

   // Assert.
}
}",
"Regex_UsingNullString_ThrowsArgumentNullException",
5,
13,
TestSettingsData.NoSettings,
false,
false
            };

            // A test method has the ArrangeActAssert comments in the wrong order.
            yield return new object[]
            {
                @"using Microsoft.VisualStudio.TestTools.UnitTesting;
class RegexUnitTests
{
[TestMethod]
public void Regex_UsingNullString_ThrowsArgumentNullException() 
{
    // Act.
    string str = null;

    // Assert.
    Regex r = new Regex(null);

   // Arrange.
}
}",
"Regex_UsingNullString_ThrowsArgumentNullException",
5,
13,
TestSettingsData.NoSettings,
false,
false
            };

            // A test method has the ArrangeActAssert comments in the wrong order.
            yield return new object[]
            {
                @"using Microsoft.VisualStudio.TestTools.UnitTesting;
class RegexUnitTests
{
[TestMethod]
public void Regex_UsingNullString_ThrowsArgumentNullException() 
{
    // Assert.
    string str = null;

    // Act.
    Regex r = new Regex(null);

   // Arrange.
}
}",
"Regex_UsingNullString_ThrowsArgumentNullException",
5,
13,
TestSettingsData.NoSettings,
false,
false
            };

            // A test method has the ArrangeActAssert comments in the wrong order.
            yield return new object[]
            {
                @"using Microsoft.VisualStudio.TestTools.UnitTesting;
class RegexUnitTests
{
[TestMethod]
public void Regex_UsingNullString_ThrowsArgumentNullException() 
{
    // Assert.
    string str = null;

    // Arrange.
    Regex r = new Regex(null);

   // Act.
}
}",
"Regex_UsingNullString_ThrowsArgumentNullException",
5,
13,
TestSettingsData.NoSettings,
false,
false
            };

            // A test method does not have the // Arrange. comment as the first line of its body.
            yield return new object[]
            {
                @"using Microsoft.VisualStudio.TestTools.UnitTesting;
class RegexUnitTests
{
[TestMethod]
public void Regex_UsingNullString_ThrowsArgumentNullException() 
{

    // Arrange.
    string str = null;

    // Act.
    Regex r = new Regex(null);

   // Assert.
}
}",
"Regex_UsingNullString_ThrowsArgumentNullException",
5,
13,
TestSettingsData.NoSettings,
true,
false
            };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
