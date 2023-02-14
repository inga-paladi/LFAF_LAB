# Intro to formal languages. Regular grammars. Finite Automata

### Course: Formal Languages & Finite Automata
### Author: Paladi Inga

## Theory

The components of a language are:
  -alphabet: set of symbols;
  -vocabulary: set of strings;
  -grammar: Set of rules for a language.
  
A grammar G is an order of four elements, VN, VT, P, S where:
  VN - is a finite set of non-terminal symbols;
  VT - is a finite set of terminal symbols;
  S - Start symbol;
  P - a set of productions of rules.



## Objectives:
* Understand what a language is and what it needs to have in order to be considered a formal one.

* Provide the initial setup for the evolving project that you will work on during this semester. I said project because usually at lab works, I encourage/impose students to treat all the labs like stages of development of a whole project. Basically you need to do the following:

    a. Create a local && remote repository of a VCS hosting service (let us all use Github to avoid unnecessary headaches);

    b. Choose a programming language, and my suggestion would be to choose one that supports all the main paradigms;

    c. Create a separate folder where you will be keeping the report. This semester I wish I won't see reports alongside source code files, fingers crossed;

* According to your variant number (by universal convention it is register ID), get the grammar definition and do the following tasks:

    a. Implement a type/class for your grammar;

    b. Add one function that would generate 5 valid strings from the language expressed by your given grammar;

    c. Implement some functionality that would convert and object of type Grammar to one of type Finite Automaton;
    
    d. For the Finite Automaton, please add a method that checks if an input string can be obtained via the state transition from it;




## Implementation description
 In the following snap of code, I have set the VN, VT, P, and S values according to variant 20. 
 For the terminal and non terminal variable is used a char array to store and initialize the information.
```
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
```
To get a word for our grammar, first we must start with 'S', therefore we will check the first word, if our word has a terminal value our function will end
and we will get our word, if is not we will get forword witt the productions and if we have 2 productions rules for a nonTerminal value, we will choose
one, randomly.
```
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
```
To find if a string word is valid, we will initialize a foreach loop, which will take each letter and will search in transitions, for the rule, according
to the letter
```
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
```
## Conclusions / Screenshots / Results


## References



















