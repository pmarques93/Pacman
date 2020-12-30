using Pacman.GameRelated;

namespace Pacman.Components
{
    public class SceneChangerComponent : Component
    {
        private KeyReaderComponent keyReader;
        private Scene currentScene;
        public readonly SceneHandler sceneHandler;

        public string sceneToLoad;
        public SceneChangerComponent(KeyReaderComponent keyReader,
                                     Scene currentScene,
                                     SceneHandler sceneHandler)
        {
            this.keyReader = keyReader;
            this.currentScene = currentScene;
            this.sceneHandler = sceneHandler;
            sceneToLoad = "";
        }

        public void ChangeScene()
        {
            sceneHandler.currentScene.terminate = true;
            sceneHandler.currentScene = sceneHandler.FindSceneByName(sceneToLoad);
            sceneHandler.currentScene.terminate = false;
        }

    }
}