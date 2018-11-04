namespace UnitTestAnalyzers.Test.TestData
{
    using System.Collections;
    using System.Collections.Generic;

    internal class UseAAACommentsAnalyzerNUnitTestValidData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            // A test method with empty body.
            yield return new object[]
            {
                @"using NUnit.Framework;
class RegexUnitTests
{
[Test]
public void Regex_UsingNullString_ThrowsArgumentNullException() 
{
}
}",
TestSettingsData.NUnitSettings
            };

            // A test method that does not invoke a member or constructor.
            yield return new object[]
            {
                @"using NUnit.Framework;
class RegexUnitTests
{
[Test]
public void Regex_UsingNullString_ThrowsArgumentNullException() 
{
     string foo = ""foo"";
}
}",
TestSettingsData.NUnitSettings
            };

            // A test method with the // Arrange. // Act. // Assert. comments.
            // Each AAA comment appears only once and in the expected order.
            // The first line of the test body is // Arrange.
            yield return new object[]
            {
                @"using NUnit.Framework;
class RegexUnitTests
{
[Test]
public void Split_UsingSplitableString_ExpectsStringIsSplit() 
{
   // Arrange.
   string foo = ""foo foo"";

   // Act.
   var parts = foo.Split(' ')[0];

   // Assert.
   Assert.IsNotNull(parts);
}
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
