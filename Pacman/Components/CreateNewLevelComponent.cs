using System;
using Pacman.GameRelated;

namespace Pacman.Components
{
    /// <summary>
    /// Class responsible for creating a new level. Extends Component.
    /// </summary>
    public class CreateNewLevelComponent : Component
    {
        private readonly KeyReaderComponent keyReader;
        private readonly SceneHandler sceneHandler;
        private readonly Random random;

        /// <summary>
        /// Constructor for CreateNewLevelComponent.
        /// </summary>
        /// <param name="keyReader">Reference to keyreader.</param>
        /// <param name="sceneHandler">Reference to scene handler.</param>
        /// <param name="random">Instance of a Random type object.</param>
        public CreateNewLevelComponent(
            KeyReaderComponent keyReader,
            SceneHandler sceneHandler,
            Random random)
        {
            this.keyReader = keyReader;
            this.sceneHandler = sceneHandler;
            this.random = random;
        }

        /// <summary>
        /// Method that runs once on start.
        /// Creates a new level and adds it to scene handler.
        /// </summary>
        public override void Start()
        {
            LevelCreation levelCreation =
                new LevelCreation(keyReader, sceneHandler, random);

            sceneHandler.AddScene(levelCreation.LevelScene, "LevelScene");

            levelCreation.GenerateScene();
        }
    }
}
