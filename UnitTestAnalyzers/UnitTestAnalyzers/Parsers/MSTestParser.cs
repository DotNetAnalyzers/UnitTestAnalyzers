namespace UnitTestAnalyzers.Parsers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Diagnostics;

    internal class MSTestParser : IUnitTestParser
    {
        /// <summary>
        /// Determines whether the provided node analysis context refers to a MSTest class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>Whether the provided node analysis context refers to a MsTest class.</returns>
        public bool IsUnitTestClass(SyntaxNodeAnalysisContext context)
        {
            ClassDeclarationSyntax classDeclaration = context.Node as ClassDeclarationSyntax;

            IEnumerable<AttributeSyntax> attributes = classDeclaration?.AttributeLists.SelectMany(x => x.Attributes).ToList();

            if (!attributes?.Any() ?? true) return false;

            AttributeSyntax testClassAttribute = attributes.FirstOrDefault(x => (x.Name.ToString().EndsWith("TestClass", StringComparison.Ordinal)));

            if (testClassAttribute == null) return false;

            var attributeSymbol = context.SemanticModel.GetSymbolInfo(testClassAttribute).Symbol as IMethodSymbol;

            return attributeSymbol?.ToString().StartsWith("Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute", StringComparison.Ordinal) ?? false;
        }

        /// <summary>
        /// Determines whether the provided node analysis context refers to a MSTest class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>Whether the provided node analysis context refers to a MsTest class.</returns>
        public bool IsUnitTestMethod(SyntaxNodeAnalysisContext context)
        {
            MethodDeclarationSyntax methodDeclaration = context.Node as MethodDeclarationSyntax;

            IEnumerable<AttributeSyntax> attributes = methodDeclaration?.AttributeLists.SelectMany(x => x.Attributes).ToList();

            if (!attributes?.Any() ?? true) return false;

            AttributeSyntax testMethodAttribute = attributes.FirstOrDefault(x => (x.Name.ToString().EndsWith("TestMethod", StringComparison.Ordinal)));

            if (testMethodAttribute == null) return false;

            var attributeSymbol = context.SemanticModel.GetSymbolInfo(testMethodAttribute).Symbol as IMethodSymbol;

            return attributeSymbol?.ToString().StartsWith("Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute", StringComparison.Ordinal) ?? false;
        }
    }
}
