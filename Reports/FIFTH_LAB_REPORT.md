# Topic: Parser & Building an Abstract Syntax Tree

### Course: Formal Languages & Finite Automata

### Author: Paladi Inga

---

## Theory
Parsing is the analysis of code to determine its structure, while an Abstract Syntax Tree (AST) is a hierarchical representation of that structure. The parser checks code against language grammar rules, creating an AST that captures essential elements without unnecessary  details. The AST uses nodes to represent language constructs and their relationships. It is commonly used in compilers and interpreters for operations like optimization and code generation.

## Objectives:
1. Get familiar with parsing, what it is and how it can be programmed [1].
2. Get familiar with the concept of AST [2].
3. In addition to what has been done in the 3rd lab work do the following:
   1. In case you didn't have a type that denotes the possible types of tokens you need to:
      1. Have a type __*TokenType*__ (like an enum) that can be used in the lexical analysis to categorize the tokens. 
      2. Please use regular expressions to identify the type of the token.
   2. Implement the necessary data structures for an AST that could be used for the text you have processed in the 3rd lab work.
   3. Implement a simple parser program that could extract the syntactic information from the input text.

## Implementation description

As required by the task, in lexer was added token find using regex, now the code is considerably reduced by the new modifications, it is easier to read and to mantain.
The `TokenType` enum was already present from lab 3.

---

The parsing follows the rules by the grammar presented below

```
PROGRAM
 : CODE_BLOCK
 ;

CODE_BLOCK
 : OpenBracket CODE_BLOCK CloseBracket
 | OpenBracket CloseBracket
 | IF_STATEMENT
 ;
 
IF_STATEMENT
 : IfKeyword OpenParenthesis EXPRESSION CloseParenthesis CODE_BLOCK
 ;
 
EXPRESSION
 : OPERAND CMP_OPERATOR OPERAND
 | OPERAND
 ;
 
 CMP_OPERATOR
  : EqualOperator
  | NotEqualOperator
  ;
  
 OPERAND
  : Number
  | Symbol
  ;
```

Keywords, like: Number, OpenBracket or EqualOperator are tokens defined in Lab 3's lexer

For the parsing process of special segments, like: IF_STATEMENT or EXPRESSION, custom classes are implemented from interface IParsable, which provides one method for implementation, which is Parse, which receives an iterator to token list and returns its own abstract syntax tree for the part it parses, which is in JSON format.

The whole parsing process is started in Parse class, which starts with the first rule

```
PROGRAM
 : CODE_BLOCK
 ;
```

Based on the grammar, the program contains a code block, which can be parsed by `ParsableCodeBlock` class, which checks further if the token list matches the grammar of the `CODE_BLOCK`, which should contain another code block inside brackets, or an `IF_STATEMENT`.
If it encoounters an if statement, it creates an `ParsableIfStatement` class instance whom passes the parsing process for the if statement

If the parsing process for all the `IParsable` implemented classes are done based on the grammar, the final `AST` is displayed to the user, if a character is expected based on the grammer rules, an error message is displayed and the parsing process stops.

New is the code for `ParsableCodeBlock`

```
public JsonObject Parse(IteratorWrapper<Token> tokensIterator)
{
    if (tokensIterator.Current == null)
        return new JsonObject();
        
    if (tokensIterator.Current.Type == TokenType.OpenBracket)
    {
        tokensIterator.MoveNext();
        if (tokensIterator.Current.Type == TokenType.CloseBracket)
            return new JsonObject();

        JsonObject codeBlockAst = new();
        ParsableCodeBlock parsableCodeBlock = new();
        codeBlockAst["type"] = "CodeBlock";
        codeBlockAst["body"] = parsableCodeBlock.Parse(tokensIterator);
        return codeBlockAst;
    }
    else
    {
        ParsableIfStatement parsableIfStatement = new();
        return parsableIfStatement.Parse(tokensIterator);
    }
}
```

It checks for tokens, like: open brackets or start of if statement. If the current token is not an open bracket, it is considered automaticaly an if statement and the responsibility for parsing is send further to `ParsableIfStatement` class which is defined bellow

```
public JsonObject Parse(IteratorWrapper<Token> tokensIterator)
{
    Expect(tokensIterator, TokenType.IfKeyword);
    tokensIterator.MoveNext();
    Expect(tokensIterator, TokenType.OpenParenthesis);
    tokensIterator.MoveNext();

    JsonObject ifStatementAst = new()
    {
        ["type"] = "IF_STATEMENT"
    };

    ParsableExpression parsableExpression = new();
    ifStatementAst["condition"] = parsableExpression.Parse(tokensIterator);

    Expect(tokensIterator, TokenType.CloseParenthesis);
    tokensIterator.MoveNext();

    ParsableCodeBlock parsableCodeBlock = new();
    ifStatementAst["block"] = parsableCodeBlock.Parse(tokensIterator);

    return ifStatementAst;
}
```

if the current token is not as expected, an error is thrown

```
private static void Expect(IteratorWrapper<Token> iterator, TokenType type)
{
    if (iterator.Current == null || iterator.Current.Type != type)
        throw new Exception("Expected " +  type.ToString());
}
```

the responsibility for expressions is passed to ParsableExpression, it checks for operands and operators

```
public JsonObject Parse(IteratorWrapper<Token> tokensIterator)
{
    JsonObject expressionAst = new()
    {
        ["type"] = "EXPRESSION"
    };

    var operand1 = ParseOperand(tokensIterator);
        
    tokensIterator.MoveNext();

    if (tokensIterator.Current.Type == TokenType.CloseParenthesis)
    {
        expressionAst["value"] = operand1;
        return expressionAst;
    }

    expressionAst["operand1"] = operand1;
    expressionAst["cmp_operator"] = ParseCmpOperator(tokensIterator);
    tokensIterator.MoveNext();

    expressionAst["operand2"] = ParseOperand(tokensIterator);
    tokensIterator.MoveNext();

    return expressionAst;
}
```

## Conclusions / Screenshots / Results

In the final lab, I learned how to implement a Parser using a lexer and tokens from the previous lab. I now understand the role of an Abstract Syntax Tree (AST) and successfully incorporated it into my code. By utilizing the existing TokenType from the Lexer class, I was able to streamline the Parser implementation. The collaboration between the lexer, parser, and AST resulted in accurate output with minimal errors. Working on all five labs was enjoyable, and I achieved a well-functioning codebase with a structured repository. Overall, this lab provided a valuable experience in Parser implementation, making the entire journey fulfilling and enjoyable.


Following are some examples of inputs and its output:

Input 1:

```
if (variabila == 1){}
```

Output 1:

```
{"type":"Program","body":{"type":"IF_STATEMENT","condition":{"type":"EXPRESSION","operand1":{"Type":"OPERAND","value":{"type":"Symbol","value":"variabila"}},"cmp_operator":{"Type":"OPERATOR","value":{"type":"EqualOperator","value":"=="}},"operand2":{"Type":"OPERAND","value":{"type":"Number","value":"1"}}},"block":{}}}
```

---

Input 2:

```
if (1) {}
```

Output 2: 

```
{"type":"Program","body":{"type":"IF_STATEMENT","condition":{"type":"EXPRESSION","value":{"Type":"OPERAND","value":{"type":"Number","value":"1"}}},"block":{}}}
```

---

Input 3:

```
{{}}
```

Output 3:

```
{"type":"Program","body":{"type":"CodeBlock","body":{}}}
```

---

Input 4:

```
if (}
```

Output 4:

```
Number or Symbol expected
```