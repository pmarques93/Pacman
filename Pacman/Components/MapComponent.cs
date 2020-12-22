﻿namespace Pacman
{
    /// <summary>
    /// Map Component. Extends Component
    /// </summary>
    public class MapComponent: Component
    {
        // Map made of Transforms
        public TransformComponent[,] Map { get; }

        /// <summary>
        /// Constructor for MapComponent
        /// </summary>
        /// <param name="xDim">X size</param>
        /// <param name="yDim">Y size</param>
        public MapComponent(byte xDim, byte yDim)
        {
            Map = new TransformComponent[xDim, yDim];

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
                    Map[i, j] = new TransformComponent(
                        new ColliderComponent(Cell.Walkable), i, j);
                }
            }

            Map[6, 6] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 6, 6);
            Map[5, 6] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 5, 6);
            Map[4, 6] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 4, 6);
            Map[3, 6] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 3, 6);
            Map[2, 6] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 2, 6);
            Map[1, 6] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 1, 6);
            Map[0, 6] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 0, 6);
        }
    }
}
