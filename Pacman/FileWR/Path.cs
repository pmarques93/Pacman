using System;
using System.IO;

namespace Pacman
{
    /// <summary>
    /// Struct with file paths
    /// </summary>
    public class Path
    {
        private static string temp = Directory.GetCurrentDirectory();

        public static readonly string lives = temp + "/lives.txt";
    }
}
