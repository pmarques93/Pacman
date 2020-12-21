using System;

namespace Pacman
{
    class Program
    {
        static void Main(string[] args)
        {
            DoubleBuffer<char> df = new DoubleBuffer<char>(10, 10);

            df[3, 3] = '<';

            df.Swap();

            Console.WriteLine(df[3, 3]);

            Console.WriteLine(df.X);
        }
    }
}
