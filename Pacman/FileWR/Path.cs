using System.IO;

namespace Pacman
{
    /// <summary>
    /// Struct with file paths
    /// </summary>
    public class Path
    {
        private static string dirPath = Directory.GetCurrentDirectory();

        public static readonly string highscore = dirPath + "/highscore.txt";
    }
}
