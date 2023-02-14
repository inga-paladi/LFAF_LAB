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
To find if a string word is valid, we will initialize a foreach loop, which will take each letter and will search in transitions, for the rule, according to the letter, if a transition is found, then we move the new state it leads to
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
In this laboratory work, I managed to create with the grammar valid words that respect the productions. that is more easy to generate a word 
with a grammar, than doing it on papper, that make take time.
From productions, I changed in transitions, to get the finite automata from regular grammar, to get all the possible states, and a final state, we have to add a new nonTerminal variable, because the production (A → d ) does not have a final state.
![image](https://user-images.githubusercontent.com/89548044/218764040-63a463b4-99da-4f9f-8421-82b5290c6e78.png)
Above is a schema that I have used to get the transitions.

Here is our output, were our program generated 5 words, and checked  if the following word is valid (finiteAutomaton.wordIsValid("daddS"))
![output](https://user-images.githubusercontent.com/89548044/218758626-a27bd2c6-ed7a-4551-8823-8e5ff4b57351.png)

## References
https://stackoverflow.com/questions/38605404/how-to-find-value-in-array-based-conditional
https://learn.microsoft.com/en-us/dotnet/api/system.array.find?view=net-6.0
https://codelikeadev.com/blog/get-last-character-of-string-c-sharp#:~:text=v-,Get%20Last%20Character%20Of%20A%20String%20Using%20the%20.,last%20character%20of%20the%20string.

















