namespace UnitTestAnalyzers.Parsers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Diagnostics;

    /// <summary>
    /// Evaluates analysis contexts determining if it represents MSTest elements according to the
    /// presence of MSTest attributes.
    /// </summary>
    /// <seealso cref="UnitTestAnalyzers.Parsers.IUnitTestParser" />
    internal class MSTestParser : AttributeBasedTestParser
    {
        /// <inheritdoc/>
        protected override string[] TestMethodAttributes { get; } = {
            "Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute"
        };

        /// <inheritdoc />
        public override bool IsUnitTestClass(SyntaxNodeAnalysisContext context)
        {
            ClassDeclarationSyntax classDeclaration = context.Node as ClassDeclarationSyntax;

            IEnumerable<AttributeSyntax> attributes = classDeclaration?.AttributeLists.SelectMany(x => x.Attributes).ToList();

            if (!attributes?.Any() ?? true)
            {
                return false;
            }

            AttributeSyntax testClassAttribute = attributes.FirstOrDefault(x => x.Name.ToString().EndsWith("TestClass", StringComparison.Ordinal));

            if (testClassAttribute == null)
            {
                return false;
            }

            var attributeSymbol = context.SemanticModel.GetSymbolInfo(testClassAttribute).Symbol as IMethodSymbol;

            return attributeSymbol?.ToString().StartsWith("Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute", StringComparison.Ordinal) ?? false;
        }
    }
}
