using System.Collections.Generic;
namespace Pacman.GameRelated
{
    public class SceneHandler
    {
        private Dictionary<string, Scene> scenes;
        public bool terminate;
        public Scene currentScene;

        public SceneHandler()
        {
            scenes = new Dictionary<string, Scene>();
            terminate = false;
        }

        public Scene FindSceneByName(string name)
        {
            return scenes[name];
        }
        public void AddScene(Scene scene, string name)
        {
            scenes.Add(name, scene);
            scene.sceneHandler = this;
        }

        public void TerminateCurrentScene()
        {
            currentScene.terminate = true;
            terminate = true;
        }

        public void RunScene()
        {
            while (!terminate)
            {
                currentScene.GameLoop(170);
            }
        }
    }
}