namespace UnitTestAnalyzers.Test.Naming
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Diagnostics;
    using TestData;
    using TestHelper;
    using Xunit;

    public class UseUnitTestsSuffixFunctionalTests : DiagnosticVerifier
    {
        private string settings;

        [Theory]
        [ClassData(typeof(UseUnitTestsSuffixMSTestValidData))]
        public async Task UseUnitTestsSuffix_MSTestCodeWithoutViolation_ExpectsNoDiagnostic(string testCode, string settings)
        {
            this.settings = settings;
            await this.VerifyCSharpDiagnosticAsync(testCode, EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(false);
        }

        [Theory]
        [ClassData(typeof(UseUnitTestsSuffixMSTestInvalidData))]
        public async Task UseUnitTestsSuffix_MSTestCodeWithViolation_ExpectsDiagnostic(string testCode, string testClassName, int violantioLine, int violationColumn, string settings)
        {
            this.settings = settings;
            DiagnosticResult expected = this.CSharpDiagnostic().WithArguments(testClassName).WithLocation(violantioLine, violationColumn);
            await this.VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None).ConfigureAwait(false);
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new UseUnitTestsSuffixAnalyzer();
        }

        protected override string GetSettings()
        {
            return this.settings;
        }
    }
}
