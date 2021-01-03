using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Pacman.GameRelated
{
    /// <summary>
    /// Represents a game scene.
    /// </summary>
    public class Scene
    {
        /// <summary>
        /// Gets or sets a value indicating whether the scene terminated or not.
        /// </summary>
        public bool Terminate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the scene unloaded.
        /// </summary>
        public bool Unload { get; set; }

        // Game objects in this scene
        private readonly Dictionary<string, IGameObject> gameObjects;
        private readonly HashSet<string> gameObjectsNames;

        /// <summary>
        /// Constructor for Scene.
        /// </summary>
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
        /// Method responsible for the main gameLoop.
        /// </summary>
        /// <param name="msFramesPerSecond">Miliseconds to wait.</param>
        public void GameLoop(int msFramesPerSecond)
        {
            for (int i = 0; i < gameObjectsNames.Count; i++)
            {
                gameObjects[gameObjectsNames.ElementAt(i)].Start();
            }

            // Executes the Update() method of the GameObjects on the scene
            while (!Terminate)
            {
                // Time to wait until next frame
                int timeToWait;

                long start = DateTime.Now.Ticks;

                // Update game objects
                for (int i = 0; i < gameObjectsNames.Count; i++)
                {
                    try
                    {
                        gameObjects[gameObjectsNames.ElementAt(i)].Update();
                    }
                    catch (InvalidOperationException)
                    {
                        // Exception intentionally left empty.
                    }
                }

                // Time to wait until next frame
                timeToWait = (int)((start / TimeSpan.TicksPerMillisecond)
                    + msFramesPerSecond
                    - (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond));

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
        }
    }
}