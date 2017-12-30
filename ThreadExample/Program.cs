using System;
using Thread;
using Thread.Parser;

namespace ThreadExample
{
    class Program
    {
        static void Main(string[] args)
        {
            var sequence = new Sequence();
            sequence.RegisterStandardLibrary();

            var expr = ParserUtilities.ParseExpressionString("   hello, {if_true human}human{/} {{set human true}} oh yeah #blah blah blah");
            Console.WriteLine(sequence.ExecuteExpression(expr));
        }
    }
}
