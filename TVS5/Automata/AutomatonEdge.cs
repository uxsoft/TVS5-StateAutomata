using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TVS5.Automata
{
    public class AutomatonEdge : QuickGraph.Edge<AutomatonState>
    {
        public AutomatonEdge(AutomatonState source, AutomatonState target) : base(source, target)
        {

        }
    }
}
