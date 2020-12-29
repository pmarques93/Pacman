using System.IO;

namespace Pacman
{
    /// <summary>
    /// Class for FileReader. Reads files
    /// </summary>
    class FileReader
    {
        private readonly string path;

        /// <summary>
        /// Constructor for FileReader
        /// </summary>
        /// <param name="path">Receives a string to read the file from</param>
        public FileReader(string path)
        {
            this.path = path;
        }

        /// <summary>
        /// Reads lives from highscore txt file
        /// </summary>
        /// <returns>Returns an uint with highscore</returns>
        public uint ReadHighScore()
        {
            uint highscore = 0;

            using (StreamReader sr = File.OpenText(path))
            {
                uint.TryParse(sr.ReadLine(), out uint result);
                highscore = result;
            }

            return highscore;
        }
    }
}
