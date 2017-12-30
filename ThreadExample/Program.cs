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
                while (sequence.StartNextLine().HasValue)
                {
                    Console.WriteLine("---");

                    var text = sequence.ExecuteCurrentLine();
                    Console.WriteLine(text);
                    Console.WriteLine("Press enter to continue...");
                    Console.ReadLine();
                }

                Console.WriteLine("---");
                Console.WriteLine("Sequence ended.");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error:");
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("Press enter to continue...");
            Console.ReadLine();
        }
    }
}
