﻿namespace UnitTestAnalyzers.Parsers
{
    using System;
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Diagnostics;

    /// <summary>
    /// Evaluates analysis contexts determining if it represents Xunit elements according to the
    /// presence of Xunit attributes.
    /// </summary>
    /// <seealso cref="UnitTestAnalyzers.Parsers.IUnitTestParser" />
    internal class XunitTestParser : IUnitTestParser
    {
        /// <inheritdoc />
        public bool IsUnitTestClass(SyntaxNodeAnalysisContext context)
        {
            if (context.Node is ClassDeclarationSyntax classDeclaration)
            {
                var methods = classDeclaration.Members.OfType<MethodDeclarationSyntax>();
                return methods.Any(x => IsUnitTestMethod(context, x));
            }
            return false;
        }

        /// <inheritdoc />
        public bool IsUnitTestMethod(SyntaxNodeAnalysisContext context)
        {
            return IsUnitTestMethod(context, context.Node as MethodDeclarationSyntax);
        }

        private static bool IsUnitTestMethod(SyntaxNodeAnalysisContext context, MethodDeclarationSyntax methodDeclaration)
        {
            var methodAttributes = methodDeclaration?.AttributeLists.SelectMany(a => a.Attributes).ToList();

            if (!methodAttributes?.Any() ?? true)
            {
                return false;
            }

            if (methodAttributes.Any(methodAttribute => IsTestMethodAttribute(context, methodAttribute)))
            {
                return true;
            }

            return false;
        }

        private static bool IsTestMethodAttribute(SyntaxNodeAnalysisContext context, AttributeSyntax attribute)
        {
            string attributeName = attribute.Name.ToString();
            if (!attributeName.EndsWith("Fact", StringComparison.Ordinal) &&
                !attributeName.EndsWith("Theory", StringComparison.Ordinal))
            {
                return false;
            }

            var attributeSymbol = context.SemanticModel.GetSymbolInfo(attribute).Symbol as IMethodSymbol;

            var attributeSymbolName = attributeSymbol?.ToString();

            if (string.IsNullOrWhiteSpace(attributeSymbolName))
            {
                return false;
            }

            return attributeSymbolName.StartsWith("Xunit.FactAttribute", StringComparison.Ordinal) ||
                   attributeSymbolName.StartsWith("Xunit.TheoryAttribute", StringComparison.Ordinal);
        }
    }
}
