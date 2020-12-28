using System;
using Pacman.Components;

namespace Pacman
{
    public class SelectorMovementBehaviour : IMovementBehaviour
    {
        // Components
        private readonly KeyReaderComponent keyReader;
        private readonly TransformComponent transform;
        private readonly SceneChangerComponent sceneChanger;

        public SelectorMovementBehaviour(GameObject gameObject,
                                         SceneChangerComponent sceneChanger)
        {
            keyReader = gameObject.GetComponent<KeyReaderComponent>();
            transform = gameObject.GetComponent<TransformComponent>();
            this.sceneChanger = sceneChanger;

            keyReader.EnterPressed += EnterPressed;
        }

        /// <summary>
        /// Movement method for pacman
        /// </summary>
        /// <param name="xMax">X map size</param>
        /// <param name="yMax">Y map size</param>
        public void Movement(int xMax, int yMax)
        {
            Direction keyPressed = keyReader.Direction;

            // When the user presses a key, pacman changes direction
            if (keyPressed != Direction.None)
            {
                switch (keyPressed)
                {
                    case Direction.Up:
                        if (transform.Position.Y == 27)
                            transform.Position += new Vector2Int(0, -2);
                        break;

                    case Direction.Down:
                        if (transform.Position.Y == 25)
                            transform.Position += new Vector2Int(0, 2);
                        break;
                }
            }
        }

        private void EnterPressed()
        {
            switch (transform.Position.Y)
            {
                case 25:
                    sceneChanger.ChangeScene();
                    keyReader.EnterPressed -= EnterPressed;
                    break;
                case 27:
                    sceneChanger.sceneHandler.TerminateCurrentScene();
                    keyReader.EnterPressed -= EnterPressed;
                    break;
            }
        }

        ~SelectorMovementBehaviour()
        {
            keyReader.EnterPressed -= EnterPressed;
        }
    }
}
