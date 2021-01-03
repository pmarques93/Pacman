using Pacman.Collisions;

namespace Pacman.GameRelated
{
    /// <summary>
    /// Class for ConsoleScore. Implements IGameObject.
    /// </summary>
    public class Score : IGameObject
    {
        /// <summary>
        /// Gets this class's name.
        /// </summary>
        public string Name => "Score";

        private readonly Collision collisions;

        private uint score;

        /// <summary>
        /// Gets string with current score.
        /// </summary>
        public string GetScore { get => score.ToString(); }

        /// <summary>
        /// Constructor for Score.
        /// </summary>
        /// <param name="collision">Collision parameter.</param>
        public Score(Collision collision)
        {
            collisions = collision;
            score = 0;
        }

        /// <summary>
        /// Method that runs once on start.
        /// </summary>
        public void Start() =>
            collisions.ScoreCollision += UpdateScore;

        /// <summary>
        /// Method that defines what happens while the class is running.
        /// </summary>
        public void Update()
        {
            // Method intentionally left empty.
        }

        /// <summary>
        /// Method that runs once on finish.
        /// </summary>
        public void Finish() =>
            collisions.ScoreCollision -= UpdateScore;

        /// <summary>
        /// Updates score everytime pacman eats a food.
        /// </summary>
        /// <param name="score">Update score with this value.</param>
        private void UpdateScore(ushort score)
            => this.score += score;
    }
}
