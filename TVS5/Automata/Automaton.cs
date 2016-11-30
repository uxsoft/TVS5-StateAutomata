using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TVS5.Automata
{
    public class Automaton
    {
        public string DefaultOutput { get; internal set; }
        public string EntryState { get; internal set; }
        public string ExitState { get; internal set; }
        public AutomatonGraph Graph { get; set; }
        public Dictionary<string, AutomatonState> States { get; set; }


        public void AddState(string stateName)
        {
            var state = new AutomatonState() { Name = stateName };
            Graph.AddVertex(state);
            States[stateName] = state;
        }

        public void AddTransition(string from, string to, string input, string output)
        {
            Graph.AddEdge(new AutomatonEdge(States[from], States[to]) { Input = input, Output = output });
        }
    }
}
