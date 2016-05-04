namespace UnitTestAnalyzers
{
    using System;
    using System.Collections.Immutable;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Diagnostics;
    using Parsers;
    using Settings;
    using Settings.ObjectModel;

    /// <summary>
    /// This analyzer identifies a test class and reports a warning if the test class name
    /// does not have the suffic 'UnitTests'.
    /// </summary>
    /// <seealso cref="Microsoft.CodeAnalysis.Diagnostics.DiagnosticAnalyzer" />
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class UseUnitTestsSuffixAnalyzer : DiagnosticAnalyzer
    {
        /// <summary>
        /// The ID for diagnostics produced by the <see cref="UseUnitTestsSuffixAnalyzer"/> analyzer.
        /// </summary>
        public const string DiagnosticId = "UseUnitTestsSuffix";
        private const string Category = "Naming";

        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.UseUnitTestsSuffixTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.UseUnitTestsSuffixMessage), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.UseUnitTestsSuffixDescription), Resources.ResourceManager, typeof(Resources));
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

            context.RegisterSyntaxNodeAction(c => AnalyzeNode(c, unitTestParser), SyntaxKind.ClassDeclaration);
        }

        private static void AnalyzeNode(SyntaxNodeAnalysisContext context, IUnitTestParser parser)
        {
            if (!parser.IsUnitTestClass(context))
            {
                return;
            }

            ClassDeclarationSyntax classDeclaration = (ClassDeclarationSyntax)context.Node;

            if (classDeclaration.Identifier.ToString().EndsWith("UnitTests"))
            {
                return;
            }

            var diagnostic = Diagnostic.Create(Rule, classDeclaration.Identifier.GetLocation(), classDeclaration.Identifier.ToString());

            context.ReportDiagnostic(diagnostic);
        }
    }
}
