namespace LFAF_LAB_3;
using System;

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

public class Token
{
    public TokenType Type { get; }
    public string Value { get; }

    public Token(TokenType type, string value)
    {
        Type = type;
        Value = value;
    }
}

public class Lexer
{
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
            else if (input[pos] == '}')
            {
                tokens.Add(new Token(TokenType.CloseBracket, "}"));
                pos++;
            }
            else if (input[pos] == '=' && pos + 1 < input.Length && input[pos + 1] == '=')
            {
                tokens.Add(new Token(TokenType.EqualOperator, "=="));
                pos += 2;
            }
            else if (input[pos] == '!' && pos + 1 < input.Length && input[pos + 1] == '=')
            {
                tokens.Add(new Token(TokenType.NotEqualOperator, "!="));
                pos += 2;
            }
            else if (char.IsDigit(input[pos]))
            {
                // Parse number
                var start = pos;
                while (pos < input.Length && char.IsDigit(input[pos]))
                {
                    pos++;
                }
                var length = pos - start;
                var value = input.Substring(start, length);
                tokens.Add(new Token(TokenType.Number, value));
            }
            else if (char.IsLetter(input[pos]))
            {
                // Parse keyword or symbol
                var start = pos;
                while (pos < input.Length && (char.IsLetterOrDigit(input[pos]) || input[pos] == '_'))
                {
                    pos++;
                }
                var length = pos - start;
                var value = input.Substring(start, length);
                if (value == "if")
                {
                    tokens.Add(new Token(TokenType.IfKeyword, "if"));
                }
                else
                {
                    tokens.Add(new Token(TokenType.Symbol, value));
                }
            }
            else
            {
                // Unexpected character
                throw new ArgumentException( $"Unexpected character: {input[pos]}");
            }
        }
        return tokens;
    }
}

