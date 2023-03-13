namespace LFAF_Lab2;

using System;

public class Lab2
{
    public static void Main(String[] args)
    {
        var grammar = new Var20Grammar();
        var finiteAutomaton = grammar.toFiniteAutomaton();
        var grammar2 = finiteAutomaton.ToGrammar();
        bool isEqual = grammar2.isEqual(grammar);
        Console.WriteLine("Is equal {0}", isEqual);

        var finiteAutomatonVar20 = new FiniteAutomatonVar20();
        var isDeterministic = finiteAutomatonVar20.IsDeterministic();
        Console.WriteLine("Is deterministic {0}", isDeterministic);
        var determ = finiteAutomatonVar20.ToDeterministic();
        isDeterministic = determ.IsDeterministic();
        Console.WriteLine("Is deterministic {0}", isDeterministic);
        Console.WriteLine("Deterministic FA: {0}", determ.To_String());

       for (int i = 0; i < 5; i++)
           Console.WriteLine("Word: {0}", grammar.generateWord());

        if (finiteAutomaton.wordIsValid("daaa"))
            Console.WriteLine("Word is valid");
        else
            Console.WriteLine("Word  is not valid");
    }
}
