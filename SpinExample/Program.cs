using System;
using Spin;
using Spin.Parser;

namespace SpinExample
{
    class Program
    {
        static void Main(string[] args)
        {
            var sequence = new Sequence(new DictionaryBackend(), new FileDocumentLoader());
            sequence.RegisterStandardLibrary();

            try
            {
                sequence.LoadAndStartDocument("sequences/example.spd");
                while (sequence.StartNextLine().HasValue)
                {
                    Console.WriteLine("---");

                    var text = sequence.ExecuteCurrentLine().BuildString();
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
