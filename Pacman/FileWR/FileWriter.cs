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
        /// Creates a text file with pacman lives
        /// </summary>
        /// <param name="lives"></param>
        public void CreateLivesText(byte lives)
        {
            using (StreamWriter sw = File.CreateText(path))
            {
                sw.WriteLine($"{lives}");
            }
        }
    }
}
