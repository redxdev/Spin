using System;
using Thread;
using Thread.Parser;

namespace ThreadExample
{
    class Program
    {
        static void Main(string[] args)
        {
            var sequence = new Sequence(new DictionaryBackend());
            sequence.RegisterStandardLibrary();
            
            var expr = ParserUtilities.ParseExpressionString("Hello {{v test}} world! My{{e \"   \"}}   {{v test}}  name is sam.");
            Console.WriteLine(sequence.ExecuteExpression(expr));
        }
    }
}
