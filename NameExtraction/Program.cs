using System;

namespace NameExtraction
{
    class Program
    {
        static NameExtraction test = new NameExtraction();
        static void Main()
        {
            Console.WriteLine("input");
            string input = Console.ReadLine();
            test.GetPath(input);
            Console.WriteLine("Complete");
        }
    }
}
