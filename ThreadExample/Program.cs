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
            
            var expr = ParserUtilities.ParseExpressionString("{h} {{s name, Sam}}    asdfasdf {{s mood, cool}}{/}Hello, {{v name}}! You look {{v mood}}.");
            Console.WriteLine(sequence.ExecuteExpression(expr));
        }
    }
}
