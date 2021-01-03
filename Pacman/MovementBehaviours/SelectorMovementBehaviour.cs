using System;
using Pacman.Components;

namespace Pacman.MovementBehaviours
{
    /// <summary>
    /// Responsible for controlling the movement of selectors.
    /// </summary>
    public class SelectorMovementBehaviour : IMovementBehaviour
    {
        // Components
        private readonly KeyReaderComponent keyReader;
        private readonly TransformComponent transform;
        private readonly SceneChangerComponent sceneChanger;

        /// <summary>
        /// Constructor, that creates a new instance of
        /// SelectorMovementBehaviour and initializes its fields.
        /// </summary>
        /// <param name="gameObject">Object that contains the selector.</param>
        /// <param name="sceneChanger">Instance of the object
        /// responsible for changing scenes.</param>
        public SelectorMovementBehaviour(
                                    GameObject gameObject,
                                    SceneChangerComponent sceneChanger)
        {
            keyReader = gameObject.GetComponent<KeyReaderComponent>();
            transform = gameObject.GetComponent<TransformComponent>();
            this.sceneChanger = sceneChanger;

            keyReader.EnterPressed += EnterPressed;
        }

        /// <summary>
        /// Moves the selector.
        /// </summary>
        /// <param name="xMax">Horizontal size of the map.</param>
        /// <param name="yMax">Vertical size of the map.</param>
        public void Movement(int xMax, int yMax)
        {
            Direction keyPressed = keyReader.Direction;

            // When the user presses a key, pacman changes direction
            if (keyPressed != Direction.None)
            {
                switch (keyPressed)
                {
                    case Direction.Up:
                        if (transform.Position.Y == 36)
                            transform.Position += new Vector2Int(0, -2);
                        break;

                    case Direction.Down:
                        if (transform.Position.Y == 34)
                            transform.Position += new Vector2Int(0, 2);
                        break;
                }
            }
        }

        /// <summary>
        /// Executes the actions for when the enter key is pressed.
        /// </summary>
        private void EnterPressed()
        {
            switch (transform.Position.Y)
            {
                case 34:
                    keyReader.QuitKeys.Clear();
                    keyReader.QuitKeys.Add(ConsoleKey.Escape);
                    sceneChanger.SceneHandler.CurrentScene.Unload = true;
                    sceneChanger.ChangeScene();
                    break;
                case 36:
                    sceneChanger.SceneHandler.TerminateCurrentScene();
                    break;
            }
        }

        /// <summary>
        /// Finalizes an instance of the 
        /// <see cref="SelectorMovementBehaviour"/> class.
        /// </summary>
        ~SelectorMovementBehaviour()
        {
            keyReader.EnterPressed -= EnterPressed;
        }
    }
}
