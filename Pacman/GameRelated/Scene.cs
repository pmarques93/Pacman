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
        // Scene dimensions
        public readonly byte xdim;
        public readonly byte ydim;
        public bool terminate;
        public bool unload;

        private readonly bool inGame;

        // Game objects in this scene
        private Dictionary<string, IGameObject> gameObjects;
        private HashSet<string> gameObjectsNames;

        public SceneHandler sceneHandler;

        // Component
        private readonly GameState gameState;
        private readonly KeyReaderComponent keyReader;
        private readonly LivesComponent lives;

        /// <summary>
        /// Constructor for scene inGame
        /// </summary>
        /// <param name="xdim">X dimensions</param>
        /// <param name="ydim">Y dimensions</param>
        /// <param name="gameState">Reference to a GameState class</param>
        /// <param name="collision">Reference to a Collision class</param>
        /// <param name="keyReader">Reference to a keyreader class</param>
        public Scene(byte xdim, byte ydim, GameState gameState, KeyReaderComponent keyReader,
            LivesComponent lives)
        {
            this.xdim = xdim;
            this.ydim = ydim;
            terminate = false;
            unload = false;
            gameObjects = new Dictionary<string, IGameObject>();
            gameObjectsNames = new HashSet<string>();
            this.gameState = gameState;
            inGame = true;
            this.keyReader = keyReader;
            this.lives = lives;
        }

        /// <summary>
        /// Constructor for menu
        /// </summary>
        /// <param name="xdim">X dimensions</param>
        /// <param name="ydim">Y dimensions</param>
        /// <param name="keyReader">Reference to a keyreader</param>
        public Scene(byte xdim, byte ydim, KeyReaderComponent keyReader)
        {
            this.xdim = xdim;
            this.ydim = ydim;
            terminate = false;
            gameObjects = new Dictionary<string, IGameObject>();
            gameObjectsNames = new HashSet<string>();
            inGame = false;
            this.keyReader = keyReader;
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

            // keyReader.EscapePressed += sceneHandler.TerminateCurrentScene;

            // Executes the Update() method of the GameObjects on the scene
            while (!terminate)
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
                    gameObjects[gameObjectsNames.ElementAt(i)].Update();
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
            if (!unload)
            {
                for (int i = 0; i < gameObjectsNames.Count; i++)
                {
                    gameObjects[gameObjectsNames.ElementAt(i)].Finish();
                }
            }

            // keyReader.EscapePressed -= sceneHandler.TerminateCurrentScene;

        }
    }
}