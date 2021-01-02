using System.Collections.Generic;

namespace Pacman.GameRelated
{
    /// <summary>
    /// Class responsible for handling scenes.
    /// </summary>
    public class SceneHandler
    {
        private readonly Dictionary<string, Scene> scenes;

        /// <summary>
        /// Gets or sets a value indicating whether the scene terminated or not.
        /// </summary>
        public bool Terminate { get; set; }

        /// <summary>
        /// Gets or sets currentScene.
        /// </summary>
        public Scene CurrentScene { get; set; }

        /// <summary>
        /// Constructor for SceneHandler.
        /// </summary>
        public SceneHandler()
        {
            scenes = new Dictionary<string, Scene>();
            Terminate = false;
        }

        /// <summary>
        /// Method that finds a scene by name.
        /// </summary>
        /// <param name="name">Name to look for.</param>
        /// <returns>Returns a scene.</returns>
        public Scene FindSceneByName(string name)
        {
            return scenes[name];
        }

        /// <summary>
        /// Method to add a scene.
        /// </summary>
        /// <param name="scene">Scene to add.</param>
        /// <param name="name">Name of the scene.</param>
        public void AddScene(Scene scene, string name)
        {
            scenes.Add(name, scene);
        }

        /// <summary>
        /// Method that removes a scene.
        /// </summary>
        /// <param name="name">Name of the scene to remove.</param>
        public void RemoveScene(string name)
        {
            scenes.Remove(name);
        }

        /// <summary>
        /// Method to terminate a scene.
        /// </summary>
        public void TerminateCurrentScene()
        {
            CurrentScene.Terminate = true;
            Terminate = true;
        }

        /// <summary>
        /// Method used to run a scene.
        /// </summary>
        public void RunScene()
        {
            while (!Terminate)
            {
                CurrentScene.GameLoop(170);
            }
        }
    }
}