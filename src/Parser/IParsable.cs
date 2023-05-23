using System.Text.Json.Nodes;

namespace LFAF_LABORATORY.Parser;

public interface IParsable
{
    public JsonObject Parse(IteratorWrapper<Token> tokensIterator);
}
