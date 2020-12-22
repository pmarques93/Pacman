namespace Pacman
{
    /// <summary>
    /// Component for colliders. Extends Component
    /// </summary>
    public class ColliderComponent: Component
    {
        /// <summary>
        /// Property to get type of collider
        /// </summary>
        public Cell Type { get; }

        /// <summary>
        /// Constructor for Collider Component
        /// </summary>
        /// <param name="cell"></param>
        public ColliderComponent (Cell cell)
        {
            Type = cell;
        }
    }
}
