namespace Pacman.Components
{
    /// <summary>
    /// Class responsible for map transforms. Extends Component.
    /// </summary>
    public class MapTransformComponent : Component
    {
        /// <summary>
        /// Gets or sets position.
        /// </summary>
        public Vector2Int Position { get; set; }

        /// <summary>
        /// Gets or sets direction.
        /// </summary>
        public Direction Direction { get; set; }

        /// <summary>
        /// Constructor for MapTransformComponent.
        /// </summary>
        /// <param name="x">X position.</param>
        /// <param name="y">Y Position.</param>
        /// <param name="direction">Current direction.</param>
        public MapTransformComponent(
            int x, int y, Direction direction = Direction.None)
        {
            Position = new Vector2Int(x, y);
            Direction = direction;
        }
    }
}