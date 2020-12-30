using System;
using Pacman.GameRelated;

namespace Pacman
{
    class Program
    {
        static void Main(string[] args)
        {
            /*LevelCreation level = new LevelCreation();
            level.Create();*/

            Random random = new Random();
            KeyReaderComponent keyReader = new KeyReaderComponent();
            SceneHandler sceneHandler = new SceneHandler();


            LevelCreation levelCreation = new LevelCreation(keyReader, sceneHandler, random);
            sceneHandler.AddScene(levelCreation.LevelScene, "LevelScene");

            MenuCreation menuCreation = new MenuCreation(keyReader, sceneHandler);
            sceneHandler.AddScene(menuCreation.MenuScene, "MenuScene");

            menuCreation.GenerateScene();
            levelCreation.GenerateScene();

            Scene menuScene = menuCreation.MenuScene;
            sceneHandler.currentScene = menuScene;

            sceneHandler.RunScene();
        }
    }
}
