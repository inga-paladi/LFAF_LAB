namespace LFAF_LABORATORY;
using System;
using System.Text.RegularExpressions;

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
    static readonly string startOfLineRegex = @"^";

    static readonly public Dictionary<TokenType, string> tokenTypeRegex = new()
    {
        { TokenType.OpenBracket, @"\{" },
        { TokenType.CloseBracket, @"\}" },
        { TokenType.OpenParenthesis, @"\("},
        { TokenType.CloseParenthesis, @"\)"},
        { TokenType.IfKeyword, "if"},
        { TokenType.EqualOperator, @"=="},
        { TokenType.NotEqualOperator, @"\!="},
        { TokenType.Number, @"\d+"},
        { TokenType.Symbol, @"[a-zA-Z_-][a-zA-Z_\d-]+"}
    };

    static public List<Token> Tokenize(string input)
    {
        var tokens = new List<Token>();

        while (!string.IsNullOrEmpty(input))
        {
            if (input[0] == ' ')
            {
                input = input[1..];
                continue;
            }

            bool matchFound = false;

            foreach (var tokenRegexPair in tokenTypeRegex)
            {
                var regex = new Regex(startOfLineRegex + tokenRegexPair.Value);
                var match = regex.Match(input);
                if (match.Success)
                {
                    matchFound = true;
                    tokens.Add(new Token(tokenRegexPair.Key, match.Value));
                    input = input[match.Length..];
                    break;
                }
            }

            if (!matchFound)
            {
                throw new Exception("Pattern not found at: " +  input);
            }
        }

        return tokens;
    }
}