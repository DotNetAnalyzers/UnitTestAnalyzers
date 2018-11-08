namespace UnitTestAnalyzers.Parsers
{
    using System;
    using Settings.ObjectModel;

    internal static class UnitTestParserFactory
    {
        public static IUnitTestParser Create(AnalyzersSettings settings)
        {
            switch (settings.UnitTestFramework)
            {
                case UnitTestFramework.MSTest:
                    return new MSTestParser();
                case UnitTestFramework.Xunit:
                    return new XunitTestParser();
                case UnitTestFramework.NUnit:
                    return new NUnitTestParser();
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
