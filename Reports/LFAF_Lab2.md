# Topic: Determinism in Finite Automata. Conversion from NDFA 2 DFA. Chomsky Hierarchy.
### Course: Formal Languages & Finite Automata
### Author: Paladi Inga
## Theory
The Regular Grammar is defined as follows:
Let: 
V_N be the set of non-terminal symbols
V_T be the set of terminal symbols
P be the set of production rules

Grammar by definition can have different Chomsky Types, there being 4 types labeled as 0,1,2 and 3.
Type 0:
Any form of string conversion.
Type 1:
Conditional string conversion, can't convert to empty string.
Type 2:
Has to have one non-terminal char in the condition string, can convert in any type of string.
Type 3:
Can convert in only the strings of form T,TN or T,NT, where T is in V_T and N is in V_N. 

The Finite Automaton is defined as follows:

Let:
Q be the set of all possible states
Σ be the alphabet
δ be the transition function
q0 be the initial state
F be the set of final states
The Chomsky hierarchy is a classification of formal grammars, named after linguist Noam Chomsky. It consists of
four types, labeled 0 through 3, 
which describe different classes of formal languages.

In Type 0 grammars, there are no restrictions on the production rules. They can generate any possible string over
the given alphabet.

Type 1 grammars generate context-sensitive languages. In these grammars, the left-hand side of each production 
rule must be at least as long as the right-hand side, and it cannot produce the empty string.

Type 2 grammars generate context-free languages. In these grammars, the left-hand side of each production rule
consists of a single nonterminal symbol, and the right-hand side can be any string of terminals and nonterminals.

Type 3 grammars generate regular languages. In these grammars, the left-hand side of each production rule consists 
of a single nonterminal symbol, and the right-hand side can only be a single terminal symbol or a nonterminal 
symbol followed by a single terminal symbol.
## Objectives:
1. Understand what an automaton is and what it can be used for.

2. Continuing the work in the same repository and the same project, the following need to be added:
    a. Provide a function in your grammar type/class that could classify the grammar based on Chomsky hierarchy.

    b. For this you can use the variant from the previous lab.

3. According to your variant number (by universal convention it is register ID), get the finite automaton 
definition and do the following tasks:

    a. Implement conversion of a finite automaton to a regular grammar.

    b. Determine whether your FA is deterministic or non-deterministic.

    c. Implement some functionality that would convert an NDFA to a DFA.
    
    d. Represent the finite automaton graphically (Optional, and can be considered as a __*bonus point*__):
      
    - You can use external libraries, tools or APIs to generate the figures/diagrams.
        
    - Your program needs to gather and send the data about the automaton and the lib/tool/API return the visual representation.
    ##Implementation description
    *Implement conversion of a finite automaton to a regular grammar.
    
```
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
```
  The method starts by creating a new instance of the "GrammarImpl" class, 
  which is a data structure that represents a grammar. The initial state 
  of the grammar is set to the initial state of the finite automaton.
  the method iterates over all the transitions in the automaton, and 
  for each transition, it creates a new production rule in the grammar.
  The left side of the production rule is set to the current state of
  the transition, and the right side is constructed by appending the 
  transition label (i.e., the input symbol) and the next state. If the
  next state is not a final state, it is also added to the right side.
* Determine whether your FA is deterministic or non-deterministic.
```
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
```
The method first checks if there are multiple transitions with the same 
current state and input symbol. If there are, then the FA is 
non-deterministic and the method returns false. Next, the method checks
if every state has exactly one transition for each input symbol. If any
state has more than one transition for a given input symbol, the FA is 
non-deterministic and the method returns false. If neither 
condition is met, the FA is deterministic and the method returns true.
*Implement some functionality that would convert an NDFA to a DFA.
```
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
}
For each state in the queue, the method checks if it contains any final 
states of the NFA, and if so, adds it to the final states of the DFA.
The method then iterates over each symbol in the alphabet of the NFA. 
For each symbol, it finds all the states that are reachable from the 
current DFA state by following that symbol, using the transitions of 
the NFA. If there are no reachable states, the method skips to the next
symbol.

If there are reachable states, the method converts them into a 
comma-separated string and checks if this new state is already in 
the set of possible states of the DFA. If not, it adds the new state
to the set and to the queue of unprocessed states.

Finally, the method adds a new transition to the DFA that connects the 
current DFA state with the new state using the current symbol, and adds
the alphabet to the DFA. This process continues until there are no
unprocessed states left in the queue.

```