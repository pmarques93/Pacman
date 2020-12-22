namespace Pacman
{
    /// <summary>
    /// Map Component. Extends Component
    /// </summary>
    public class MapComponent : Component
    {
        // Map made of Transforms
        public TransformComponent[,] Map { get; }

        public MapStruct[,] MapTest { get; private set; }


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
            for (int i = 0; i < Map.GetLength(0); i++)
            {
                for (int j = 0; j < Map.GetLength(1); j++)
                {
                    TransformComponent tempTransform = new TransformComponent(i, j);
                    MapTest[i,j] = new MapStruct(tempTransform, Cell.Walkable);
                }
            }

            MapTest[0, 6].Cell = Cell.Wall;
            MapTest[1, 6].Cell = Cell.Wall;
            MapTest[2, 6].Cell = Cell.Wall;
            MapTest[3, 6].Cell = Cell.Wall;
            MapTest[4, 6].Cell = Cell.Wall;
            MapTest[5, 6].Cell = Cell.Wall;
        }
    }
}
