using System;

namespace Pacman
{
    /// <summary>
    /// Class for pacman movement. Implements IMovementBehaviour
    /// </summary>
    public class PacmanMovementBehaviour : IMovementBehaviour
    {
        // Last Key pressed for continuous movement
        private Direction pacmanDirection;

        // Components
        private readonly KeyReaderComponent keyReader;
        private readonly TransformComponent transform;

        private int incrementCounter;

        private readonly TransformComponent mapTransform;
        private readonly MapComponent map;

        /// <summary>
        /// Gets components from pacman gameobject
        /// </summary>
        /// <param name="pacman">Object to get components from</param>
        public PacmanMovementBehaviour(GameObject pacman)
        {
            keyReader = pacman.GetComponent<KeyReaderComponent>();
            transform = pacman.GetComponent<TransformComponent>();
            mapTransform = new TransformComponent(5, transform.Position.Y);
            map = pacman.GetComponent<MapComponent>();
            incrementCounter = 0;
        }

        /// <summary>
        /// Gets components from pacman gameobject
        /// </summary>
        /// <param name="pacman">Object to get components from</param>
        /// <param name="moveComponent">Move component to add
        /// this behaviour to</param>
        public PacmanMovementBehaviour(GameObject pacman,
            MoveComponent moveComponent)
        {
            keyReader = pacman.GetComponent<KeyReaderComponent>();
            transform = pacman.GetComponent<TransformComponent>();
            map = pacman.GetComponent<MapComponent>();

            moveComponent.AddMovementBehaviour(this);
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
                        if (map.Map[mapTransform.Position.X,
                            Math.Max(0, mapTransform.Position.Y - 1)].
                            Collider.Type != Cell.Wall)
                        {


                            transform.Position =
                            new Vector2Int(transform.Position.X,
                            Math.Max(0, transform.Position.Y - 1));


                            mapTransform.Position =
                            new Vector2Int(mapTransform.Position.X,
                            Math.Max(0, mapTransform.Position.Y - 1));

                            // Console.WriteLine($"transform.Position: {transform.Position}");
                            // Console.WriteLine($"mapTransform.Position: {mapTransform.Position}");
                            // System.Threading.Thread.Sleep(500);
                        }
                        break;

                    case Direction.Right:
                        if (map.Map[
                            Math.Min(xMax - 1, mapTransform.Position.X + 1),
                            mapTransform.Position.Y].
                            Collider.Type != Cell.Wall)
                        {

                            mapTransform.Position =
                            new Vector2Int(
                            Math.Min(xMax - 1, mapTransform.Position.X + 1),
                            mapTransform.Position.Y);

                            
                            transform.Position =
                                new Vector2Int(
                                Math.Min(xMax * 2 - 1, transform.Position.X + 2),
                                transform.Position.Y);
              
                            // Console.WriteLine($"transform.Position: {transform.Position}");
                            // Console.WriteLine($"mapTransform.Position: {mapTransform.Position}");
                            // System.Threading.Thread.Sleep(500);
                        }
                        break;

                    case Direction.Down:
                        if (map.Map[mapTransform.Position.X,
                            Math.Min(yMax - 1, mapTransform.Position.Y + 1)].
                            Collider.Type != Cell.Wall)
                        {
                            transform.Position =
                            new Vector2Int(transform.Position.X,
                            Math.Min(yMax - 1, transform.Position.Y + 1));


                            mapTransform.Position =
                            new Vector2Int(mapTransform.Position.X,
                            Math.Min(yMax - 1, mapTransform.Position.Y + 1));

                            // Console.WriteLine($"transform.Position: {transform.Position}");
                            // Console.WriteLine($"mapTransform.Position: {mapTransform.Position}");
                            // System.Threading.Thread.Sleep(500);
                        }
                        break;

                    case Direction.Left:
                        if (map.Map[
                            Math.Max(0, mapTransform.Position.X - 1),
                            mapTransform.Position.Y].
                            Collider.Type != Cell.Wall)
                        {

                            mapTransform.Position =
                            new Vector2Int(
                            Math.Max(0, mapTransform.Position.X - 1),
                            mapTransform.Position.Y);


                            transform.Position =
                            new Vector2Int(
                            Math.Max(0, transform.Position.X - 2),
                            transform.Position.Y);

                            // Console.WriteLine($"transform.Position: {transform.Position}");
                            // Console.WriteLine($"mapTransform.Position: {mapTransform.Position}");
                            // System.Threading.Thread.Sleep(500);

                        }
                        break;
                }
            }
        }
    }
}
