using System;

namespace LFAF_LAB1
{
    public class Lab1
    {
        public static void Main(String[] args)
        {
            Var20Grammar grammar = new Var20Grammar();
            
            for (int i = 0; i < 5; i++)
                Console.WriteLine("Word: {0}", grammar.generateWord());

            var finiteAutomaton = grammar.toFiniteAutomaton();
            if (finiteAutomaton.wordIsValid("da"))           
                Console.WriteLine("Word dabadd is valid");
            else
                Console.WriteLine("Word dabadd is not valid");
        }
    }
} // namespace LFAF_LAB1