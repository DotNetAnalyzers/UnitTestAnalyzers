 namespace UnitTestAnalyzers
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Diagnostics;
    using Parsers;
    using Settings;
    using Settings.ObjectModel;

    /// <summary>
    /// This analyzer verifies whether a test method name contains the name of the member under test.
    /// If the regex provided by the analyzer settings <see cref="AnalyzersSettings"/> contains a group named with <see cref="AnalyzersSettings.MemberUnderTestRegexGroupName"/>
    /// this analyzer will validate that the test is invoking a member that matches the value represented by this group generating a warning if it does not.
    /// </summary>
    /// <seealso cref="Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzer" />
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class TestNameMustIncludeMemberUnderTestAnalyzer : DiagnosticAnalyzer
    {
        /// <summary>
        /// The ID for diagnostics produced by the <see cref="TestNameMustIncludeMemberUnderTestAnalyzer"/> analyzer when the comments are missing, too many or out of order.
        /// </summary>
        public const string DiagnosticId = "TestNameMustIncludeMemberUnderTest";
        private const string Category = "Design";

        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.TestMethodNameMustIncludeMemberUnderTestTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.TestMethodNameMustIncludeMemberUnderTestMessage), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.TestMethodNameMustIncludeMemberUnderTestDescription), Resources.ResourceManager, typeof(Resources));
        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

        /// <inheritdoc/>
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        /// <inheritdoc/>
        public override void Initialize(AnalysisContext context)
        {
            context.RegisterCompilationStartAction(HandleCompilationStart);
        }

        private static void HandleCompilationStart(CompilationStartAnalysisContext context)
        {
            AnalyzersSettings settings = SettingsProvider.GetSettings(context);

            IUnitTestParser unitTestParser;

            switch (settings.UnitTestFramework)
            {
                    case UnitTestFramework.MSTest:
                    unitTestParser = new MSTestParser();
                    break;

                    case UnitTestFramework.Xunit:
                    unitTestParser = new XunitTestParser();
                    break;

                default:
                    throw new NotSupportedException();
            }

            context.RegisterSyntaxNodeAction(c => AnalyzeNode(c, unitTestParser, settings), SyntaxKind.MethodDeclaration);
        }

        private static void AnalyzeNode(SyntaxNodeAnalysisContext context, IUnitTestParser parser, AnalyzersSettings settings)
        {
            if (!parser.IsUnitTestMethod(context))
            {
                return;
            }

            MethodDeclarationSyntax methodDeclaration = (MethodDeclarationSyntax)context.Node;

            Match match = settings.TestMethodNameFormatRegex.Match(methodDeclaration.Identifier.ToString());

            string memberUnderTest = match?.Groups[AnalyzersSettings.MemberUnderTestRegexGroupName].Value;

            if (string.IsNullOrWhiteSpace(memberUnderTest))
            {
                return;
            }

            IEnumerable<SyntaxNode> descendantNodes = methodDeclaration.Body.DescendantNodes();
            if (!descendantNodes?.Any() ?? true)
            {
                return;
            }

            IEnumerable<MemberAccessExpressionSyntax> memberAccessExpressions = descendantNodes.OfType<MemberAccessExpressionSyntax>();
            IEnumerable<ObjectCreationExpressionSyntax> ctorExpressions = descendantNodes.OfType<ObjectCreationExpressionSyntax>();
            if (!memberAccessExpressions.Any() && !ctorExpressions.Any())
            {
                return;
            }

            if (memberAccessExpressions.Any(memberAccess => string.Equals(memberUnderTest, memberAccess.Name.Identifier.ToString(), StringComparison.Ordinal)))
            {
                return;
            }

            if (ctorExpressions.Any(ctorExpression => string.Equals(memberUnderTest, ctorExpression.Type.ToString(), StringComparison.Ordinal)))
            {
                return;
            }

            var diagnostic = Diagnostic.Create(Rule, methodDeclaration.Identifier.GetLocation(), methodDeclaration.Identifier.ToString(), memberUnderTest);

            context.ReportDiagnostic(diagnostic);
        }
    }
}