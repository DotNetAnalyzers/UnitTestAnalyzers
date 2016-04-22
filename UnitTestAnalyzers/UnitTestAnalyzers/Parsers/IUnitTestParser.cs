namespace UnitTestAnalyzers.Parsers
{
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Diagnostics;

    internal interface IUnitTestParser
    {
        bool IsUnitTestClass(SyntaxNodeAnalysisContext context);

        bool IsUnitTestMethod(SyntaxNodeAnalysisContext context);
    }
}
