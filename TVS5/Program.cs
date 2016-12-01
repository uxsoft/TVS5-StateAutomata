using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
                foreach (var path in automaton.StateCoverage())
                    WriteLine($"[{string.Join(", ", path.Select(e => e.Input))}]");


                //Transition Coverage
                WriteLine("\n## Transition Coverage\n");
                foreach (var path in automaton.TransitionCoverage())
                    WriteLine($"[{string.Join(", ", path.Select(e => e.Input))}]");


                //Minimalization
                WriteLine("\n## Minimalization\n");
                if (automaton.Minimalize())
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

            }
        }
    }
}
