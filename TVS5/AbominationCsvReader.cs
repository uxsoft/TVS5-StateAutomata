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
            int i = 0;
            using (StreamReader sr = new StreamReader(file))
            {
                string[] inputs = new string[3];
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine().Split(new string[] { "," }, StringSplitOptions.None);
                    i++;

                    if (i == 1)
                    {
                        automaton.EntryState = automaton.GetOrCreateState(line[1].Trim());
                    }
                    else if (i == 2)
                    {
                        automaton.ExitState = automaton.GetOrCreateState(line[1].Trim());
                    }
                    else if (i == 3)
                    {
                        automaton.DefaultOutput = line[1].Trim();
                    }
                    else if (i == 6)
                    {
                        inputs[0] = line[1].Trim();
                        inputs[1] = line[2].Trim();
                        inputs[2] = line[3].Trim();
                    }
                    else if (i > 6)
                    {
                        string from = line[0].Trim();
                        for (int col = 0; col < 3; col++)
                        {
                            string to = line[col + 1].Trim();
                            string input = inputs[col].Trim();
                            string output = line[col + 5].Trim();
                            automaton.AddTransition(automaton.GetOrCreateState(from), automaton.GetOrCreateState(to), input, output);
                        }
                    }
                }
            }

            return automaton;
        }
    }
}