using System;

namespace Pacman
{
    /// <summary>
    /// Movement Component. Extends Component
    /// </summary>
    public class MoveComponent: Component
    {
        // Position related variables
        private int maxX, maxY;
        private byte initPosX, initPosY;

        // Last Key pressed for continuous movement
        private Direction pacmanDirection;

        // Components
        private KeyReaderComponent keyReader;
        private TransformComponent transform;
        private MapComponent map;

        /// <summary>
        /// Constructor for Move component
        /// </summary>
        /// <param name="initPosX">X position</param>
        /// <param name="initPosY">Y position</param>
        public MoveComponent(byte initPosX, byte initPosY)
        {
            this.initPosX = initPosX;
            this.initPosY = initPosY;
        }

        /// <summary>
        /// Method that runs once on start
        /// </summary>
        public override void Start()
        {
            keyReader = ParentGameObject.GetComponent<KeyReaderComponent>();
            transform = ParentGameObject.GetComponent<TransformComponent>();
            map = ParentGameObject.GetComponent<MapComponent>();

            // Adds initial position
            transform.Position = new Vector2Int(initPosX, initPosY);

            maxX = map.Map.GetLength(0);
            maxY = map.Map.GetLength(1);
        }

        /// <summary>
        /// Method responsible for what happens when the GameObject is running
        /// </summary>
        public override void Update()
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
                        if (map.Map[transform.Position.X, 
                            Math.Max(0, transform.Position.Y - 1)].
                            Cell != Cell.Wall)
                            {
                                transform.Position =
                                new Vector2Int(transform.Position.X,
                                Math.Max(0, transform.Position.Y - 1));
                            }
                        break;

                    case Direction.Right:
                        if (map.Map[
                            Math.Min(maxX - 1, transform.Position.X + 1),
                            transform.Position.Y].
                            Cell != Cell.Wall)
                            {
                                transform.Position =
                                new Vector2Int(
                                Math.Min(maxX - 1, transform.Position.X + 1),
                                transform.Position.Y);
                            }
                        break;

                    case Direction.Down:
                        if (map.Map[transform.Position.X, 
                            Math.Min(maxY - 1, transform.Position.Y + 1)].
                            Cell != Cell.Wall)
                            {
                                transform.Position =
                                new Vector2Int(transform.Position.X,
                                Math.Min(maxY - 1, transform.Position.Y + 1));
                            }
                        break;

                    case Direction.Left:
                        if (map.Map[
                            Math.Max(0, transform.Position.X - 1),
                            transform.Position.Y].
                            Cell != Cell.Wall)
                            {
                                transform.Position =
                                new Vector2Int(
                                Math.Max(0, transform.Position.X - 1),
                                transform.Position.Y);
                            }
                        break;
                }
            }
        }
    }
}
