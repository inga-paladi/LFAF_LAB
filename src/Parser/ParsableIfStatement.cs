using System.Text.Json;
using System.Text.Json.Nodes;

namespace LFAF_LABORATORY.Parser;

public class ParsableIfStatement : IParsable
{
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

    private static void Expect(IteratorWrapper<Token> iterator, TokenType type)
    {
        if (iterator.Current == null || iterator.Current.Type != type)
            throw new Exception("Expected " +  type.ToString());
    }
}
