namespace UnitTestAnalyzers.Test.Naming
{
    using Xunit;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Diagnostics;
    using Settings.ObjectModel;
    using TestData;
    using TestHelper;

    public class UseCorrectTestNameFormatFunctionalTests : DiagnosticVerifier
    {
        private string settings;

        [Theory]
        [ClassData(typeof(UseCorrectTestNameFormatMSTestInvalidData))]
        public async Task UseCorrectTestNameFormat_MSTestCodeWithViolation_ExpectsDiagnostic(string testCode, string testMethodName, int violantioLine, int violationColumn, string settings, string testMethodFormat, string[] testMethodNameExamples)
        {
            this.settings = settings;
            string testMethodNameExamplesArgument = string.Join(", ", testMethodNameExamples);
            DiagnosticResult expected = this.CSharpDiagnostic().WithArguments(testMethodName, testMethodFormat, testMethodNameExamplesArgument).WithLocation(violantioLine, violationColumn);
            await this.VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None).ConfigureAwait(false);
        }

        [Theory]
        [ClassData(typeof(UseCorrectTestNameFormatMSTestValidData))]
        public async Task UseCorrectTestNameFormat_MSTestCodeWithoutViolation_ExpectsNoDiagnostic(string testCode, string settings)
        {
            this.settings = settings;
            await this.VerifyCSharpDiagnosticAsync(testCode, EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(false);
        }

        [Theory]
        [ClassData(typeof(UseCorrectTestNameFormatXunitTestInvalidData))]
        public async Task UseCorrectTestNameFormat_XunitTestCodeWithViolation_ExpectsDiagnostic(string testCode, string testMethodName, int violantioLine, int violationColumn, string settings, string testMethodFormat, string[] testMethodNameExamples)
        {
            this.settings = settings;
            string testMethodNameExamplesArgument = string.Join(", ", testMethodNameExamples);
            DiagnosticResult expected = this.CSharpDiagnostic().WithArguments(testMethodName, testMethodFormat, testMethodNameExamplesArgument).WithLocation(violantioLine, violationColumn);
            await this.VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None, UnitTestFramework.Xunit).ConfigureAwait(false);
        }

        [Theory]
        [ClassData(typeof(UseCorrectTestNameFormatXunitTestValidData))]
        public async Task UseCorrectTestNameFormat_UsingxUnitTestCodeWithoutViolation_ExpectsNoDiagnostic(string testCode, string settings)
        {
            this.settings = settings;
            await this.VerifyCSharpDiagnosticAsync(testCode, EmptyDiagnosticResults, CancellationToken.None, UnitTestFramework.Xunit).ConfigureAwait(false);
        }

        /// <summary>
        /// Get the CSharp analyzer being tested.
        /// </summary>
        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new UseCorrectTestNameFormatAnalyzer();
        }

        /// <inheritdoc/>
        protected override string GetSettings()
        {
            return this.settings;
        }
    }
}