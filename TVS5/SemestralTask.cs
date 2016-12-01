using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TVS5.Automata;

namespace TVS5
{
    public class SemestralTask
    {
        public static Automaton Build()
        {
            var automaton = new Automaton();
            automaton.EntryState = automaton.GetOrCreateState("Nonexistant");
            automaton.ExitState = automaton.GetOrCreateState("Discontinued");
            automaton.AddTransition(automaton.GetOrCreateState("Nonexistant"), automaton.GetOrCreateState("Available"), "CreateAvailable", "Available");
            automaton.AddTransition(automaton.GetOrCreateState("Nonexistant"), automaton.GetOrCreateState("Preorder"), "CreatePreorder", "Preorder");
            automaton.AddTransition(automaton.GetOrCreateState("Preorder"), automaton.GetOrCreateState("Preorder"), "ToPreorder", "Preorder");
            automaton.AddTransition(automaton.GetOrCreateState("Preorder"), automaton.GetOrCreateState("Available"), "ToAvailable", "Available");
            automaton.AddTransition(automaton.GetOrCreateState("Preorder"), automaton.GetOrCreateState("OutOfStock"), "ToOutOfStock", "OutOfStock");
            automaton.AddTransition(automaton.GetOrCreateState("Preorder"), automaton.GetOrCreateState("OnTheWay"), "ToOnTheWay", "OnTheWay");
            automaton.AddTransition(automaton.GetOrCreateState("Preorder"), automaton.GetOrCreateState("Discontinued"), "ToDiscontinued", "Discontinued");
            automaton.AddTransition(automaton.GetOrCreateState("Available"), automaton.GetOrCreateState("Available"), "ToAvailable", "Available");
            automaton.AddTransition(automaton.GetOrCreateState("Available"), automaton.GetOrCreateState("OutOfStock"), "ToOutOfStock", "OutOfStock");
            automaton.AddTransition(automaton.GetOrCreateState("Available"), automaton.GetOrCreateState("OnTheWay"), "ToOnTheWay", "OnTheWay");
            automaton.AddTransition(automaton.GetOrCreateState("Available"), automaton.GetOrCreateState("Discontinued"), "ToDiscontinued", "Discontinued");
            automaton.AddTransition(automaton.GetOrCreateState("OutOfStock"), automaton.GetOrCreateState("OutOfStock"), "ToOutOfStock", "OutOfStock");
            automaton.AddTransition(automaton.GetOrCreateState("OutOfStock"), automaton.GetOrCreateState("Available"), "ToAvailable", "Available");
            automaton.AddTransition(automaton.GetOrCreateState("OutOfStock"), automaton.GetOrCreateState("OnTheWay"), "ToOnTheWay", "OnTheWay");
            automaton.AddTransition(automaton.GetOrCreateState("OutOfStock"), automaton.GetOrCreateState("Discontinued"), "ToDiscontinued", "Discontinued");
            automaton.AddTransition(automaton.GetOrCreateState("OnTheWay"), automaton.GetOrCreateState("OnTheWay"), "ToOnTheWay", "OnTheWay");
            automaton.AddTransition(automaton.GetOrCreateState("OnTheWay"), automaton.GetOrCreateState("Available"), "ToAvailable", "Available");
            automaton.AddTransition(automaton.GetOrCreateState("OnTheWay"), automaton.GetOrCreateState("Discontinued"), "ToDiscontinued", "Discontinued");
            //automaton.AddTransition(automaton.GetOrCreateState(""), automaton.GetOrCreateState(""), "", "");

            return automaton;
        }
    }
}
