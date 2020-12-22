namespace Pacman
{
    /// <summary>
    /// Map Component. Extends Component
    /// </summary>
    public class MapComponent : Component
    {
        // Map made of Transforms
        public TransformComponent[,] Map { get; }

        public MapStruct[,] MapTest { get; }

        /// <summary>
        /// Constructor for MapComponent
        /// </summary>
        /// <param name="xDim">X size</param>
        /// <param name="yDim">Y size</param>
        public MapComponent(byte xDim, byte yDim)
        {
            Map = new TransformComponent[xDim, yDim];
            MapTest = new MapStruct[xDim, yDim];
            CreatePacmanMap();
        }

        /// <summary>
        /// Creates map for pacman
        /// </summary>
        private void CreatePacmanMap()
        {
            for (int i = 0; i < MapTest.GetLength(0); i++)
            {
                for (int j = 0; j < MapTest.GetLength(1); j++)
                {
                    MapTest[i, j] = new MapStruct(
                                        new TransformComponent(i, j),
                                        new ColliderComponent(Cell.Walkable));
                }
            }
            MapTest[6, 6] = new MapStruct(new TransformComponent(6, 6),
                                            new ColliderComponent(Cell.Wall));
            MapTest[7, 6] = new MapStruct(new TransformComponent(7, 6),
                                            new ColliderComponent(Cell.Wall));
            MapTest[8, 6] = new MapStruct(new TransformComponent(8, 6),
                                            new ColliderComponent(Cell.Wall));
            MapTest[9, 6] = new MapStruct(new TransformComponent(9, 6),
                                            new ColliderComponent(Cell.Wall));
        }

        // Temp 
        /*
        /// <summary>
        /// Creates a walkable position everytime there is a collision
        /// with somthing that disappears
        /// </summary>
        /// <param name="position">Position to create</param>
        private void CreateWalkablePosition(Vector2Int position)
            => Map[position.X, position.Y] =
                new TransformComponent(new ColliderComponent(Cell.Walkable),
                    position.X, position.Y);
        */
    }
}
