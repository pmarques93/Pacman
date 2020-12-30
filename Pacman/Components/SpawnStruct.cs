namespace Pacman.Components
{
    public struct SpawnStruct
    {
        public Vector2Int TransformPosition { get; }
        public Vector2Int MapTransformPosition { get; }
        public GameObject GameObject { get; }
        public SpawnStruct(Vector2Int transformComponent,
                            Vector2Int mapTransformPosition,
                            GameObject gameObject)
        {
            TransformPosition = transformComponent;
            MapTransformPosition = mapTransformPosition;
            GameObject = gameObject;
        }
    }
}