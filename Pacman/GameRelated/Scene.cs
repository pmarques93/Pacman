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

        private readonly bool inGame;
        private bool createNewGame;
        private bool createNewMenu;

        // Game objects in this scene
        private Dictionary<string, IGameObject> gameObjects;

        // Component
        private readonly GameState gameState;
        private readonly Collision collisions;
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
        public Scene(byte xdim, byte ydim, GameState gameState, 
            Collision collision, KeyReaderComponent keyReader,
            LivesComponent lives)
        {
            this.xdim = xdim;
            this.ydim = ydim;
            terminate = false;
            gameObjects = new Dictionary<string, IGameObject>();
            this.gameState = gameState;
            this.collisions = collision;
            inGame = true;
            createNewGame = false;
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
            inGame = false;
            this.keyReader = keyReader;

            createNewGame = false;
            SelectorMovementBehaviour.QuitGame += () => terminate = true;
            SelectorMovementBehaviour.StartNewGame += CreateNewGame;
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
            foreach (IGameObject gameObject in gameObjects.Values)
            {
                gameObject.Start();
            }

            keyReader.EscapePressed += () => terminate = true;
            if (inGame)
            {
                lives.EndGame += CreateNewMenu;
                collisions.FoodCollision += RemoveGameObject;
                gameState.GhostChaseCollision += CreateNewGame;
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

            keyReader.EscapePressed -= () => terminate = true;
            if (inGame)
            {
                lives.EndGame -= CreateNewMenu;
                collisions.FoodCollision -= RemoveGameObject;
                gameState.GhostChaseCollision -= CreateNewGame;
            }

            // After the loop, if true, creates a new game
            if (createNewGame)
            {
                LevelCreation levelCreation = new LevelCreation();
                levelCreation.Create();
            }

            if (createNewMenu)
            {
                MenuCreation menuCreation = new MenuCreation();
                menuCreation.Run();
            }
        }

        /// <summary>
        /// Creates a new menu
        /// </summary>
        private void CreateNewMenu()
        {
            createNewMenu = true;
            terminate = true;
        }

        /// <summary>
        /// Creates a new game.
        /// Removes a life from pacman.
        /// </summary>
        private void CreateNewGame()
        {
            FileReader fileReader = new FileReader(Path.lives);
            byte currentLives = fileReader.ReadLives();
            FileWriter fileWriter = new FileWriter(Path.lives);
            fileWriter.CreateLivesText(--currentLives);

            createNewGame = true;
            terminate = true;
        }

        /// <summary>
        /// Deconstructor for scene
        /// </summary>
        ~Scene()
        {
            SelectorMovementBehaviour.QuitGame -= () => terminate = true;
            SelectorMovementBehaviour.StartNewGame -= CreateNewGame;
        }
    }
}