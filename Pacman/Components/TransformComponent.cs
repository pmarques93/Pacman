namespace Pacman
{
    /// <summary>
    /// Transform component. Extends component
    /// </summary>
    public class TransformComponent: Component
    {
        /// <summary>
        /// Collider component on this transform
        /// </summary>
        public ColliderComponent Collider { get; set; }

        /// <summary>
        /// Property for position
        /// </summary>
        public Vector2Int Position { get; set; }

        /// <summary>
        /// Constructor for TransformComponent
        /// </summary>
        /// <param name="cell">Type of cell</param>
        /// <param name="x">Position X</param>
        /// <param name="y">Position Y</param>
        public TransformComponent(ColliderComponent collider, int x, int y)
        {
            Position = new Vector2Int(x, y);
            Collider = collider;
        }

        /// <summary>
        /// Empty Constructor
        /// </summary>
        public TransformComponent() { }
    }
}
