using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using NPOI.SS.Formula.Functions;

namespace LFAF_LAB
{
    class ChomskyNormalForm : GrammarImpl
    {
        public ChomskyNormalForm()
        {
            nonTerminalVariables = new List<string> { "S", "A", "B", "C", "D" };
            terminalVariables = new List<string> { "a", "b" };
            productions = new List<Production>{
                new Production{leftSide = "S", rightSide = "aB"},
                new Production{leftSide = "S", rightSide = "A"},
                new Production{leftSide = "A", rightSide = "B"},
                new Production{leftSide = "A", rightSide = "SA"},
                new Production{leftSide = "A", rightSide = "bBA"},
                new Production{leftSide = "A", rightSide = "b"},
                new Production{leftSide = "B", rightSide = "b"},
                new Production{leftSide = "B", rightSide = "bS"},
                new Production{leftSide = "B", rightSide = "aD"},
                new Production{leftSide = "B", rightSide = ""},
                new Production{leftSide = "C", rightSide = "Ba"},
                new Production{leftSide = "D", rightSide = "AA"}
            };
            startCharacter = "S";

        }
        
        //STEP 1: eliminate EPSILON productions
        public void EliminateEProductions()
        {
            // Find nullable symbols
            List<string> nullableSymbols = new List<string>();
            bool hasChanges = true;
            while (hasChanges)
            {
                hasChanges = false;
                foreach (string symbol in nonTerminalVariables)
                {
                    if (nullableSymbols.Contains(symbol))
                    {
                        continue;
                    }
                    foreach (Production production in productions)
                    {
                        if (production.rightSide.Contains(symbol))
                        {
                            bool isNullable = true;
                            foreach (char c in production.rightSide)
                            {
                                if (!nullableSymbols.Contains(c.ToString()) && c.ToString() != symbol && nonTerminalVariables.Contains(c.ToString()))
                                {
                                    isNullable = false;
                                    break;
                                }
                            }
                            if (isNullable)
                            {
                                nullableSymbols.Add(symbol);
                                hasChanges = true;
                                break;
                            }
                        }
                    }
                }
            }

            // Remove e-productions
            List<Production> newProductions = new List<Production>();
            foreach (Production production in productions)
            {
                if (production.rightSide == "")
                {
                    continue;
                }
                bool isNullable = true;
                foreach (char c in production.rightSide)
                {
                    if (!nullableSymbols.Contains(c.ToString()) && nonTerminalVariables.Contains(c.ToString()))
                    {
                        isNullable = false;
                        break;
                    }
                }
                if (isNullable)
                {
                    List<string> combinations = GetCombinations(production.rightSide, nullableSymbols);
                    foreach (string combination in combinations)
                    {
                        if (combination != "")
                        {
                            newProductions.Add(new Production { leftSide = production.leftSide, rightSide = combination });
                        }
                    }
                }
                else
                {
                    newProductions.Add(production);
                }
            }

            productions.Clear();
            var uniqueProd = new HashSet<Production>(newProductions);
            foreach (var prod in uniqueProd)
            {
                if (prod.leftSide != prod.rightSide)
                    productions.Add(prod);
            }
        }
        private List<string> GetCombinations(string s, List<string> nullableSymbols)
        {
            List<string> combinations = new List<string>();
            combinations.Add(s);
            foreach (char c in s)
            {
                if (nullableSymbols.Contains(c.ToString()))
                {
                    List<string> newCombinations = new List<string>();
                    foreach (string combination in combinations)
                    {
                        newCombinations.Add(combination.Replace(c.ToString(), ""));
                    }
                    combinations.AddRange(newCombinations);
                }
            }
            return combinations.Distinct().ToList();
        }

        public void EliminateUnitProductions()
        {
            var newProductions = new List<Production>();

            var unitProds = new List<Production>();
            // remove unit productions
            foreach (Production production in productions)
            {
                if (!IsUnit(production))
                    newProductions.Add(production);
                else
                    unitProds.Add(production);
            }

            productions = new List<Production>(newProductions);

            foreach (Production unitProd in unitProds)
            {
                foreach (Production production in productions)
                {
                    if (production.leftSide == unitProd.rightSide)
                        newProductions.Add(new Production
                        {
                            leftSide = unitProd.leftSide,
                            rightSide = production.rightSide
                        });
                }
            }

            productions = newProductions;
        }

        bool IsUnit(Production production)
        {
            // If the right side is just one nonTermina, it will match one of
            // non terminal variables as string, this means this productions
            // is a unit one
            return nonTerminalVariables.Contains(production.rightSide);
        }

