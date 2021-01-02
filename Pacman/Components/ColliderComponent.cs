namespace Pacman
{
    /// <summary>
    /// Component for colliders. Extends Component.
    /// </summary>
    public class ColliderComponent : Component
    {
        /// <summary>
        /// Gets or sets type property.
        /// </summary>
        public Cell Type { get; set; }

        /// <summary>
        /// Constructor for Collider Component.
        /// </summary>
        /// <param name="cell">Cell of this collider.</param>
        public ColliderComponent(Cell cell)
        {
            Type = cell;
        }
    }
}
