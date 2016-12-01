using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MoreLinq;
using System.Text;
using System.Threading.Tasks;
using TVS5.Automata;

namespace TVS5
{
    class Program
    {
        static void Main(string[] args)
        {

            var artificialAutomaton = AbominationCsvReader.Parse("Input/g1A23A.csv");
            Report("g1A23A", artificialAutomaton);

            var semestralAutomaton = SemestralTask.Build();
            Report("Zboží v eshopu", semestralAutomaton);

            Console.ReadKey();
        }

        static void Report(string name, Automaton automaton)
        {
            using (var sw = new StreamWriter($"{name} - output.md"))
            {
                var WriteLine = new Action<string>(line =>
                {
                    Console.WriteLine(line);
                    sw.WriteLine(line);
                });

                //Title
                WriteLine($"# Testing State Automata: {name}\n");

                string imgFileName = $"{name}-graph.jpg";
                automaton.Graph.Export(imgFileName);
                WriteLine($"<img src='{imgFileName}' alt='graph'/>");

                //State Coverage
                WriteLine("\n## State Coverage\n");
                WriteLine("Set L was generated using function StateCoverage()\n");
                foreach (var path in automaton.StateCoverage())
                    WriteLine($"[{string.Join(", ", path.Select(e => e.Input))}]");


                //Transition Coverage
                WriteLine("\n## Transition Coverage\n");
                WriteLine("Set T was generated using function TransitionCoverage(). T = L • (Input^1 ∪ {<>}).\n");
                foreach (var path in automaton.TransitionCoverage())
                    WriteLine($"[{string.Join(", ", path.Select(e => e.Input))}]");


                //Minimalization
                WriteLine("\n## Minimalization\n");
                bool minimalized = false;
                while (automaton.Minimalize())
                {
                    minimalized = true;
                }

                if (minimalized)
                {
                    WriteLine("The automaton wasn't in its minimal form and had to be minimized.\n");

                    imgFileName = $"{name}-minimized.jpg";
                    automaton.Graph.Export(imgFileName);
                    WriteLine($"<img src='{imgFileName}' alt='graph'/>");
                }
                else
                {
                    WriteLine("The automaton is already in its minimal form.");
                }

                //Characteristic Set
                WriteLine("\n## Characteristic Set\n");
                WriteLine("Set W was generated using function CharacteristicSet() on a minimized automaton.\n");
                foreach (var path in automaton.CharacteristicSet())
                {
                    WriteLine($"{{{path.ToDelimitedString(", ")}}}");
                }

                //Hidden States
                WriteLine("\n## Hidden States\n");
                WriteLine("Set Z was generated using function HiddenStatesCoverage(). For depth k=1: Z = Input • W ∪ W.\n");
                foreach (var path in automaton.HiddenStatesCoverage())
                    WriteLine($"[{path.ToDelimitedString(", ")}]");

                //Final Test Suite
                WriteLine("\n## Final Test Suite\n");
                WriteLine("Set F was generated using function ComprehensiveCoverage(). F = T • Z.\n");
                foreach (var path in automaton.ComprehensiveCoverage())
                    WriteLine($"[{path.ToDelimitedString(", ")}]");
            }
        }
    }
}
