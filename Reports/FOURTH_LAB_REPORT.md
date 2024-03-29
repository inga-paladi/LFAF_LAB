# Topic: Chomsky Normal Form

### Course: Formal Languages & Finite Automata

### Author: Paladi Inga

---

## Theory

A grammar is in Chomsky Normal Form if all its production rules are of the form:

`A → BC or A → a`
meaning that there are either two non-terminal symbols or one terminal symbol.

To obtain grammar in CNF we have to follow these 5 steps:

1. Eliminate ε productions.
2. Eliminate unit productions.
3. Eliminate inaccessible symbols.
4. Eliminate the non productive symbols.
5. Obtain the Chomsky Normal Form.

## Objectives:

1. Learn about Chomsky Normal Form (CNF).
2. Get familiar with the approaches of normalizing a grammar.
3. Implement a method for normalizing an input grammar by the rules of CNF.
   1. The implementation needs to be encapsulated in a method with an appropriate signature (also ideally in an appropriate class/type).
   2. The implemented functionality needs executed and tested.
   3. A BONUS point will be given for the student who will have unit tests that validate the functionality of the project.
   4. Also, another BONUS point would be given if the student will make the aforementioned function to accept any grammar, not only the one from the student's variant.

## Implementation description
The first step is ELiminate  ε productions, where the method EliminateEProductions() starts by finding nullable
symbols. A nullable symbol is a non-terminal variable that can derive the empty string. 
The method uses a while loop to iterate over the non-terminal variables and productions to find all nullable symbols.
For each symbol, the method checks if it is already in the list of nullable symbols. If it is, the method skips
to the next symbol. If it isn't, the method iterates over all productions to find
productions that contain the symbol in their right-hand side.
```
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
```
After eliminating epsilon production we have to add some new productions. Therefore
the method  GetCombinations that takes a string s and a list of nullable symbols as 
input and returns a list of all possible combinations of s after removing any
occurrences of nullable symbols.
```
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
```
Step 2. Eliminating Unit Production
The EliminateUnitProductions() method creates a new list and iterates through each
production, separating unit and non-unit productions. It then replaces unit productions
with the productions that match their right side. The IsUnit() method checks if a 
production is a unit production by looking for a single non-terminal variable on its right side.
```
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
```
Step 3. Eliminate Inaccesible Symbols
The EliminateInaccesibleSymbols() method identifies and removes any inaccessible symbols
in a given set of productions. It first creates a list to store the inaccessible 
symbols. Then, for each non-terminal variable in the set, it checks if the variable
is accessible by looking for it in the right side of any production. If the variable
is not found in any production, it is considered inaccessible and added to the list.
If there are no inaccessible symbols, the method prints a message and returns.
Otherwise, the method prints the list of inaccessible symbols and removes any
productions that have an inaccessible left side.
```
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
```
Step 4. Eliminate Unproductive Symbols
The EliminateUnproductiveSymbols() method first finds all productive symbols by
initializing a list with the start character and iteratively adding symbols that
appear on the right-hand side of productions whose left-hand side is already in the 
productive symbols list.
Next, it creates a new list of productions that only includes productions 
whose left-hand side is in the productive symbols list and whose right-hand side
contains only symbols that are either in the productive symbols list or are terminal
symbols.
The GetUnique() method is a helper method that takes a list of productions and 
returns a list of unique productions (i.e., without duplicates).
Finally, the method updates the productions list to the new list of productions
and updates the non-terminal variables list to be the list of productive symbols.
```
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
```
Step 5. Converting to CNF
The ConvertToCNF() method is a part of a class that implements a context-free grammar. 
It converts the grammar to Chomsky normal form (CNF), which is a form where all
productions are in one of two forms: either a non-terminal symbol followed by two
non-terminal symbols, or a non-terminal symbol followed by a single terminal symbol.
The method does this by replacing long productions with new non-terminal symbols and 
splitting up any terminal productions that contain more than one symbol.
```
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

```
## Conclusions / Screenshots / Results
This laboratory work helped me to gain knowledge on how to convert a Context-Free
Grammar into Chomsky Normal Form. I was able to implement distinct 
functions for each step of the process and examine the intermediate outcomes.
After running the project I got the following results:
```
Eliminate epsilon productions
S -> aB
S -> a
S -> A
A -> B
A -> SA
A -> S
A -> bBA
A -> bA
A -> bB
A -> b
A -> b
B -> b
B -> bS
B -> b
B -> aD
B -> a
C -> Ba
C -> a
D -> AA
Eliminate unit productions
S -> aB
S -> a
A -> SA
A -> bBA
A -> bA
A -> bB
A -> b
A -> b
B -> b
B -> bS
B -> b
B -> aD
B -> a
C -> Ba
C -> a
D -> AA
S -> SA
S -> bBA
S -> bA
S -> bB
S -> b
S -> b
A -> b
A -> bS
A -> b
A -> aD
A -> a
A -> aB
A -> a
Eliminate inaccesible symbols
Inaccessible vars: C
S -> aB
S -> a
A -> SA
A -> bBA
A -> bA
A -> bB
A -> b
A -> b
B -> b
B -> bS
B -> b
B -> aD
B -> a
D -> AA
S -> SA
S -> bBA
S -> bA
S -> bB
S -> b
S -> b
A -> b
A -> bS
A -> b
A -> aD
A -> a
A -> aB
A -> a
Eliminate unproductive productions
S -> aB
S -> a
A -> SA
A -> bBA
A -> bA
A -> bB
A -> b
B -> b
B -> bS
B -> aD
B -> a
D -> AA
S -> SA
S -> bBA
S -> bA
S -> bB
S -> b
A -> bS
A -> aD
A -> a
A -> aB
Convert to CNF
S -> a
A -> b
B -> b 
B -> a
S -> b
A -> a
S -> SB
A -> SA
A -> CA
A -> AA
A -> AB
B -> AS
B -> SD
D -> AA
S -> SA
S -> EA
S -> AA
S -> AB
A -> AS
A -> SD
A -> SB
C -> AB
E -> AB
```
