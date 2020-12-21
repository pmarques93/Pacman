namespace Pacman
{
    /// <summary>
    /// Interface for every GameObject
    /// </summary>
    interface IGameObject
    {
        /// <summary>
        /// Method that runs once on start
        /// </summary>
        void Start();

        /// <summary>
        /// Method responsible for what happens when the GameObject is running
        /// </summary>
        void Update();

        /// <summary>
        /// Method that runs once on finish
        /// </summary>
        void Finish();
    }
}
