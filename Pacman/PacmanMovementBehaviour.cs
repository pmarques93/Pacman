using System;

namespace Pacman
{
    public class PacmanMovementBehaviour : IMovementBehaviour
    {
        // Last Key pressed for continuous movement
        private Direction pacmanDirection;

        public KeyReaderComponent KeyReader { get; set; }
        public TransformComponent Transform { get; set; }
        public MapComponent Map { get; set; }


        public void Movement(int xMax, int yMax)
        {
            Direction keyPressed = KeyReader.Direction;

            // When the user presses a key, pacman changes direction
            if (keyPressed != Direction.None)
            {
                switch (keyPressed)
                {
                    case Direction.Up:
                        pacmanDirection = Direction.Up;
                        break;

                    case Direction.Right:
                        pacmanDirection = Direction.Right;
                        break;

                    case Direction.Down:
                        pacmanDirection = Direction.Down;
                        break;

                    case Direction.Left:
                        pacmanDirection = Direction.Left;
                        break;
                }
            }

            // After the user pressed a key, pacman keeps moving towards
            // previous direction
            else if (pacmanDirection != Direction.None)
            {
                switch (pacmanDirection)
                {
                    case Direction.Up:
                        if (Map.Map[Transform.Position.X,
                            Math.Max(0, Transform.Position.Y - 1)].
                            Cell != Cell.Wall)
                        {
                            Transform.Position =
                            new Vector2Int(Transform.Position.X,
                            Math.Max(0, Transform.Position.Y - 1));
                        }
                        break;

                    case Direction.Right:
                        if (Map.Map[
                            Math.Min(xMax - 1, Transform.Position.X + 1),
                            Transform.Position.Y].
                            Cell != Cell.Wall)
                        {
                            Transform.Position =
                            new Vector2Int(
                            Math.Min(xMax - 1, Transform.Position.X + 1),
                            Transform.Position.Y);
                        }
                        break;

                    case Direction.Down:
                        if (Map.Map[Transform.Position.X,
                            Math.Min(yMax - 1, Transform.Position.Y + 1)].
                            Cell != Cell.Wall)
                        {
                            Transform.Position =
                            new Vector2Int(Transform.Position.X,
                            Math.Min(yMax - 1, Transform.Position.Y + 1));
                        }
                        break;

                    case Direction.Left:
                        if (Map.Map[
                            Math.Max(0, Transform.Position.X - 1),
                            Transform.Position.Y].
                            Cell != Cell.Wall)
                        {
                            Transform.Position =
                            new Vector2Int(
                            Math.Max(0, Transform.Position.X - 1),
                            Transform.Position.Y);
                        }
                        break;
                }
            }
        }
    }
}
