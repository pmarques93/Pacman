using System.IO;

namespace Pacman.Components
{
    /// <summary>
    /// Class responsible for updating the high score. Extends ecomponent.
    /// </summary>
    public class HighScoreComponent : Component
    {
        /// <summary>
        /// Gets highScore.
        /// </summary>
        public uint HighScore { get; private set; }

        /// <summary>
        /// Method thatr runs once on start.
        /// Reads a file with high score. If the file doesn't exist,
        /// the high score is 0.
        /// </summary>
        public override void Start()
        {
            // If file doesn't exist, highscore is 0
            uint highScore = 0;

            if (File.Exists(Path.highscore))
            {
                FileReader fileReader = new FileReader(Path.highscore);
                highScore = fileReader.ReadHighScore();
            }

            HighScore = highScore;
        }
    }
}
