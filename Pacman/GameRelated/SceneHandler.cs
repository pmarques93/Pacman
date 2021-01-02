using System.Collections.Generic;
namespace Pacman.GameRelated
{
    public class SceneHandler
    {
        private Dictionary<string, Scene> scenes;
        public bool Terminate { get; set; }
        public Scene CurrentScene { get; set; }

        public SceneHandler()
        {
            scenes = new Dictionary<string, Scene>();
            Terminate = false;
        }

        public Scene FindSceneByName(string name)
        {
            return scenes[name];
        }
        public void AddScene(Scene scene, string name)
        {
            scenes.Add(name, scene);
            scene.SceneHandler = this;
        }

        public void TerminateCurrentScene()
        {
            CurrentScene.Terminate = true;
            Terminate = true;
        }

        public void RunScene()
        {
            while (!Terminate)
            {
                CurrentScene.GameLoop(170);
            }
        }
    }
}