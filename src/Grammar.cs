namespace LFAF_LABORATORY;

using System;

public abstract class Grammar
{
    public List<string> nonTerminalVariables = new List<string>();
    public List<string> terminalVariables = new List<string>();
    public List<Production> productions = new List<Production>();
    public string startCharacter = new string("");

    public abstract string generateWord();
    public abstract FiniteAutomaton toFiniteAutomaton();
}

public class Production
{
    public string leftSide = new string("");
    public string rightSide = new string("");

    public bool isEqual(Production production)
    {
        return Equals(production);
    }

    public bool Equals(Production obj)
    {
        return this.leftSide == obj.leftSide
            && this.rightSide == obj.rightSide;
    }

    override public int GetHashCode()
    {
        var concatenatedProd = String.Concat(this.leftSide, this.rightSide);
        return concatenatedProd.GetHashCode();
    }

    public static bool operator==(Production lhs, Production rhs)
    {
        return lhs.Equals(rhs); 
    }

    public static bool operator!=(Production lhs, Production rhs)
    {
        return !lhs.Equals(rhs);
    }
}

public class GrammarImpl : Grammar
{
    public string getChomskyClassification() {
        var isType0 = true;
        var isType1 = true;
        var isType2 = true;
        var isType3 = true;

        // Check if each production meets the requirements for each classification
        foreach (var production in productions) {
            if (production.rightSide.Length == 0) {
                // Production has empty string on right side (Type 3)
                isType0 = false;
                isType1 = false;
            } else if (production.rightSide.Length == 1) {
                if (isTerminal(production.rightSide[0].ToString() )) {
                    // Production has terminal symbol on right side (Type 3)
                    isType0 = false;
                    isType1 = false;
                } else {
                    // Production has non-terminal symbol on right side (Type 2)
                    isType0 = false;
                }
            } else if (production.rightSide.Length == 2) {
                if (!isTerminal(production.rightSide[0].ToString()) && !isTerminal(production.rightSide[1].ToString())) {
                    // Production has two non-terminal symbols on right side (Type 0)
                    isType1 = false;
                    isType3 = false;
                } else {
                    // Production has at least one terminal symbol on right side (Type 2)
                    isType0 = false;
                }
            } else {
                // Production has more than two symbols on right side (Type 0)
                isType1 = false;
                isType3 = false;
            }
        }

        // Determine Chomsky classification
        if (isType0) {
            return "Type 0 (Unrestricted grammar)";
        } else if (isType1) {
            return "Type 1 (Context-sensitive grammar)";
        } else if (isType2) {
            return "Type 2 (Context-free grammar)";
        } else if (isType3) {
            return "Type 3 (Regular grammar)";
        } else {
            return "Unknown classification";
        }
    }

    private bool isTerminal(string c) {
        foreach (var terminalVar in terminalVariables)
            if (terminalVar == c) return true;
        
        return false;
    }

    public override string generateWord()
    {
        var generatedString = startCharacter;
        while(true)
        {
            var lastCharacter = generatedString.Last();
            // Console.WriteLine("the last char is {0}",lastCharacter);
            if (terminalVariables.Exists(element => element == lastCharacter.ToString()))
                break;
            var nonTermProd = productions.Where(val => val.leftSide == lastCharacter.ToString());
            var random = new Random();
            var randomNr = random.Next(0, nonTermProd.Count());
            var prodAles = nonTermProd.ElementAt(randomNr);
            // Remove the last one and..
            generatedString = generatedString.Remove(generatedString.Length-1, 1);
            // .. replace with the right side of production. ex A -> aB
            generatedString += prodAles.rightSide;
        }

        return generatedString;
    }

    public override FiniteAutomatonImpl toFiniteAutomaton()
    {
        var finalState = "F";

        var possibleStates = new List<String>{finalState};
        foreach (var state in nonTerminalVariables)
            possibleStates.Add(state);

        var transitions = new List<Transition>();
        foreach (var product in productions)
        {
            transitions.Add(
                new Transition{
                    currentState = product.leftSide,
                    transitionLabel = product.rightSide[0].ToString(),
                    nextState = product.rightSide.Length == 1 ?
                        finalState
                        : product.rightSide[1].ToString()}
            );
        }

        var var20fa = new FiniteAutomatonImpl();
        var20fa.possibleStates = possibleStates;
        var20fa.alphabet = terminalVariables;
        var20fa.transitions = transitions;
        var20fa.initialState = startCharacter.ToString();
        var20fa.finalStates = new List<string>{finalState};
        return var20fa;
    }

    public bool isEqual(GrammarImpl grammar)
    {
        if (!this.nonTerminalVariables.SequenceEqual(grammar.nonTerminalVariables))
            return false;
        if (!this.terminalVariables.SequenceEqual(grammar.terminalVariables))
            return false;
        if (this.productions.Count != grammar.productions.Count)
            return false;
        for (var i = 0; i < this.productions.Count; i++)
            if (!this.productions[i].isEqual(grammar.productions[i]))
                return false;
        if (this.startCharacter != grammar.startCharacter)
            return false;

        return true;
    }
} // class GrammarImpl

public class Var20Grammar : GrammarImpl
{
    public Var20Grammar()
    {
        nonTerminalVariables = new List<string>{"S", "A", "B", "C"};
        terminalVariables = new List<string>{"a","b","c","d"};
        productions = new List<Production>{
            new Production{leftSide = "S", rightSide = "dA"},
            new Production{leftSide = "A", rightSide = "d"},
            new Production{leftSide = "A", rightSide = "aB"},
            new Production{leftSide = "B", rightSide = "bC"},
            new Production{leftSide = "C", rightSide = "cA"},
            new Production{leftSide = "C", rightSide = "aS"}
            };
        startCharacter = "S";
    }
} // class Var20Grammar
