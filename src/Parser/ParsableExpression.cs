using System.Text.Json.Nodes;

namespace LFAF_LABORATORY.Parser;

public class ParsableExpression : IParsable
{
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

    static JsonObject ParseOperand(IteratorWrapper<Token> tokensIterator)
    {
        JsonObject operandAst = new()
        {
            ["Type"] = "OPERAND"
        };

        if (tokensIterator.Current.Type == TokenType.Number)
        {
            operandAst["value"] = new JsonObject{
                { "type", "Number"},
                { "value", tokensIterator.Current.Value }
            };
        }
        else if (tokensIterator.Current.Type == TokenType.Symbol)
        {
            operandAst["value"] = new JsonObject{
                { "type", "Symbol"},
                { "value", tokensIterator.Current.Value }
            };
        }
        else
            throw new Exception("Number or Symbol expected");

        return operandAst;
    }

    static JsonObject ParseCmpOperator(IteratorWrapper<Token> tokensIterator)
    {
        JsonObject operatorAst = new()
        {
            ["Type"] = "OPERATOR"
        };

        if (tokensIterator.Current.Type == TokenType.EqualOperator)
        {
            operatorAst["value"] = new JsonObject{
                { "type", "EqualOperator"},
                { "value", tokensIterator.Current.Value }
            };
        }
        else if (tokensIterator.Current.Type == TokenType.NotEqualOperator)
        {
            operatorAst["value"] = new JsonObject{
                { "type", "NotEqualOperator"},
                { "value", tokensIterator.Current.Value }
            };
        }
        else
            throw new Exception("Compare operator expected");

        return operatorAst;
    }
}
