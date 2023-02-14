using System;

namespace LFAF_LAB1
{

public abstract class FiniteAutomaton
{
    public char[] possibleStates;
    public char[] alphabet;
    public Transition[] transitions;
    public char initialState;
    public char[] finalStates;

    public abstract bool wordIsValid(String word);
}

public class Transition
{
    public char currentState;
    public char transitionLabel;
    public char nextState;
}

public class Var20FiniteAutomaton : FiniteAutomaton
{
    public char[] possibleStates {set; get;}
    public char[] alphabet {set; get;}
    public Transition[] transitions {set; get;}
    public char initialState {set; get;}
    public char[] finalStates {set; get;}

    public override bool wordIsValid(String word)
    {
        char currentState = initialState;
        foreach(char letter in word)
        {
            char transitionLabel = letter;
            Transition transition = Array.Find(transitions,
                transition => transition.currentState == currentState && transition.transitionLabel == letter);
            if (transition == null) return false;
            currentState = transition.nextState;
        }

        if (Array.IndexOf(finalStates, currentState) > -1)
            return true;

        return false;
    }
}

} // namespace LFAF_LAB1