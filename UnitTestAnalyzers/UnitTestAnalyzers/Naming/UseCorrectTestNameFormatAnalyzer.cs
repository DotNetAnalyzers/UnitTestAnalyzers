namespace UnitTestAnalyzers
{
    using System;
    using System.Collections.Immutable;
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Diagnostics;
    using Parsers;
    using Settings;
    using Settings.ObjectModel;

    /// <summary>
    /// This analyzer identifies a test method and reports a warning if the test method name
    /// does match the expected format.
    /// </summary>
    /// <seealso cref="Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzer" />
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class UseCorrectTestNameFormatAnalyzer : DiagnosticAnalyzer
    {
        /// <summary>
        /// The ID for diagnostics produced by the <see cref="UseCorrectTestNameFormatAnalyzer"/> analyzer.
        /// </summary>
        public const string DiagnosticId = "UseCorrectTestNameFormat";
        private const string Category = "Naming";

        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.UseCorrectTestNameFormatTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.UseCorrectTestNameFormatMessage), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.UseCorrectTestNameFormatDescription), Resources.ResourceManager, typeof(Resources));
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

            var identifier = methodDeclaration.Identifier;
            var name = identifier.ToString();

            if (settings.TestMethodNameFormatRegex.IsMatch(name))
            {
                return;
            }

            string testNameFormatExamples = settings.TestMethodNameFormatExamples.Any() ? string.Join(", ", settings.TestMethodNameFormatExamples) : AnalyzersSettings.NoTestMethodNameExamplesProvided;
            var diagnostic = Diagnostic.Create(Rule, methodDeclaration.Identifier.GetLocation(), methodDeclaration.Identifier.ToString(), settings.TestMethodNameFormat, testNameFormatExamples);

            context.ReportDiagnostic(diagnostic);
        }
    }
}
