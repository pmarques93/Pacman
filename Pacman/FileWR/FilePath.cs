using System.IO;

namespace Pacman
{
    /// <summary>
    /// Struct with file paths.
    /// </summary>
    public struct FilePath
    {
        private static readonly string DirPath = Directory.GetCurrentDirectory();

        /// <summary>
        /// Highscore path.
        /// </summary>
        public static readonly string Highscore =
            DirPath + $"{Path.DirectorySeparatorChar}highscore.txt";

    }
}
