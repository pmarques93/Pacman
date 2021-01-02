using Pacman.GameRelated;

namespace Pacman.Components
{
    /// <summary>
    /// Class responsible for changing scenes. Extends component.
    /// </summary>
    public class SceneChangerComponent : Component
    {
        /// <summary>
        /// Gets or sets sceneHandler.
        /// </summary>
        public SceneHandler SceneHandler { get; set; }

        /// <summary>
        /// Gets or sets sceneToLoad.
        /// </summary>
        public string SceneToLoad { get; set; }

        /// <summary>
        /// Constructor for SceneChangerComponent.
        /// </summary>
        /// <param name="sceneHandler">Reference to sceneHandler.</param>
        public SceneChangerComponent(SceneHandler sceneHandler)
        {
            SceneHandler = sceneHandler;
            SceneToLoad = string.Empty;
        }

        /// <summary>
        /// Method responsible for changing scenes.
        /// </summary>
        public void ChangeScene()
        {
            SceneHandler.CurrentScene.Terminate = true;
            SceneHandler.CurrentScene = SceneHandler.FindSceneByName(SceneToLoad);
            SceneHandler.CurrentScene.Terminate = false;
        }
    }
}