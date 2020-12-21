namespace Pacman
{
    /// <summary>
    /// Represents a game scene
    /// </summary>
    public class Scene
    {
        // Scene dimensions
        public readonly int xdim;
        public readonly int ydim;
        private bool terminate;

        
        public Scene(int xdim, int ydim)
        {
            this.xdim = xdim;
            this.ydim = ydim;
            terminate = false;
        }

        /// <summary>
        /// Adds a GameObject to the scene.
        /// </summary>
        public void AddGameObject()
        {

        }

        /// <summary>
        /// Looks for a GameObject on the scene.
        /// </summary>
        public void FindGameObjectByName()
        {

        }

        /// <summary>
        /// Terminates the scene.
        /// </summary>
        public void Terminate()
        {
            terminate = true;
        }
        public void GameLoop(int msFramesPerSecond)
        {

        }
    }
}