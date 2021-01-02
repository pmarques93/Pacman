
namespace Pacman
{
    /// <summary>
    /// Class for ConsoleScore. Implements IGameObject
    /// </summary>
    class Score : IGameObject
    {
        /// <summary>
        /// Property with this class's name
        /// </summary>
        public string Name => "Score";

        private readonly Collision collisions;

        private uint score;

        /// <summary>
        /// Property that returns a string with current score
        /// </summary>
        public string GetScore { get => score.ToString(); }

        /// <summary>
        /// Constructor for Score
        /// </summary>
        /// <param name="collision">Collision parameter</param>
        public Score(Collision collision)
        {
            collisions = collision;
            score = 0;
        }

        /// <summary>
        /// Method that runs once on start
        /// </summary>
        public void Start() =>
            collisions.ScoreCollision += UpdateScore;

        public void Update() { }

        /// <summary>
        /// Method that runs once on finish
        /// </summary>
        public void Finish() =>
            collisions.ScoreCollision -= UpdateScore;


        /// <summary>
        /// Updates score everytime pacman eats a food
        /// </summary>
        /// <param name="score"></param>
        private void UpdateScore (ushort score)
            => this.score += score;
    }
}
