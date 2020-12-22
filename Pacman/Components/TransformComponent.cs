namespace Pacman
{
    /// <summary>
    /// Transform component. Extends component
    /// </summary>
    public class TransformComponent: Component
    {
        // /// <summary>
        // /// Property for cell type
        // /// </summary>
        // public Cell Cell { get; set; }

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
        public TransformComponent(int x, int y)
        {
            Position = new Vector2Int(x, y);
        }
    }
}
