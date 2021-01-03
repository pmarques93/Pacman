namespace Pacman
{
    /// <summary>
    /// Interface for every GameObject.
    /// </summary>
    public interface IGameObject
    {
        /// <summary>
        /// Gets IGameObject's name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Method that runs once on start.
        /// </summary>
        void Start();

        /// <summary>
        /// Method responsible for what happens when the GameObject is running.
        /// </summary>
        void Update();

        /// <summary>
        /// Method that runs once on finish.
        /// </summary>
        void Finish();
    }
}
