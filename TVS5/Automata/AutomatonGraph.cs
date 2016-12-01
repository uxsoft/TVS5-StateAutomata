using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TVS5.Automata
{
    public class AutomatonGraph : QuickGraph.AdjacencyGraph<AutomatonState, AutomatonEdge>
    {
        public void Export(string file)
        {
            var algo = new QuickGraph.Graphviz.GraphvizAlgorithm<AutomatonState, AutomatonEdge>(this);
            algo.FormatEdge += (sender, e) =>
            {
                e.EdgeFormatter.Label = new QuickGraph.Graphviz.Dot.GraphvizEdgeLabel() { Value = $"{e.Edge.Input}/{e.Edge.Output}" };
            };
            algo.FormatVertex += (sender, e) =>
            {
                e.VertexFormatter.Label = e.Vertex.Name;
            };

            string dotContents = algo.Generate();
            try
            {
                string dotFile = Path.ChangeExtension(file, "dot");
                string imgFile = Path.ChangeExtension(file, "jpg");
                File.WriteAllText(dotFile, dotContents);
                Process.Start(new ProcessStartInfo(@"dot.exe", $"-Tjpg \"{dotFile}\" -o \"{imgFile}\"") { CreateNoWindow = true, WindowStyle = ProcessWindowStyle.Hidden })?.WaitForExit();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
