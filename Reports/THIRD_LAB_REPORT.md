# Lexer & Scanner

### Course: Formal Languages & Finite Automata
### Author: Inga Paladi

----

## Theory
A lexer is a program that separates a given string into smaller parts called tokens. It does this by examining
each character in the string and determining the meaning of the group of characters that it forms. A token is a
group of characters that represents a single unit in the syntax of the language being analyzed. Examples of tokens
include keywords, identifiers, operators, literals, and punctuation marks.


## Objectives:
1. Understand what lexical analysis is.
2. Get familiar with the inner workings of a lexer/scanner/tokenizer.
3. Implement a sample lexer and show how it works.


## Implementation description
While analyzing the input text we encounter constants that represent a different type of tokens, that can
be encountered by a lexer while analyzing input text.

```
public enum TokenType
{
    OpenBracket,
    CloseBracket,
    OpenParenthesis,
    CloseParenthesis,
    IfKeyword,
    EqualOperator,
    NotEqualOperator,
    Number,
    Symbol
}
```
The Tokenize method scans the input string character by character, identifying the
meaning of each group of characters and adding a corresponding Token object to the
list. The method handles various types of characters, such as whitespace,
punctuation marks, operators, numbers, and keywords, and raises an exception if it
encounters an unexpected character.
```
public List<Token> Tokenize(string input)
    {
        var tokens = new List<Token>();
            var pos = 0;
            while (pos < input.Length)
            {
                if (char.IsWhiteSpace(input[pos]))
                {
                    // Skip whitespace
                    pos++;
                }
                else if (input[pos] == '(')
                {
                    tokens.Add(new Token(TokenType.OpenParenthesis, "("));
                    pos++;
                }
                else if (input[pos] == ')')
                {
                    tokens.Add(new Token(TokenType.CloseParenthesis, ")"));
                    pos++;
                }
                else if (input[pos] == '{')
                {
                    tokens.Add(new Token(TokenType.OpenBracket, "{"));
                    pos++;
                }
```
If we have an input that those not belong to our token types then that character will be
considered as unexpected character
```
else
{
    // Unexpected character
    throw new ArgumentException( $"Unexpected character: {input[pos]}");
}
```
In the main class we are creating a lexer object, initializing an input string, and tokenizing the input string using the lexer object.
```
 var lexer = new Lexer();
        string input = "if (variabila == 1 ){} ";
        List<Token> tokens = lexer.Tokenize(input);

        foreach (var token in tokens)
        {
            Console.WriteLine("Value: {0}, type: {1}", token.Value, token.Type) ;
        }
```
## Conclusions / Screenshots / Results
The input string that had to be analyzed is: if (variabila == 1 ){}
As result we received 
<p align="center">
  <img src="/home/moonbye/Pictures/Screenshots/Screenshot from 2023-03-30 14-22-27.png" width="350" title="Results">
</p>
In this laboratory work I got a deeper understanding of what a Lexer is, how to implement it. How to deal with the arithmetic characters, and the one that are unexpected characters.