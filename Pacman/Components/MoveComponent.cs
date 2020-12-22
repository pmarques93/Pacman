using System;

namespace Pacman
{
    /// <summary>
    /// Movement Component. Extends Component
    /// </summary>
    public class MoveComponent: Component
    {
        private readonly byte maxX, maxY;
        private int x, y;

        // Components
        private KeyReaderComponent keyReader;
        private TransformComponent transform;

        /// <summary>
        /// Constructor for Move component
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="maxX">X max range</param>
        /// <param name="maxY">Y max range</param>
        public MoveComponent(int x, int y, byte maxX, byte maxY)
        {
            this.x = x;
            this.y = y;
            this.maxX = maxX;
            this.maxY = maxY;
        }

        /// <summary>
        /// Method that runs once on start
        /// </summary>
        public override void Start()
        {
            keyReader = ParentGameObject.GetComponent<KeyReaderComponent>();
            transform = ParentGameObject.GetComponent<TransformComponent>();

            transform.Position = new Vector2Int(x, y);
        }

        /// <summary>
        /// Method responsible for what happens when the GameObject is running
        /// </summary>
        public override void Update()
        {
            Direction dir = keyReader.Direction;
            if (dir != Direction.None)
            {
                switch (dir)
                {
                    case Direction.Up:
                        transform.Position =
                            new Vector2Int(transform.Position.X,
                            Math.Max(0, transform.Position.Y - 1));
                        break;
                    case Direction.Right:
                        transform.Position =
                            new Vector2Int(
                                Math.Min(maxX - 1, transform.Position.X + 1),
                                transform.Position.Y);
                        break;
                    case Direction.Down:
                        transform.Position =
                            new Vector2Int(transform.Position.X,
                            Math.Min(maxY - 1, transform.Position.Y + 1));
                        break;
                    case Direction.Left:
                        transform.Position =
                            new Vector2Int(
                                Math.Min(0, transform.Position.X - 1),
                                transform.Position.Y);
                        break;
                }
            }
        }
    }
}
