namespace TestHelper
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Diagnostics;
    using Microsoft.CodeAnalysis.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using UnitTestAnalyzers.Settings.ObjectModel;
    using UnitTestAnalyzers.Test.TestData;
    using Xunit;

    /// <summary>
    /// Class for turning strings into documents and getting the diagnostics on them
    /// All methods are static
    /// </summary>
    public abstract partial class DiagnosticVerifier
    {
        private const string DefaultFilePathPrefix = "Test";
        private const string CSharpDefaultFileExt = "cs";
        private const string VisualBasicDefaultExt = "vb";
        private const string TestProjectName = "TestProject";
        private const string SettingsFileName = "unittestcodeanalysis.json";

        private static readonly MetadataReference CorlibReference = MetadataReference.CreateFromFile(typeof(object).Assembly.Location);
        private static readonly MetadataReference SystemCoreReference = MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location);
        private static readonly MetadataReference CSharpSymbolsReference = MetadataReference.CreateFromFile(typeof(CSharpCompilation).Assembly.Location);
        private static readonly MetadataReference CodeAnalysisReference = MetadataReference.CreateFromFile(typeof(Compilation).Assembly.Location);
        private static readonly MetadataReference ThisProjectReference = MetadataReference.CreateFromFile(typeof(TestSettingsData).Assembly.Location);
        private static readonly MetadataReference MsTestReference = MetadataReference.CreateFromFile(typeof(TestClassAttribute).Assembly.Location);
        private static readonly MetadataReference XunitReference = MetadataReference.CreateFromFile(typeof(TheoryAttribute).Assembly.Location);
        private static readonly string ProgramFilesX86 = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
        private static readonly MetadataReference PortableCorlibReference = MetadataReference.CreateFromFile(Path.Combine(ProgramFilesX86, @"Reference Assemblies\Microsoft\Framework\.NETPortable\v4.5\Profile\Profile7\System.Runtime.dll"));

        /// <summary>
        /// Given classes in the form of strings, their language, and an <see cref="DiagnosticAnalyzer"/> to apply to
        /// it, return the <see cref="Diagnostic"/>s found in the string after converting it to a
        /// <see cref="Document"/>.
        /// </summary>
        /// <param name="sources">Classes in the form of strings.</param>
        /// <param name="language">The language the source classes are in. Values may be taken from the
        /// <see cref="LanguageNames"/> class.</param>
        /// <param name="analyzer">The analyzer to be run on the sources.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that the task will observe.</param>
        /// <param name="framework">The targeted unit test framework.</param>
        /// <returns>A collection of <see cref="Diagnostic"/>s that surfaced in the source code, sorted by
        /// <see cref="Diagnostic.Location"/>.</returns>
        private Task<ImmutableArray<Diagnostic>> GetSortedDiagnosticsAsync(string[] sources, string language, DiagnosticAnalyzer analyzer, CancellationToken cancellationToken, UnitTestFramework framework)
        {
            return GetSortedDiagnosticsFromDocumentsAsync(analyzer, this.GetDocuments(sources, language, framework), cancellationToken);
        }

        /// <summary>
        /// Given an analyzer and a document to apply it to, run the analyzer and gather an array of diagnostics found in it.
        /// The returned diagnostics are then ordered by location in the source document.
        /// </summary>
        /// <param name="analyzer">The analyzer to run on the documents</param>
        /// <param name="documents">The Documents that the analyzer will be run on</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that the task will observe.</param>
        /// <returns>An IEnumerable of Diagnostics that surfaced in the source code, sorted by Location</returns>
        protected static async Task<ImmutableArray<Diagnostic>> GetSortedDiagnosticsFromDocumentsAsync(DiagnosticAnalyzer analyzer, Document[] documents, CancellationToken cancellationToken)
        {
            var projects = new HashSet<Project>();
            foreach (var document in documents)
            {
                projects.Add(document.Project);
            }

            var diagnostics = new List<Diagnostic>();
            foreach (var project in projects)
            {
                var compilation = await project.GetCompilationAsync(cancellationToken).ConfigureAwait(false);
                var compilationWithAnalyzers = compilation.WithAnalyzers(ImmutableArray.Create(analyzer), project.AnalyzerOptions, cancellationToken);
                var diags = compilationWithAnalyzers.GetAnalyzerDiagnosticsAsync().Result;
                foreach (var diag in diags)
                {
                    if (diag.Location == Location.None || diag.Location.IsInMetadata)
                    {
                        diagnostics.Add(diag);
                    }
                    else
                    {
                        for (int i = 0; i < documents.Length; i++)
                        {
                            var document = documents[i];
                            var tree = document.GetSyntaxTreeAsync().Result;
                            if (tree == diag.Location.SourceTree)
                            {
                                diagnostics.Add(diag);
                            }
                        }
                    }
                }
            }

            var results = SortDiagnostics(diagnostics);
            diagnostics.Clear();
            return results.ToImmutableArray();
        }

        /// <summary>
        /// Sort diagnostics by location in source document
        /// </summary>
        /// <param name="diagnostics">The list of Diagnostics to be sorted</param>
        /// <returns>An IEnumerable containing the Diagnostics in order of Location</returns>
        private static Diagnostic[] SortDiagnostics(IEnumerable<Diagnostic> diagnostics)
        {
            return diagnostics.OrderBy(d => d.Location.SourceSpan.Start).ToArray();
        }

        /// <summary>
        /// Given an array of strings as sources and a language, turn them into a project and return the documents and spans of it.
        /// </summary>
        /// <param name="sources">Classes in the form of strings</param>
        /// <param name="language">The language the source code is in</param>
        /// <param name="framework">The targeted unit test framework.</param>
        /// <returns>A Tuple containing the Documents produced from the sources and their TextSpans if relevant</returns>
        private Document[] GetDocuments(string[] sources, string language, UnitTestFramework framework)
        {
            if (language != LanguageNames.CSharp && language != LanguageNames.VisualBasic)
            {
                throw new ArgumentException("Unsupported Language");
            }

            var project = this.CreateProject(sources, language, framework);
            var documents = project.Documents.ToArray();

            if (sources.Length != documents.Length)
            {
                throw new SystemException("Amount of sources did not match amount of Documents created");
            }

            return documents;
        }

        /// <summary>
        /// Create a project using the inputted strings as sources.
        /// </summary>
        /// <param name="sources">Classes in the form of strings</param>
        /// <param name="language">The language the source code is in</param>
        /// <param name="framework">The targed unit test framework.</param>
        /// <returns>A Project created out of the Documents created from the source strings</returns>
        private Project CreateProject(string[] sources, string language, UnitTestFramework framework)
        {
            string fileNamePrefix = DefaultFilePathPrefix;
            string fileExt = language == LanguageNames.CSharp ? CSharpDefaultFileExt : VisualBasicDefaultExt;
            var compilationOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, allowUnsafe: true);

            var projectId = ProjectId.CreateNewId(debugName: TestProjectName);

            MetadataReference corlibReference;
            MetadataReference unitTestFrameworkReference;

            switch (framework)
            {
               case UnitTestFramework.MSTest:
                    unitTestFrameworkReference = MsTestReference;
                    corlibReference = CorlibReference;
                    break;

                case UnitTestFramework.Xunit:
                    unitTestFrameworkReference = XunitReference;
                    corlibReference = PortableCorlibReference;
                    break;

                default:
                    throw new ArgumentException("The provided value is not supported", nameof(framework));
            }

            var solution = new AdhocWorkspace()
                .CurrentSolution
                .AddProject(projectId, TestProjectName, TestProjectName, language)
                .WithProjectCompilationOptions(projectId, compilationOptions)
                .AddMetadataReference(projectId, corlibReference)
                .AddMetadataReference(projectId, SystemCoreReference)
                .AddMetadataReference(projectId, CSharpSymbolsReference)
                .AddMetadataReference(projectId, CodeAnalysisReference)
                .AddMetadataReference(projectId, ThisProjectReference)
                .AddMetadataReference(projectId, unitTestFrameworkReference);

            var settings = this.GetSettings();
            if (!string.IsNullOrEmpty(settings))
            {
                var documentId = DocumentId.CreateNewId(projectId);
                solution = solution.AddAdditionalDocument(documentId, SettingsFileName, settings);
            }

            ParseOptions parseOptions = solution.GetProject(projectId).ParseOptions;
