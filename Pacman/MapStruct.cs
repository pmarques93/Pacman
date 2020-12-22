namespace Pacman
{
        public struct MapStruct
        {
            public TransformComponent Transform { get; set; }
            public Cell Cell { get; set; }

            public MapStruct(TransformComponent transform, Cell cell)
            {
                Transform = transform;
                Cell = cell;
            }
        }
}