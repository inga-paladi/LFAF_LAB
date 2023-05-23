using System.Text.Json;
using System.Text.Json.Nodes;

namespace LFAF_LABORATORY.Parser;

/*
 * Grammar
 * 
 * PROGRAM
 *  : CODE_BLOCK
 *  ;
 * 
 * CODE_BLOCK
 *  : OpenBracket CODE_BLOCK CloseBracket
 *  | OpenBracket CloseBracket
 *  | IF_STATEMENT
 *  ;
 *  
 * IF_STATEMENT
 *  : IfKeyword OpenParenthesis EXPRESSION CloseParenthesis CODE_BLOCK
 *  ;
 *  
 * EXPRESSION
 *  : OPERAND CMP_OPERATOR OPERAND
 *  | OPERAND
 *  ;
 *  
 *  CMP_OPERATOR
 *   : EqualOperator
 *   | NotEqualOperator
 *   ;
 *   
 *  OPERAND
 *   : Number
 *   | Symbol
 *   ;
 * 
 */


public class Parser
{
    private List<Token> m_tokens;

    public Parser()
    {
        m_tokens = new List<Token>();
    }

    public void Init(List<Token> tokens)
    {
        m_tokens = new List<Token>(tokens);
    }

    public string Parse()
    {
        var parsableCodeBlock = new ParsableCodeBlock();
        var iterator = new IteratorWrapper<Token>(m_tokens.GetEnumerator());
        // Go to first valid element, if it exists
        iterator.MoveNext();

        JsonObject ast = new()
        {
            ["type"] = "Program",
            ["body"] = parsableCodeBlock.Parse(iterator)
        };

        return JsonSerializer.Serialize(ast);
    }
}
