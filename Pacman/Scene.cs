using System;
using System.Collections.Generic;
using System.Threading;

namespace Pacman
{
    /// <summary>
    /// Represents a game scene
    /// </summary>
    public class Scene
    {
        // Scene dimensions
        public readonly byte xdim;
        public readonly byte ydim;
        private bool terminate;
        // Game objects in this scene
        private Dictionary<string, IGameObject> gameObjects;
        public Scene(byte xdim, byte ydim)
        {
            this.xdim = xdim;
            this.ydim = ydim;
            terminate = false;
            gameObjects = new Dictionary<string, IGameObject>();
        }

        /// <summary>
        /// Adds a GameObject to the scene.
        /// </summary>
        /// <param name="gameObject">GameObject to be added.</param>
        public void AddGameObject(IGameObject gameObject)
        {
            gameObjects.Add(gameObject.Name ?? " ", gameObject);
        }

        /// <summary>
        /// Removes a GameObject from the scene
        /// </summary>
        /// <param name="gameObject">Object to remove</param>
        public void RemoveGameObject(IGameObject gameObject)
        {
            gameObjects.Remove(gameObject.Name);
        }

        /// <summary>
        /// Finds a GameObject with a given name.
        /// </summary>
        /// <param name="name">Name of the GameObject.</param>
        /// <returns>Returns the GameObject that was found.</returns>
        public IGameObject FindGameObjectByName(string name)
        {
            return gameObjects[name];
        }

        /// <summary>
        /// Method responsible for the main gameLoop
        /// </summary>
        /// <param name="msFramesPerSecond">Miliseconds to wait</param>
        public void GameLoop(int msFramesPerSecond)
        {
            // // Calls the Start() method of the GameObjects on the scene
            // foreach (IGameObject gameObject in gameObjects.Values)
            // {
            //     Console.WriteLine(gameObject.Name);
            // }
            foreach (IGameObject gameObject in gameObjects.Values)
            {
                gameObject.Start();
            }

            // Executes the Update() method of the GameObjects on the scene
            while (!terminate)
            {
                // Time to wait until next frame
                int timeToWait;

                long start = DateTime.Now.Ticks;

                // Update game objects
                foreach (IGameObject gameObject in gameObjects.Values)
                {
                    gameObject.Update();
                }

                // Time to wait until next frame
                timeToWait = (int)(start / TimeSpan.TicksPerMillisecond
                    + msFramesPerSecond
                    - DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond);

                // If this time is negative it is set to zero
                timeToWait = timeToWait > 0 ? timeToWait : 0;

                // Wait until next frame
                Thread.Sleep(timeToWait);
            }

            // Executes the Finish() method of the GameObjects on the scene
            // tearing them down
            foreach (IGameObject gameObject in gameObjects.Values)
            {
                gameObject.Finish();
            }
        }
    }
}