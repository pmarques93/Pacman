using System;

namespace Pacman
{
    /// <summary>
    /// Class for ConsoleScore. Implements IGameObject
    /// </summary>
    class ConsoleScore : IGameObject
    {
        /// <summary>
        /// Property with this class's name
        /// </summary>
        public string Name => "Score";

        private readonly Collision collisions;

        private ushort score;

        /// <summary>
        /// Constructor for ConsoleScore
        /// </summary>
        /// <param name="collision">Collision parameter</param>
        public ConsoleScore(Collision collision)
        {
            collisions = collision;
            score = 0;
        }

        /// <summary>
        /// Method that runs once on start
        /// </summary>
        public void Start() =>
            collisions.ScoreCollision += UpdateScore;

        /// <summary>
        /// Method that runs once on finish
        /// </summary>
        public void Finish() =>
            collisions.ScoreCollision -= UpdateScore;

        /// <summary>
        /// Method responsible for what happens when the GameObject is running
        /// </summary>
        public void Update()
        {
            Console.WriteLine($"Score: {score}");
        }

        /// <summary>
        /// Updates score everytime pacman eats a food
        /// </summary>
        /// <param name="score"></param>
        private void UpdateScore (ushort score)
            => this.score += score;
    }
}
