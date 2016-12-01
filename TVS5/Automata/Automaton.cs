using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickGraph.Algorithms;
using MoreLinq;

namespace TVS5.Automata
{
    public class Automaton
    {
        public string DefaultOutput { get; set; }
        public AutomatonState EntryState { get; set; }
        public AutomatonState ExitState { get; set; }
        public AutomatonGraph Graph { get; set; } = new AutomatonGraph();
        public Dictionary<string, AutomatonState> States { get; set; } = new Dictionary<string, AutomatonState>();


        public AutomatonState GetOrCreateState(string stateName)
        {
            if (States.ContainsKey(stateName))
                return States[stateName];
            else return AddState(stateName);
        }

        public AutomatonState AddState(string stateName)
        {
            var state = new AutomatonState() { Name = stateName };
            Graph.AddVertex(state);
            States[stateName] = state;
            return state;
        }

        public AutomatonEdge AddTransition(AutomatonState from, AutomatonState to, string input, string output)
        {
            var edge = new AutomatonEdge(from, to) { Input = input, Output = output };
            Graph.AddEdge(edge);
            return edge;
        }

        public IEnumerable<IEnumerable<AutomatonEdge>> StateCoverage()
        {
            var tryGetPath = Graph.ShortestPathsDijkstra(e => 1, EntryState);

            foreach (var destination in States.Values)
                if (destination != EntryState)
                {
                    IEnumerable<AutomatonEdge> path;
                    if (!tryGetPath(destination, out path))
                    {
                        throw new Exception("Inaccessible state!");
                    }
                    yield return path;
                }
        }

        public IEnumerable<IEnumerable<AutomatonEdge>> TransitionCoverage()
        {
            foreach (var path in StateCoverage())
                foreach (var newEdge in Graph.OutEdges(path.Last().Target))
                    yield return path.Concat(Enumerable.Repeat(newEdge, 1));
        }

        public bool Minimalize()
        {
            var changed = false;

            var groups = States.Values.GroupBy(s =>
            {
                var outEdges = Graph.Edges.Where(e => e.Source == s).OrderBy(e => e.Input).Select(e => $"{e.Input}/{e.Output}");
                var inEdges = Graph.Edges.Where(e => e.Target == s).OrderBy(e => e.Input).Select(e => $"{e.Input}/{e.Output}");

                return $"[{string.Join(", ", inEdges)}] -> [{string.Join(", ", inEdges)}]";
            });

            foreach (var group in groups)
                if (group.Count() > 1)
                {
                    changed = true;
                    string newName = string.Join("+", group.Select(s => s.Name));
                    group.First().Name = newName;
                    foreach (var exState in group.Skip(1))
                    {
                        Graph.RemoveEdgeIf(e => e.Source == exState || e.Target == exState);
                        Graph.RemoveVertex(exState);
                    }
                }

            States = Graph.Vertices.ToDictionary(s => s.Name);
            return changed;
        }

        public IEnumerable<IEnumerable<string>> CharacteristicSet()
        {
            return Graph.Vertices.SelectMany(v => Graph.Vertices.Select(v2 => new { v1 = v, v2 = v2 }))
                .Select(t => CharacteristicSet(t.v1, t.v2))
                .Distinct();
        }

        public IEnumerable<string> CharacteristicSet(AutomatonState v1, AutomatonState v2)
        {
            return null;
        }
    }
}
