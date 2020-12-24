using System;

namespace Pacman
{
    public class SelectorMovementBehaviour : IMovementBehaviour
    {
        // Components
        private readonly KeyReaderComponent keyReader;
        private readonly TransformComponent transform;

        public SelectorMovementBehaviour(GameObject gameObject)
        {
            keyReader = gameObject.GetComponent<KeyReaderComponent>();
            transform = gameObject.GetComponent<TransformComponent>();
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
                    OnStartNewGame();
                    break;
                case 27:
                    OnQuitGame();
                    break;
            }
        }

        protected virtual void OnStartNewGame()
            => StartNewGame?.Invoke();

        protected virtual void OnQuitGame()
            => QuitGame?.Invoke();

        public static event Action StartNewGame;
        public static event Action QuitGame;

        ~SelectorMovementBehaviour()
        {
            keyReader.EnterPressed -= EnterPressed;
        }
    }
}
