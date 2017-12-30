using System;
using Thread;
using Thread.Parser;

namespace ThreadExample
{
    class Program
    {
        static void Main(string[] args)
        {
            var sequence = new Sequence(new DictionaryBackend(), new FileDocumentLoader());
            sequence.RegisterStandardLibrary();

            try
            {
                sequence.LoadAndStartDocument("sequences/example.ths");
                sequence.StartNextLine();
                while (sequence.CurrentLine != null)
                {
                    Console.WriteLine("---");

                    var text = sequence.ExecuteCurrentLine();
                    Console.WriteLine(text);

                    Console.ReadKey();
                    sequence.StartNextLine();
                }

                Console.WriteLine("---");
                Console.WriteLine("Sequence ended.");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error:");
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
