namespace UnitTestAnalyzers.Parsers
{
    using System;
    using Microsoft.CodeAnalysis.Diagnostics;

    /// <summary>
    /// Evaluates analysis contexts determining if it represents NUnit elements according to the
    /// presence of NUnit attributes.
    /// </summary>
    internal class NUnitTestParser: AttributeBasedTestParser
    {
        /// <inheritdoc/>
        protected override string[] TestMethodAttributes { get; } = new[]
        {
            "NUnit.Framework.TestAttribute",
            "NUnit.Framework.TestCaseAttribute",
            "NUnit.Framework.TestCaseSourceAttribute",
        };
    }
}
