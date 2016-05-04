namespace UnitTestAnalyzers.Test.Naming
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Diagnostics;
    using Settings.ObjectModel;
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

        [Theory]
        [ClassData(typeof(UseUnitTestsSuffixXunitTestValidData))]
        public async Task UseUnitTestsSuffix_xUnitTestCodeWithoutViolation_ExpectsNoDiagnostic(string testCode, string settings)
        {
            this.settings = settings;
            await this.VerifyCSharpDiagnosticAsync(testCode, EmptyDiagnosticResults, CancellationToken.None, UnitTestFramework.Xunit).ConfigureAwait(false);
        }

        [Theory]
        [ClassData(typeof(UseUnitTestsSuffixXunitTestInvalidData))]
        public async Task UseUnitTestsSuffix_xUnitTestCodeWithViolation_ExpectsDiagnostic(string testCode, string testClassName, int violantionLine, int violationColumn, string settings)
        {
            this.settings = settings;
            DiagnosticResult expected = this.CSharpDiagnostic().WithArguments(testClassName).WithLocation(violantionLine, violationColumn);
            await this.VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None, UnitTestFramework.Xunit).ConfigureAwait(false);
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
