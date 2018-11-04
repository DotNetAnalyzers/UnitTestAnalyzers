namespace UnitTestAnalyzers.Test.Design
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Diagnostics;
    using Settings.ObjectModel;
    using TestData;
    using TestHelper;
    using Xunit;

    public class TestNameMustIncludeMemberUnderTestFunctionalTests : DiagnosticVerifier
    {
        private string settings;

        [Theory]
        [ClassData(typeof(TestNameMustIncludeMemberUnderTestAnalyzerMSTestInvalidData))]
        public async Task IncludeMemberUnderTestInTestName_MSTestCodeWithViolation_ExpectsDiagnostic(string testCode, string testMethodName, string testMethodNameMemberUnderTest, int violantioLine, int violationColumn, string settings)
        {
            this.settings = settings;
            DiagnosticResult expected = this.CSharpDiagnostic().WithArguments(testMethodName, testMethodNameMemberUnderTest).WithLocation(violantioLine, violationColumn);
            await this.VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None).ConfigureAwait(false);
        }

        [Theory]
        [ClassData(typeof(TestNameMustIncludeMemberUnderTestAnalyzerXunitTestInvalidData))]
        public async Task IncludeMemberUnderTestInTestName_XunitTestCodeWithViolation_ExpectsDiagnostic(string testCode, string testMethodName, string testMethodNameMemberUnderTest, int violantioLine, int violationColumn, string settings)
        {
            this.settings = settings;
            DiagnosticResult expected = this.CSharpDiagnostic().WithArguments(testMethodName, testMethodNameMemberUnderTest).WithLocation(violantioLine, violationColumn);
            await this.VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None, UnitTestFramework.Xunit).ConfigureAwait(false);
        }

        [Theory]
        [ClassData(typeof(TestNameMustIncludeMemberUnderTestAnalyzerNUnitTestInvalidData))]
        public async Task IncludeMemberUnderTestInTestName_NUnitTestCodeWithViolation_ExpectsDiagnostic(string testCode, string testMethodName, string testMethodNameMemberUnderTest, int violantioLine, int violationColumn, string settings)
        {
            this.settings = settings;
            DiagnosticResult expected = this.CSharpDiagnostic().WithArguments(testMethodName, testMethodNameMemberUnderTest).WithLocation(violantioLine, violationColumn);
            await this.VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None, UnitTestFramework.NUnit).ConfigureAwait(false);
        }

        [Theory]
        [ClassData(typeof(TestNameMustIncludeMemberUnderTestAnalyzerMSTestValidData))]
        public async Task IncludeMemberUnderTestInTestName_MSTestCodeWithoutViolation_ExpectsNoDiagnostic(string testCode, string settings)
        {
            this.settings = settings;
            await this.VerifyCSharpDiagnosticAsync(testCode, EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(false);
        }

        [Theory]
        [ClassData(typeof(TestNameMustIncludeMemberUnderTestAnalyzerXunitTestValidData))]
        public async Task IncludeMemberUnderTestInTestName_XunitTestCodeWithoutViolation_ExpectsNoDiagnostic(string testCode, string settings)
        {
            this.settings = settings;
            await this.VerifyCSharpDiagnosticAsync(testCode, EmptyDiagnosticResults, CancellationToken.None, UnitTestFramework.Xunit).ConfigureAwait(false);
        }

        [Theory]
        [ClassData(typeof(TestNameMustIncludeMemberUnderTestAnalyzerNUnitTestValidData))]
        public async Task IncludeMemberUnderTestInTestName_NUnitTestCodeWithoutViolation_ExpectsNoDiagnostic(string testCode, string settings)
        {
            this.settings = settings;
            await this.VerifyCSharpDiagnosticAsync(testCode, EmptyDiagnosticResults, CancellationToken.None, UnitTestFramework.NUnit).ConfigureAwait(false);
        }

        /// <summary>
        /// Get the CSharp analyzer being tested.
        /// </summary>
        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new TestNameMustIncludeMemberUnderTestAnalyzer();
        }

        /// <inheritdoc/>
        protected override string GetSettings()
        {
            return this.settings;
        }
    }
}