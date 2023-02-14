using System;

namespace LFAF_LAB1
{

public abstract class Grammar
{
    protected char[] nonTerminalVariables;//{'a','b'} 
    protected char[] terminalVariables;
    protected Production[] production;
    protected char startCharacter;

    public abstract String generateWord();
    public abstract FiniteAutomaton toFiniteAutomaton();
}

public class Production
{
    public char leftSide;
    public String rightSide;
}

public class Var20Grammar : Grammar
{   
    public Var20Grammar()
    {
        nonTerminalVariables = new char[] {'S', 'A', 'B', 'C'};
        terminalVariables = new char[] {'a','b','c','d'};
        production = new Production[] {
            new Production{leftSide = 'S', rightSide = "dA"},
            new Production{leftSide = 'A', rightSide = "d"},
            new Production{leftSide = 'A', rightSide = "aB"},
            new Production{leftSide = 'B', rightSide = "bC"},
            new Production{leftSide = 'C', rightSide = "cA"},
            new Production{leftSide = 'C', rightSide = "aS"}
            };
        startCharacter = 'S';
    }

    public override String generateWord()
    {
        String generatedString = "";
        generatedString += startCharacter;
        while(true)
        {
            char lastCharacter = generatedString[generatedString.Length-1];
            // Console.WriteLine("the last char is {0}",lastCharacter);
            bool isTerminal = Array.Exists(terminalVariables, element => element == lastCharacter  );
            if (isTerminal) break;
            var nonTermProd = production.Where(val => val.leftSide == lastCharacter);
            Random random = new Random();
            int randomNr = random.Next(0, nonTermProd.Count());
            Production prodAles = nonTermProd.ElementAt(randomNr);
            generatedString = generatedString.Remove(generatedString.Length-1, 1);
            generatedString += prodAles.rightSide;
        }

        return generatedString;
    }

    public override FiniteAutomaton toFiniteAutomaton()
    {   
        char finalState = 'F';

        List<char> possibleStates = new List<char>{finalState};
        foreach (char state in nonTerminalVariables)
            possibleStates.Add(state);
        
        List<Transition> transitions = new List<Transition>();
        foreach (Production product in production)
        {
            transitions.Add(
                new Transition{
                    currentState = product.leftSide,
                    transitionLabel = product.rightSide[0],
                    nextState = product.rightSide.Length == 1 ?
                        finalState
                        : product.rightSide[1]}
            );
        }

        Var20FiniteAutomaton var20fa = new Var20FiniteAutomaton();
        var20fa.possibleStates = possibleStates.ToArray();
        var20fa.alphabet = terminalVariables;
        var20fa.transitions = transitions.ToArray();        
        var20fa.initialState = startCharacter;
        var20fa.finalStates = new char[] {finalState};
        return var20fa;
    }
} // class Var20Grammar

} // namespace LFAF_LAB1