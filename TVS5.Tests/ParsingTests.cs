using System;
using Xunit;

namespace TVS5.Tests
{
    public class ParsingTests
    {
        [Fact]
        public void ParseArtificialInput()
        {
            var automaton = AbominationCsvReader.Parse("Input/g1A23A.csv");

        }
    }
}
