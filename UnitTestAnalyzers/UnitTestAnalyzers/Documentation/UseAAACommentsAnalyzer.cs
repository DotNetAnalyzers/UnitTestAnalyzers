namespace UnitTestAnalyzers
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Diagnostics;
    using Microsoft.CodeAnalysis.Text;
    using Parsers;
    using Settings;
    using Settings.ObjectModel;

    /// <summary>
    /// This analyzer verifies whether a test method contains the comments // Arrange. // Act. // Assert.
    /// and it reports a warning if the comments are missing, too many or out of order.
    /// </summary>
    /// <seealso cref="Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzer" />
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class UseAAACommentsAnalyzer : DiagnosticAnalyzer
    {
        /// <summary>
        /// The ID for diagnostics produced by the <see cref="UseAAACommentsAnalyzer"/> analyzer when the comments are missing, too many or out of order.
        /// </summary>
        public const string AAACommentsDiagnosticId = "UseAAAComments";

        /// <summary>
        /// The ID for diagnostics produced by the <see cref="UseAAACommentsAnalyzer"/> analyzer when the comment // Arrange. is not in the
        /// first line of the body of the test method.
        /// </summary>
        public const string ArrangeCommentFirstLineDiagnosticId = "UseArrangeCommentAsFirstLine";

        private const string Category = "Documentation";

        private static readonly LocalizableString AAACommentsTitle = new LocalizableResourceString(nameof(Resources.UseAAACommentsAnalyzerTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString AAACommentsMessageFormat = new LocalizableResourceString(nameof(Resources.UseAAACommentsAnalyzerMessage), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString AAACommentsDescription = new LocalizableResourceString(nameof(Resources.UseAAACommentsAnalyzerDescription), Resources.ResourceManager, typeof(Resources));

        private static readonly LocalizableString FirstLineArrangeTitle = new LocalizableResourceString(nameof(Resources.ArrangeCommentMustBeFirstLineTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString FirstLineArrangeMessageFormat = new LocalizableResourceString(nameof(Resources.ArrangeCommentMustBeFirstLineMessage), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString FirstLineArrangeDescription = new LocalizableResourceString(nameof(Resources.ArrangeCommentMustBeFirstLineDescription), Resources.ResourceManager, typeof(Resources));

        private static readonly DiagnosticDescriptor AAACommentsRule = new DiagnosticDescriptor(AAACommentsDiagnosticId, AAACommentsTitle, AAACommentsMessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: AAACommentsDescription);
        private static readonly DiagnosticDescriptor FirstLineArrangeRule = new DiagnosticDescriptor(ArrangeCommentFirstLineDiagnosticId, FirstLineArrangeTitle, FirstLineArrangeMessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: FirstLineArrangeDescription);

        /// <inheritdoc/>
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(AAACommentsRule, FirstLineArrangeRule);

        /// <inheritdoc/>
        public override void Initialize(AnalysisContext context)
        {
            context.RegisterCompilationStartAction(HandleCompilationStart);
        }

        private static void HandleCompilationStart(CompilationStartAnalysisContext context)
        {
            AnalyzersSettings settings = SettingsProvider.GetSettings(context);
            var unitTestParser = UnitTestParserFactory.Create(settings);
            context.RegisterSyntaxNodeAction(c => AnalyzeNode(c, unitTestParser), SyntaxKind.MethodDeclaration);
        }

        private static void AnalyzeNode(SyntaxNodeAnalysisContext context, IUnitTestParser parser)
        {
            if (!parser.IsUnitTestMethod(context))
            {
                return;
            }

            MethodDeclarationSyntax methodDeclaration = (MethodDeclarationSyntax)context.Node;

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

            IEnumerable<SyntaxTrivia> singleLineComments = methodDeclaration.Body.DescendantTrivia().Where(trivia => trivia.IsKind(SyntaxKind.SingleLineCommentTrivia));

            IEnumerable<SyntaxTrivia> arrangeComments = singleLineComments.Where(comment => string.Equals("// Arrange.", comment.ToFullString()));
            IEnumerable<SyntaxTrivia> actComments = singleLineComments.Where(comment => string.Equals("// Act.", comment.ToFullString()));
            IEnumerable<SyntaxTrivia> assertComments = singleLineComments.Where(comment => string.Equals("// Assert.", comment.ToFullString()));

            Diagnostic diagnostic = null;
            LinePosition arrangeCommentLine = default(LinePosition);

            if (arrangeComments.Any())
            {
                arrangeCommentLine = arrangeComments.First().GetLocation().GetLineSpan().StartLinePosition;
            }

            // Check to see whether each comment exists. Only a single instance of each comment should exist.
            if (arrangeComments.Count() != 1 || actComments.Count() != 1 || assertComments.Count() != 1)
            {
                diagnostic = Diagnostic.Create(AAACommentsRule, methodDeclaration.Identifier.GetLocation(), methodDeclaration.Identifier.ToString());
            }
            else
            {
                // Check whether the comments appear with the expected order.
                LinePosition actCommentLine = actComments.First().GetLocation().GetLineSpan().StartLinePosition;
                LinePosition assertCommentLine = assertComments.First().GetLocation().GetLineSpan().StartLinePosition;

                if (arrangeCommentLine > actCommentLine || actCommentLine > assertCommentLine)
                {
                    diagnostic = Diagnostic.Create(AAACommentsRule, methodDeclaration.Identifier.GetLocation(), methodDeclaration.Identifier.ToString());
                }
            }

            if (diagnostic != null)
            {
                context.ReportDiagnostic(diagnostic);
            }

            var methodOpenBrace = methodDeclaration.Body.ChildTokens().FirstOrDefault(token => token.IsKind(SyntaxKind.OpenBraceToken));

            LinePosition openBraceLinePosition = methodOpenBrace.GetLocation().GetLineSpan().EndLinePosition;

            if (arrangeCommentLine.Line != openBraceLinePosition.Line + 1)
            {
                diagnostic = Diagnostic.Create(FirstLineArrangeRule, methodDeclaration.Identifier.GetLocation(), methodDeclaration.Identifier.ToString());
                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}