using Pacman.Components;

namespace Pacman.GameRelated
{
    /// <summary>
    /// Struct responsible for map structure.
    /// </summary>
    public struct MapStruct
    {
        /// <summary>
        /// Gets transform.
        /// </summary>
        public TransformComponent Transform { get; }

        /// <summary>
        /// Gets collider.
        /// </summary>
        public ColliderComponent Collider { get; }

        /// <summary>
        /// Constructor for MapStruct.
        /// </summary>
        /// <param name="transform">Reference to transform.</param>
        /// <param name="collider">Reference to collider.</param>
        public MapStruct(
            TransformComponent transform, ColliderComponent collider)
        {
            Transform = transform;
            Collider = collider;
        }
    }
}