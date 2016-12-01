using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TVS5.Tests
{
    public class CharacteristicSetTests
    {
        [Fact]
        public void Task40Test()
        {
            var automaton = AbominationCsvReader.Parse("Input/g1A40A.csv");
            automaton.Minimalize();
            string result = string.Join(", ", automaton.CharacteristicSet().Select(p => $"({string.Join(", ", p)})"));
            string correctResult = "(e67, e67), (e67, e98), (e87), (e98, e67), (e98, e87), (e98, e98)";

            Assert.Equal(correctResult, result);
        }
    }
}