#pragma warning disable RS1014 // Do not ignore values returned by methods on immutable objects.
            solution.WithProjectParseOptions(projectId, parseOptions.WithDocumentationMode(DocumentationMode.Diagnose));
#pragma warning restore RS1014 // Do not ignore values returned by methods on immutable objects.

            int count = 0;
            foreach (var source in sources)
            {
                var newFileName = fileNamePrefix + count + "." + fileExt;
                var documentId = DocumentId.CreateNewId(projectId, debugName: newFileName);
                solution = solution.AddDocument(documentId, newFileName, SourceText.From(source));
                count++;
            }

            return solution.GetProject(projectId);
        }

        /// <summary>
        /// Gets the content of the settings file to use.
        /// </summary>
        /// <returns>The contents of the settings file to use.</returns>
        protected virtual string GetSettings()
        {
            return null;
        }

        protected DiagnosticResult CSharpDiagnostic(string diagnosticId = null)
        {
            DiagnosticAnalyzer analyzer = this.GetCSharpDiagnosticAnalyzer();
            var supportedDiagnostics = analyzer.SupportedDiagnostics;
            if (diagnosticId == null)
            {
                return new DiagnosticResult(supportedDiagnostics.Single());
            }
            else
            {
                return new DiagnosticResult(supportedDiagnostics.Single(i => i.Id == diagnosticId));
            }
        }
    }
}
