using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TVS5.Automata;

namespace TVS5
{
    public class AbominationCsvReader
    {
        public static Automaton Parse(string file)
        {
            Automaton automaton = new Automaton();
            using (StreamReader sr = new StreamReader(file))
            {
                var line = sr.ReadLine().Split(new string[] {","}, StringSplitOptions.None);

            }

            return automaton;
        }
    }
}