namespace UnitTestAnalyzers.Test.Documentation
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Diagnostics;
    using Settings.ObjectModel;
    using TestData;
    using TestHelper;
    using Xunit;

    public class UseAAACommentsFunctionalTests : DiagnosticVerifier
    {
        private string settings;

        [Theory]
        [ClassData(typeof(UseAAACommentsAnalyzerMSTestInvalidData))]
        public async Task UseAAAComments_MSTestCodeWithViolation_ExpectsDiagnostic(string testCode, string testMethodName, int violantioLine, int violationColumn, string settings, bool hasValidAAAComments, bool firstLineIsArrangeComment)
        {
            this.settings = settings;
            var expectedDiagnostics = new List<DiagnosticResult>();
            if (!hasValidAAAComments)
            {
                expectedDiagnostics.Add(this.CSharpDiagnostic(UseAAACommentsAnalyzer.AAACommentsDiagnosticId).WithArguments(testMethodName).WithLocation(violantioLine, violationColumn));
            }

            if (!firstLineIsArrangeComment)
            {
                expectedDiagnostics.Add(this.CSharpDiagnostic(UseAAACommentsAnalyzer.ArrangeCommentFirstLineDiagnosticId).WithArguments(testMethodName).WithLocation(violantioLine, violationColumn));
            }

            await this.VerifyCSharpDiagnosticAsync(testCode, expectedDiagnostics.ToArray(), CancellationToken.None).ConfigureAwait(false);
        }

        [Theory]
        [ClassData(typeof(UseAAACommentsAnalyzerXunitTestInvalidData))]
        public async Task UseAAAComments_XunitTestCodeWithViolation_ExpectsDiagnostic(string testCode, string testMethodName, int violantioLine, int violationColumn, string settings, bool hasValidAAAComments, bool firstLineIsArrangeComment)
        {
            this.settings = settings;
            var expectedDiagnostics = new List<DiagnosticResult>();
            if (!hasValidAAAComments)
            {
                expectedDiagnostics.Add(this.CSharpDiagnostic(UseAAACommentsAnalyzer.AAACommentsDiagnosticId).WithArguments(testMethodName).WithLocation(violantioLine, violationColumn));
            }

            if (!firstLineIsArrangeComment)
            {
                expectedDiagnostics.Add(this.CSharpDiagnostic(UseAAACommentsAnalyzer.ArrangeCommentFirstLineDiagnosticId).WithArguments(testMethodName).WithLocation(violantioLine, violationColumn));
            }

            await this.VerifyCSharpDiagnosticAsync(testCode, expectedDiagnostics.ToArray(), CancellationToken.None, UnitTestFramework.Xunit).ConfigureAwait(false);
        }

        [Theory]
        [ClassData(typeof(UseAAACommentsAnalyzerMSTestValidData))]
        public async Task UseAAAComments_MSTestCodeWithoutViolation_ExpectsNoDiagnostic(string testCode, string settings)
        {
            this.settings = settings;
            await this.VerifyCSharpDiagnosticAsync(testCode, EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(false);
        }

        [Theory]
        [ClassData(typeof(UseAAACommentsAnalyzerXunitTestValidData))]
        public async Task UseAAAComments_XunitTestCodeWithoutViolation_ExpectsNoDiagnostic(string testCode, string settings)
        {
            this.settings = settings;
            await this.VerifyCSharpDiagnosticAsync(testCode, EmptyDiagnosticResults, CancellationToken.None, UnitTestFramework.Xunit).ConfigureAwait(false);
        }

        /// <summary>
        /// Get the CSharp analyzer being tested.
        /// </summary>
        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new UseAAACommentsAnalyzer();
        }

        /// <inheritdoc/>
        protected override string GetSettings()
        {
            return this.settings;
        }
    }
}