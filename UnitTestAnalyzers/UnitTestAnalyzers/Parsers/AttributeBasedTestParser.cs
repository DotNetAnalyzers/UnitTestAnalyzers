namespace UnitTestAnalyzers.Parsers
{
    using System;
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Diagnostics;

    /// <summary>
    /// A base class for test parser based on method attributes
    /// </summary>
    internal abstract class AttributeBasedTestParser : IUnitTestParser
    {
        /// <summary>
        ///  Return the list of attributes full name used to mark test methods
        /// </summary>
        protected abstract string[] TestMethodAttributes { get; }

        /// <inheritdoc />
        public virtual bool IsUnitTestClass(SyntaxNodeAnalysisContext context)
        {
            if (context.Node is ClassDeclarationSyntax classDeclaration)
            {
                var methods = classDeclaration.Members.OfType<MethodDeclarationSyntax>();
                return methods.Any(x => this.IsUnitTestMethod(context, x));
            }
            return false;
        }

        /// <inheritdoc />
        public bool IsUnitTestMethod(SyntaxNodeAnalysisContext context)
        {
            return this.IsUnitTestMethod(context, context.Node as MethodDeclarationSyntax);
        }

        private bool IsUnitTestMethod(SyntaxNodeAnalysisContext context, MethodDeclarationSyntax methodDeclaration)
        {
            var methodAttributes = methodDeclaration?.AttributeLists.SelectMany(a => a.Attributes).ToList();

            if (!methodAttributes?.Any() ?? true)
            {
                return false;
            }

            if (methodAttributes.Any(methodAttribute => this.IsTestMethodAttribute(context, methodAttribute)))
            {
                return true;
            }

            return false;
        }

        private bool IsTestMethodAttribute(SyntaxNodeAnalysisContext context, AttributeSyntax attribute)
        {
            var attributeName = attribute.Name.ToString();

            if (this.TestMethodAttributes.Any(x => x.EndsWith(attributeName, StringComparison.Ordinal) || x.EndsWith(attributeName + "Attribute", StringComparison.Ordinal)) == false)
            {
                return false;
            }

            var attributeSymbol = context.SemanticModel.GetSymbolInfo(attribute).Symbol as IMethodSymbol;

            var attributeSymbolName = attributeSymbol?.ToString();

            if (string.IsNullOrWhiteSpace(attributeSymbolName))
            {
                return false;
            }


            return this.TestMethodAttributes.Any(x => attributeSymbolName.StartsWith(x, StringComparison.Ordinal));
        }
    }
}