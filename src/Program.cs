namespace LFAF_LABORATORY;

using LFAF_LABORATORY.Parser;
using System;
public class LAB_3
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
        
        //Lexer implementation---------------------------------------------------
        string input = "if (variabila == 1 ){}";
        List<Token> tokens;
        try
        {
            tokens = Lexer.Tokenize(input);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return;
        }
    
        // Chomsky normal form ------------------------------------------------
        ChomskyNormalForm cnf = new ChomskyNormalForm();
        Console.WriteLine("Eliminate epsilon productions");
        cnf.EliminateEProductions();
        foreach (var production in cnf.productions)
        {
            Console.WriteLine("{0} -> {1}", production.leftSide, production.rightSide);
        }

        Console.WriteLine("Eliminate unit productions");
        cnf.EliminateUnitProductions();
        foreach (Production production in cnf.productions)
        {
            Console.WriteLine(production.leftSide + " -> " + production.rightSide);
        }

        Console.WriteLine("Eliminate inaccesible symbols");
        cnf.EliminateInaccesibleSymbols();
        foreach (Production production in cnf.productions)
        {
            Console.WriteLine(production.leftSide + " -> " + production.rightSide);
        }

        Console.WriteLine("Eliminate unproductive productions");
        cnf.EliminateUnproductiveSymbols();
        foreach (Production production in cnf.productions)
        {
            Console.WriteLine(production.leftSide + " -> " + production.rightSide);
        }

        Console.WriteLine("Convert to CNF");
        cnf.ConvertToCNF();
        foreach (Production production in cnf.productions)
        {
            Console.WriteLine(production.leftSide + " -> " + production.rightSide);
        }

        //Parser implementation---------------------------------------------------
        var parser1 = new Parser.Parser();
        try
        {
            parser1.Init(tokens);
            Console.WriteLine(parser1.Parse());
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}


