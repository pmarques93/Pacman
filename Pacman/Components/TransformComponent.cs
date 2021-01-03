namespace Pacman.Components
{
    /// <summary>
    /// Transform component. Extends component.
    /// </summary>
    public class TransformComponent : Component
    {
        /// <summary>
        /// Gets or sets position.
        /// </summary>
        public Vector2Int Position { get; set; }

        /// <summary>
        /// Constructor for TransformComponent.
        /// </summary>
        /// <param name="x">Position X.</param>
        /// <param name="y">Position Y.</param>
        public TransformComponent(int x, int y)
        {
            Position = new Vector2Int(x, y);
        }

        /// <summary>
        /// Empty Constructor.
        /// </summary>
        public TransformComponent()
        {
        }
    }
}
