using System.IO;

namespace Pacman
{
    /// <summary>
    /// Class for FileWriter. Writes files
    /// </summary>
    public class FileWriter
    {
        private readonly string path;

        /// <summary>
        /// Constructor for FileWriter
        /// </summary>
        /// <param name="path">Path to write to</param>
        public FileWriter(string path)
        {
            this.path = path;
        }

        /// <summary>
        /// Creates a text file with pacman highscore
        /// </summary>
        /// <param name="highscore"></param>
        public void CreateHighScoreTXT(uint highscore)
        {
            using (StreamWriter sw = File.CreateText(path))
            {
                sw.WriteLine($"{highscore}");
            }
        }
    }
}
