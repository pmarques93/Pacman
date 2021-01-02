using System;
using Pacman.GameRelated;

namespace Pacman
{
    class Program
    {
        static void Main(string[] args)
        {
            KeyReaderComponent keyReader = new KeyReaderComponent();
            SceneHandler sceneHandler = new SceneHandler();

            MenuCreation menuCreation = new MenuCreation(keyReader, sceneHandler);
            sceneHandler.AddScene(menuCreation.MenuScene, "MenuScene");

            menuCreation.GenerateScene();

            Scene menuScene = menuCreation.MenuScene;
            sceneHandler.CurrentScene = menuScene;

            sceneHandler.RunScene();
        }
    }
}
