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
            
            var expr = ParserUtilities.ParseExpressionString("{{s a true}} {{s b true}} {{s c asdf}} {ifeq a b}a = b{/} {ifneq a c}a != c{/}");
            Console.WriteLine(sequence.ExecuteExpression(expr));
        }
    }
}
