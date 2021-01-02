using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Pacman.GameRelated;

namespace Pacman
{
    /// <summary>
    /// Represents a game scene
    /// </summary>
    public class Scene
    {
        public bool Terminate { get; set; }
        public bool Unload { get; set; }

        // Game objects in this scene
        private Dictionary<string, IGameObject> gameObjects;
        private HashSet<string> gameObjectsNames;

        /// <summary>
        /// Constructor for scene inGame
        /// </summary>
        /// <param name="xdim">X dimensions</param>
        /// <param name="ydim">Y dimensions</param>
        /// <param name="gameState">Reference to a GameState class</param>
        /// <param name="collision">Reference to a Collision class</param>
        /// <param name="keyReader">Reference to a keyreader class</param>
        public Scene()
        {
            Terminate = false;
            Unload = false;
            gameObjects = new Dictionary<string, IGameObject>();
            gameObjectsNames = new HashSet<string>();
        }

        /// <summary>
        /// Adds a GameObject to the scene.
        /// </summary>
        /// <param name="gameObject">GameObject to be added.</param>
        public void AddGameObject(IGameObject gameObject)
        {
            gameObjects.Add(gameObject.Name ?? " ", gameObject);
            gameObjectsNames.Add(gameObject.Name ?? " ");
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
            for (int i = 0; i < gameObjectsNames.Count; i++)
            {
                gameObjects[gameObjectsNames.ElementAt(i)].Start();
            }

            // keyReader.EscapePressed += SceneHandler.TerminateCurrentScene;

            // Executes the Update() method of the GameObjects on the scene
            while (!Terminate)
            {
                // Time to wait until next frame
                int timeToWait;

                long start = DateTime.Now.Ticks;

                // Update game objects
                // foreach (IGameObject gameObject in gameObjects.Values)
                // {
                //     gameObject.Update();
                // }
                for (int i = 0; i < gameObjectsNames.Count; i++)
                {
                    try
                    {
                        gameObjects[gameObjectsNames.ElementAt(i)].Update();
                    }
                    catch (InvalidOperationException) { }
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
            if (!Unload)
            {
                for (int i = 0; i < gameObjectsNames.Count; i++)
                {
                    gameObjects[gameObjectsNames.ElementAt(i)].Finish();
                }
            }

            // keyReader.EscapePressed -= SceneHandler.TerminateCurrentScene;

        }
    }
}