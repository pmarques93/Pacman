namespace Pacman.Components
{
    /// <summary>
    /// Struct for spawn struct.
    /// </summary>
    public struct SpawnStruct
    {
        /// <summary>
        /// Gets transform Position.
        /// </summary>
        public Vector2Int TransformPosition { get; }

        /// <summary>
        /// Gets map Transform Position.
        /// </summary>
        public Vector2Int MapTransformPosition { get; }

        /// <summary>
        /// Gets gameObject.
        /// </summary>
        public GameObject GameObject { get; }

        /// <summary>
        /// Constructor for SpawnStruct.
        /// </summary>
        /// <param name="transformComponent">Reference to transform
        /// component.</param>
        /// <param name="mapTransformPosition">Reference to map transform
        /// component.</param>
        /// <param name="gameObject">Reference to a game object.</param>
        public SpawnStruct(
            Vector2Int transformComponent,
            Vector2Int mapTransformPosition,
            GameObject gameObject)
        {
            TransformPosition = transformComponent;
            MapTransformPosition = mapTransformPosition;
            GameObject = gameObject;
        }
    }
}