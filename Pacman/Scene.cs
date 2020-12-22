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
        private Dictionary<string, GameObject> gameObjects;

        public Scene(byte xdim, byte ydim)
        {
            this.xdim = xdim;
            this.ydim = ydim;
            terminate = false;
            gameObjects = new Dictionary<string, GameObject>();
        }

        /// <summary>
        /// Adds a GameObject to the scene.
        /// </summary>
        /// <param name="gameObject">GameObject to be added.</param>
        public void AddGameObject(GameObject gameObject)
        {
            gameObjects.Add(gameObject.Name, gameObject);
        }

        /// <summary>
        /// Finds a GameObject with a given name.
        /// </summary>
        /// <param name="name">Name of the GameObject.</param>
        /// <returns>Returns the GameObject that was found.</returns>
        public GameObject FindGameObjectByName(string name)
        {
            return gameObjects[name];
        }

        /// <summary>
        /// Terminates the scene.
        /// </summary>
        public void Terminate()
        {
            terminate = true;
        }

        /// <summary>
        /// Sets initial scene
        /// </summary>
        public void SetupScene()
        {
            GameObject pacman = new GameObject("Pacman");
            GameObject pinky = new GameObject("Pinky");

            KeyReaderComponent keyReader = new KeyReaderComponent();
            keyReader.EscapePressed += () => terminate = true;

            // Pacman
            TransformComponent pacmanTransform = new TransformComponent();
            MoveComponent pacmanMovement = new MoveComponent(5, 5, xdim, ydim);

            pacman.AddComponent(keyReader);
            pacman.AddComponent(pacmanTransform);
            pacman.AddComponent(pacmanMovement);

            AddGameObject(pacman);

            // Pinky
            TransformComponent pinkyTransform = new TransformComponent();
            MoveComponent pinkyMovement = new MoveComponent(2, 2, xdim, ydim);

            pinky.AddComponent(pinkyTransform);
            pinky.AddComponent(pinkyMovement);
            
            AddGameObject(pinky);


            // PARA TESTES SÓ
            pacman.Start();
            Console.WriteLine(pacman.GetComponent<TransformComponent>().Position.X);
        }

        /// <summary>
        /// Method responsible for the main gameLoop
        /// </summary>
        /// <param name="msFramesPerSecond">Miliseconds to wait</param>
        public void GameLoop(int msFramesPerSecond)
        {
            // Calls the Start() method of the GameObjects on the scene
            foreach (GameObject gameObject in gameObjects.Values)
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
                foreach (GameObject gameObject in gameObjects.Values)
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
            foreach (GameObject gameObject in gameObjects.Values)
            {
                gameObject.Finish();
            }
        }
    }
}