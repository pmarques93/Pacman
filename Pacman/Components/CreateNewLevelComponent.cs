﻿using System;
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

        /// <summary>
        /// Constructor for CreateNewLevelComponent.
        /// </summary>
        /// <param name="keyReader">Reference to keyreader.</param>
        /// <param name="sceneHandler">Reference to scene handler.</param>
        public CreateNewLevelComponent(
            KeyReaderComponent keyReader,
            SceneHandler sceneHandler)
        {
            this.keyReader = keyReader;
            this.sceneHandler = sceneHandler;
        }

        /// <summary>
        /// Method that runs once on start.
        /// Creates a new level and adds it to scene handler.
        /// </summary>
        public override void Start()
        {
            Random random = new Random();

            LevelCreation levelCreation =
                new LevelCreation(keyReader, sceneHandler, random);

            sceneHandler.AddScene(levelCreation.LevelScene, "LevelScene");

            levelCreation.GenerateScene();
        }
    }
}