        public void EliminateInaccesibleSymbols()
        {
            var inaccessibleVars = new List<string>();

            bool accessible;
            foreach (var variable in nonTerminalVariables)
            {
                accessible = false;
                foreach (var prod in productions)
                { 
                    if (prod.rightSide.Contains(variable))
                    {
                        accessible = true;
                        break;
                    }

                }
                if (!accessible)
                    inaccessibleVars.Add(variable);
            }

            if (inaccessibleVars.Count == 0)
            {
                Console.WriteLine("No inaccessible vars found");
                return;
            }

            Console.Write("Inaccessible vars: ");
            foreach (var inaccessible in inaccessibleVars)
            {
                Console.Write(inaccessible + " ");
                for (int i = 0; i < productions.Count;)
                {
                    if (productions[i].leftSide == inaccessible)
                    {
                        productions.RemoveAt(i);
                    }
                    else
                    {
                        i++;
                    }
                }
            }
            Console.WriteLine("");
        }

        public void EliminateUnproductiveSymbols()
        {
            // Find productive symbols
            List<string> productiveSymbols = new List<string>();
            productiveSymbols.Add(startCharacter);
            bool hasChanges = true;
            while (hasChanges)
            {
                hasChanges = false;
                foreach (Production production in productions)
                {
                    if (!productiveSymbols.Contains(production.leftSide))
                    {
                        continue;
                    }
                    foreach (char c in production.rightSide)
                    {
                        if (!productiveSymbols.Contains(c.ToString()) && nonTerminalVariables.Contains(c.ToString()))
                        {
                            productiveSymbols.Add(c.ToString());
                            hasChanges = true;
                        }
                    }
                }
            }

            // Remove unproductive symbols
            List<Production> newProductions = new List<Production>();
            foreach (Production production in productions)
            {
                if (productiveSymbols.Contains(production.leftSide))
                {
                    bool isProductive = true;
                    foreach (char c in production.rightSide)
                    {
                        if (!productiveSymbols.Contains(c.ToString()) && nonTerminalVariables.Contains(c.ToString()))
                        {
                            isProductive = false;
                            break;
                        }
                    }
                    if (isProductive)
                    {
                        newProductions.Add(production);
                    }
                }
            }

            productions = GetUnique(newProductions);
            nonTerminalVariables = productiveSymbols;
        }

        static List<Production> GetUnique(List<Production> productions)
        {
            var uniqueProds = new List<Production>();
            bool added = false;
            foreach (Production production in productions)
            {
                added = false;
                foreach (var uniqueProd in uniqueProds)
                {
                    if (production.Equals(uniqueProd))
                    {
                        added = true;
                        break;
                    }
                }
                if (!added)
                    uniqueProds.Add(production);
            }
            return uniqueProds;
        }

        public void ConvertToCNF()
        {
            // Create array with letters to use instead of X1, X2, or any letter with number
            string alphabet = new string("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
            var freeLetters = new List<string>();
            foreach (var letter in alphabet)
            {
                if (!nonTerminalVariables.Contains(letter.ToString()))
                    freeLetters.Add(letter.ToString());
            }

            // Separate Vn -> Vt and others.
            // Those to have only one terminal and the others
            var nonTerminalProductions = new List<Production>();
            var terminalProductions = new List<Production>();
            var newNonTerminals = new List<string>(nonTerminalVariables);

            foreach (var production in productions)
            {
                if (production.rightSide.Length == 1 && terminalVariables.Contains(production.rightSide))
                    terminalProductions.Add(production);
                else
                    nonTerminalProductions.Add(production);
            }

            // Find In terminal productions, for every terminal letter, a terminal letter
            // that derives to it, based on terminalProductions.
            foreach (var terminalVar in terminalVariables)
            {
                string letterToReplaceWith = new string("");
                foreach (var terminalProd in terminalProductions)
                {
                    if (terminalProd.rightSide == terminalVar)
                    {
                        letterToReplaceWith = new string(terminalProd.leftSide);
                        break;
                    }
                }
                if (letterToReplaceWith.Length == 0)
                    continue;

                // After finding, replace terminal with non terminal where it meets in
                // RHS in nonTerminalProductions list
                foreach (var nonTerminalProd in nonTerminalProductions)
                {
                    nonTerminalProd.rightSide = nonTerminalProd.rightSide.Replace(terminalVar[0], letterToReplaceWith[0]);
                }
            }

            // Now we have nonterminal to one terminal and nonterminal to multiple nonterminals
            // those to derive to more than two nonterminals will be splited

            var newProductions = new List<Production>();
            foreach (var terminalProd in terminalProductions)
                newProductions.Add(terminalProd);
            foreach (var nonTerminalProd in nonTerminalProductions)
                newProductions.Add(nonTerminalProd);

            foreach (var nonTerminalProd in nonTerminalProductions)
            {
                while (nonTerminalProd.rightSide.Length > 2)
                {
                    // Create new production and shorten the current one
                    var newLetter = new string(freeLetters[0].ToString());
                    freeLetters.RemoveAt(0);
                    newProductions.Add(new Production
                    {
                        leftSide = newLetter,
                        rightSide = nonTerminalProd.rightSide.Substring(0, 2)
                    });
                    nonTerminalProd.rightSide = newLetter + nonTerminalProd.rightSide.Substring(2);
                }
            }

            productions = newProductions;
        }

    }
}
