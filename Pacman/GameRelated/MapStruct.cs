using Pacman.Components;

namespace Pacman.GameRelated
{
    /// <summary>
    /// Struct responsible for map structure.
    /// </summary>
    public struct MapStruct
    {
        /// <summary>
        /// Gets collider.
        /// </summary>
        public ColliderComponent Collider { get; }

        /// <summary>
        /// Constructor for MapStruct.
        /// </summary>
        /// <param name="collider">Reference to collider.</param>
        public MapStruct(ColliderComponent collider)
        {
            Collider = collider;
        }
    }
}