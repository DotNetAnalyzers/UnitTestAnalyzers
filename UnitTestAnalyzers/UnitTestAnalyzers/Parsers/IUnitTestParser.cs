namespace UnitTestAnalyzers.Parsers
{
    using Microsoft.CodeAnalysis.Diagnostics;

    /// <summary>
    /// The parser interface for various unit test frameworks.
    /// Different unit test frameworks use different attributes and templates to represent test classes and methods
    /// </summary>
    internal interface IUnitTestParser
    {
        /// <summary>
        /// Determines whether the specified context refers to a unit test class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>
        /// <see langword="true"/> if the <see cref="SyntaxNodeAnalysisContext"/> is a unit test class otherwise, <see langword="false"/>.
        /// </returns>
        bool IsUnitTestClass(SyntaxNodeAnalysisContext context);

        /// <summary>
        /// Determines whether the specified context refers to a unit test class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>
        /// <see langword="true"/> if the <see cref="SyntaxNodeAnalysisContext"/> is a unit test class otherwise, <see langword="false"/>.
        /// </returns>
        bool IsUnitTestMethod(SyntaxNodeAnalysisContext context);
    }
}
