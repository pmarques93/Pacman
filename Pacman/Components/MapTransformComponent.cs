namespace Pacman.Components
{
    public class MapTransformComponent : Component
    {

        /// <summary>
        /// Property for position
        /// </summary>
        public Vector2Int Position { get; set; }

        public Direction Direction { get; set; }

        public MapTransformComponent(int x, int y, Direction direction = Direction.None)
        {
            Position = new Vector2Int(x, y);
            Direction = direction;
        }
    }
}