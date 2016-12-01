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

        public Tuple<AutomatonState, string> Next(AutomatonState current, string input)
        {
            var edge = Graph.OutEdges(current).SingleOrDefault(e => e.Input == input);
            if (edge != null)
                return new Tuple<AutomatonState, string>(edge.Target, edge.Output);
            else return new Tuple<AutomatonState, string>(current, DefaultOutput);
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
                var outEdges = Graph.OutEdges(s).OrderBy(e => e.Input).Select(e => $"{e.Input} -> {e.Target.Name}");

                return outEdges.ToDelimitedString(", ");
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
                .Where(t => t.v1 != t.v2)
                .Select(t => CharacteristicSet(t.v1, t.v2))
                .DistinctBy(p => p.ToDelimitedString(", "));
        }

        public IEnumerable<string> CharacteristicSet(AutomatonState v1, AutomatonState v2)
        {
            var queue = new Queue<CharacteristicSetQueueItem>();
            queue.Enqueue(new CharacteristicSetQueueItem() { V1 = v1, V2 = v2 });

            while (queue.Count > 0)
            {
                var item = queue.Dequeue();

                foreach (var input in Graph.OutEdges(item.V1).Concat(Graph.OutEdges(item.V2)).Select(e => e.Input))
                {
                    var v1Next = Next(item.V1, input);
                    var v2Next = Next(item.V2, input);
                    if (v1Next.Item2 != v2Next.Item2)
                    {
                        return item.Path.Concat(input);
                    }
                    else
                    {
                        queue.Enqueue(new CharacteristicSetQueueItem() { V1 = v1Next.Item1, V2 = v2Next.Item1, Path = item.Path.Concat(input) });
                    }
                }
            }

            return null;
        }

        private class CharacteristicSetQueueItem
        {
            public AutomatonState V1 { get; set; }
            public AutomatonState V2 { get; set; }
            public IEnumerable<string> Path { get; set; } = Enumerable.Empty<string>();
        }
    }
}
