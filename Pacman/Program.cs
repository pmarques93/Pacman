using System;
using Pacman.Components;
using Pacman.GameRelated;

namespace Pacman
{
    /// <summary>
    /// Program class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Static method that runs on start.
        /// </summary>
        /// <param name="args">Arguments.</param>
        public static void Main(string[] args)
        {
            KeyReaderComponent keyReader = new KeyReaderComponent();
            SceneHandler sceneHandler = new SceneHandler();

            MenuCreation menuCreation = new MenuCreation(keyReader, sceneHandler);
            sceneHandler.AddScene(menuCreation.MenuScene, "MenuScene");

            menuCreation.GenerateScene();
            sceneHandler.CurrentScene = menuCreation.MenuScene;

            // Tries to run scene. If the window size is too small
            // it shows a message asking to expand the window size
            try
            {
                sceneHandler.RunScene();
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("Please expand your window size");
            }
        }
    }
}
