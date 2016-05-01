using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace UnitTestAnalyzers
{
    using Parsers;
    using Settings;
    using Settings.ObjectModel;

    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class UseUnitTestsSuffixAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "UnitTestAnalyzers";

        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.UseUnitTestsSuffixTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.UseUnitTestsSuffixMessage), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.UseUnitTestsSuffixDescription), Resources.ResourceManager, typeof(Resources));
        private const string Category = "Naming";

        private static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

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
