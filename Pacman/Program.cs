using System;

namespace Pacman
{
    class Program
    {
        static void Main(string[] args)
        {
            // DB test
            DoubleBuffer<char> df = new DoubleBuffer<char>(10, 10);
            df[3, 3] = '<';
            df.Swap();
            Console.WriteLine(df[3, 3]);

            // Pacman test
            Scene scene = new Scene(10, 10);
            scene.SetupScene();
            scene.GameLoop(300);
        }
    }
}
