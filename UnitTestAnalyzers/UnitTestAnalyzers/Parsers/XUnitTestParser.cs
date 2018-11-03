namespace UnitTestAnalyzers.Parsers
{
    /// <summary>
    /// Evaluates analysis contexts determining if it represents Xunit elements according to the
    /// presence of Xunit attributes.
    /// </summary>
    /// <seealso cref="UnitTestAnalyzers.Parsers.IUnitTestParser" />
    internal class XunitTestParser : AttributeBasedTestParser
    {
        /// <inheritdoc/>
        protected override string[] TestMethodAttributes { get; } = {
            "Xunit.FactAttribute",
            "Xunit.TheoryAttribute"
        };
    }
}
