using System.Text.Json;
using System.Text.Json.Nodes;

namespace LFAF_LABORATORY.Parser;

public class ParsableCodeBlock : IParsable
{
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
}
