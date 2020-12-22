namespace Pacman
{
    public class MapStruct
    {
        public TransformComponent Transform { get; }
        public ColliderComponent Collider { get; }
        public MapStruct(TransformComponent transform, ColliderComponent collider)
        {
            Transform = transform;
            Collider = collider;
        }
    }
}