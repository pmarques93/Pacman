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
        /// Reads lives from lives txt file
        /// </summary>
        /// <returns>Returns a byte with lives</returns>
        public byte ReadLives()
        {
            byte lives = 0;

            using (StreamReader sr = File.OpenText(path))
            {
                byte.TryParse(sr.ReadLine(), out byte result);
                lives = result;
            }

            return lives;
        }
    }
}
