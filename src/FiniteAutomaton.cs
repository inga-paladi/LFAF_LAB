namespace LFAF_Lab2;

using System;
using System.Text;

public abstract class FiniteAutomaton
{
    public List<string> possibleStates = new List<string>();
    public List<string> alphabet = new List<string>();
    public List<Transition> transitions = new List<Transition>();
    public string initialState = new string("");
    public List<string> finalStates = new List<string>();

    public abstract bool wordIsValid(string word);
}

public class Transition
{
    public string currentState = new string("");
    public string transitionLabel = new string("");
    public string nextState = new string("");
}

public class FiniteAutomatonImpl : FiniteAutomaton
{
    public GrammarImpl ToGrammar()
    {
        var grammar = new GrammarImpl();
        grammar.startCharacter = initialState;

        var productions = new List<Production>();
        foreach (var transition in transitions)
        {
            var production = new Production{
                leftSide = transition.currentState
            };

            var rightSide = new StringBuilder();
            rightSide.Append(transition.transitionLabel);
            if (!finalStates.Contains(transition.nextState))
            {
                rightSide.Append(transition.nextState);
            }
            production.rightSide = rightSide.ToString();
            productions.Add(production);
        }

        grammar.productions = productions;
        grammar.nonTerminalVariables = possibleStates.Except(finalStates).ToList();
        grammar.terminalVariables = alphabet;

        return grammar;
    }

    public override bool wordIsValid(string word)
    {
        var currentState = initialState;
        foreach(var letter in word)
        {
            string transitionLabel = letter.ToString();
            Transition ? transition = transitions.Find(
                transition => transition.currentState == currentState && transition.transitionLabel == transitionLabel
                );
            if (transition == null) return false;
            currentState = transition.nextState;
        }

        return finalStates.IndexOf(currentState) > -1;
    }

    public bool IsDeterministic()
    {
        // Check if there are multiple transitions with the same current state and input symbol
        for (var i = 0; i < transitions.Count; i++)
        {
            var t1 = transitions[i];
            for (var j = i + 1; j < transitions.Count; j++)
            {
                var t2 = transitions[j];
                if (t1.currentState == t2.currentState && t1.transitionLabel == t2.transitionLabel)
                {
                    return false;
                }
            }
        }

        // Check if every state has exactly one transition for each input symbol
        foreach (var state in possibleStates)
        {
            foreach (var symbol in alphabet)
            {
                var count = 0;
                foreach (var transition in transitions)
                {
                    if (transition.currentState == state && transition.transitionLabel == symbol)
                        count++;
                }
                
                if (count > 1)
                    return false;
            }
        }

        return true;
    }
    
    public FiniteAutomatonImpl ToDeterministic()
    {
        var deterministic_FA = new FiniteAutomatonImpl();

        deterministic_FA.possibleStates.Add(string.Join(',', initialState));
        deterministic_FA.initialState = string.Join(',', initialState);

        var unprocessedDfaStates = new Queue<string>();
        unprocessedDfaStates.Enqueue(string.Join(',', initialState));

        while (unprocessedDfaStates.Count > 0){
            var dfaState = unprocessedDfaStates.Dequeue();

            foreach (var finalState in finalStates){
                if (dfaState.Split(',').Contains(finalState)){
                    deterministic_FA.finalStates.Add(dfaState);
                    break;
                }
            }

            foreach (var symbol in alphabet){
                var nfaStates = new HashSet<string>();
                foreach (var state in dfaState.Split(','))
                {
                    // make a list of all states that are reachable from current state = 'state' and 
                    // transitionLabel = 'symbol'
                    foreach (var transition in transitions)
                    {
                        if (transition.currentState == state && transition.transitionLabel == symbol)
                            nfaStates.Add(transition.nextState);
                    }
                }

                if (nfaStates.Count == 0){
                    continue;
                }

                var newDfaState = string.Join(',', nfaStates);

                if (!deterministic_FA.possibleStates.Contains(newDfaState))
                {
                    deterministic_FA.possibleStates.Add(newDfaState);
                    unprocessedDfaStates.Enqueue(newDfaState);
                }

                deterministic_FA.transitions.Add(new Transition
                {
                    currentState = dfaState,
                    transitionLabel = symbol,
                    nextState = newDfaState
                });
                deterministic_FA.alphabet = alphabet;
            }
        }

        return deterministic_FA;
    }

    public string To_String()
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.AppendFormat("States: {0}\n", possibleStates);
        stringBuilder.AppendFormat("Alphabet: {0}\n", alphabet);
        stringBuilder.AppendFormat("Initial state: {0}\n", initialState);
        stringBuilder.AppendFormat("Final states: {0}\n", finalStates);
        
        return stringBuilder.ToString();
    }
}

public class FiniteAutomatonVar20 : FiniteAutomatonImpl
{
    public FiniteAutomatonVar20()
    {
        possibleStates = new List<string>{"q0", "q1", "q2", "q3"};
        alphabet = new List<string>{"a", "b", "c"};
        transitions = new List<Transition>{
            new Transition{currentState = "q0", transitionLabel = "a", nextState = "q0"},
            new Transition{currentState = "q0", transitionLabel = "a", nextState = "q1"},
            new Transition{currentState = "q1", transitionLabel = "b", nextState = "q2"},
            new Transition{currentState = "q2", transitionLabel = "a", nextState = "q2"},
            new Transition{currentState = "q2", transitionLabel = "c", nextState = "q3"},
            new Transition{currentState = "q3", transitionLabel = "c", nextState = "q3"}
        };
        initialState = "q0";
        finalStates = new List<string>{"q3"};
    }
}
