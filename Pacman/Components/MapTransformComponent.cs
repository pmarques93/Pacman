namespace Pacman.Components
{
    public class MapTransformComponent : Component
    {

        /// <summary>
        /// Property for position
        /// </summary>
        public Vector2Int Position { get; set; }

        public MapTransformComponent(int x, int y)
        {
            Position = new Vector2Int(x, y);
        }
    }
}