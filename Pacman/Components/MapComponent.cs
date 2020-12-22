namespace Pacman
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

            Map[0, 0] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 0, 0);
            Map[0, 1] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 0, 1);
            Map[0, 2] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 0, 2);
            Map[0, 3] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 0, 3);
            Map[0, 4] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 0, 4);
            Map[0, 5] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 0, 5);
            Map[0, 6] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 0, 6);
            Map[0, 7] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 0, 7);
            Map[0, 8] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 0, 8);
            Map[0, 9] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 0, 9);
            Map[0, 19] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 0, 19);
            Map[0, 20] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 0, 20);
            Map[0, 21] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 0, 21);
            Map[0, 22] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 0, 22);
            Map[0, 23] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 0, 23);
            Map[0, 24] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 0, 24);
            Map[0, 25] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 0, 25);
            Map[0, 26] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 0, 26);
            Map[0, 27] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 0, 27);
            Map[0, 28] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 0, 28);
            Map[0, 29] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 0, 29);
            Map[0, 30] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 0, 30);

            Map[1, 0] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 1, 0);
            Map[1, 9] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 1, 9);
            Map[1, 19] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 1, 19);
            Map[1, 24] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 1, 24);
            Map[1, 25] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 1, 25);
            Map[1, 30] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 1, 30);

            Map[2, 0] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 2, 0);
            Map[2, 2] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 2, 2);
            Map[2, 3] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 2, 3);
            Map[2, 4] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 2, 4);
            Map[2, 6] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 2, 6);
            Map[2, 7] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 2, 7);
            Map[2, 9] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 2, 9);
            Map[2, 19] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 2, 19);
            Map[2, 21] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 2, 21);
            Map[2, 22] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 2, 22);
            Map[2, 24] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 2, 24);
            Map[2, 25] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 2, 25);
            Map[2, 27] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 2, 27);
            Map[2, 28] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 2, 28);
            Map[2, 30] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 2, 30);

            Map[3, 0] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 3, 0);
            Map[3, 2] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 3, 2);
            Map[3, 3] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 3, 3);
            Map[3, 4] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 3, 4);
            Map[3, 6] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 3, 6);
            Map[3, 7] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 3, 7);
            Map[3, 9] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 3, 9);
            Map[3, 19] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 3, 19);
            Map[3, 21] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 3, 21);
            Map[3, 22] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 3, 22);
            Map[3, 27] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 3, 27);
            Map[3, 28] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 3, 28);
            Map[3, 30] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 3, 30);

            Map[4, 0] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 4, 0);
            Map[4, 2] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 4, 2);
            Map[4, 3] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 4, 3);
            Map[4, 4] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 4, 4);
            Map[4, 6] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 4, 6);
            Map[4, 7] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 4, 7);
            Map[4, 9] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 4, 9);
            Map[4, 19] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 4, 19);
            Map[4, 21] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 4, 21);
            Map[4, 22] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 4, 22);
            Map[4, 23] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 4, 23);
            Map[4, 24] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 4, 24);
            Map[4, 25] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 4, 25);
            Map[4, 27] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 4, 27);
            Map[4, 28] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 4, 28);
            Map[4, 30] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 4, 30);

            Map[5, 0] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 5, 0);
            Map[5, 2] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 5, 2);
            Map[5, 3] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 5, 3);
            Map[5, 4] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 5, 4);
            Map[5, 6] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 5, 6);
            Map[5, 7] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 5, 7);
            Map[5, 9] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 5, 9);
            Map[5, 10] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 5, 10);
            Map[5, 11] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 5, 11);
            Map[5, 12] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 5, 12);
            Map[5, 13] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 5, 13);
            Map[5, 14] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 5, 14);
            Map[5, 15] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 5, 15);
            Map[5, 16] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 5, 16);
            Map[5, 17] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 5, 17);
            Map[5, 18] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 5, 18);
            Map[5, 19] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 5, 19);
            Map[5, 21] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 5, 21);
            Map[5, 22] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 5, 22);
            Map[5, 23] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 5, 23);
            Map[5, 24] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 5, 24);
            Map[5, 25] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 5, 25);
            Map[5, 27] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 5, 27);
            Map[5, 28] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 5, 28);
            Map[5, 30] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 5, 30);

            Map[6, 0] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 6, 0);
            Map[6, 27] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 6, 27);
            Map[6, 28] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 6, 28);
            Map[6, 30] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 6, 30);

            Map[7, 0] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 7, 0);
            Map[7, 2] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 7, 2);
            Map[7, 3] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 7, 3);
            Map[7, 4] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 7, 4);
            Map[7, 6] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 7, 6);
            Map[7, 7] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 7, 7);
            Map[7, 8] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 7, 8);
            Map[7, 9] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 7, 9);
            Map[7, 10] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 7, 10);
            Map[7, 11] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 7, 11);
            Map[7, 12] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 7, 12);
            Map[7, 13] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 7, 13);
            Map[7, 15] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 7, 15);
            Map[7, 16] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 7, 16);
            Map[7, 17] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 7, 17);
            Map[7, 18] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 7, 18);
            Map[7, 19] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 7, 19);
            Map[7, 13] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 7, 13);
            Map[7, 21] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 7, 21);
            Map[7, 22] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 7, 22);
            Map[7, 24] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 7, 24);
            Map[7, 25] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 7, 25);
            Map[7, 26] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 7, 26);
            Map[7, 27] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 7, 27);
            Map[7, 28] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 7, 28);
            Map[7, 30] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 7, 30);

            Map[8, 0] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 8, 0);
            Map[8, 2] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 8, 2);
            Map[8, 3] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 8, 3);
            Map[8, 4] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 8, 4);
            Map[8, 6] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 8, 6);
            Map[8, 7] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 8, 7);
            Map[8, 8] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 8, 8);
            Map[8, 9] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 8, 9);
            Map[8, 10] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 8, 10);
            Map[8, 11] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 8, 11);
            Map[8, 12] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 8, 12);
            Map[8, 13] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 8, 13);
            Map[8, 15] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 8, 15);
            Map[8, 16] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 8, 16);
            Map[8, 17] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 8, 17);
            Map[8, 18] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 8, 18);
            Map[8, 19] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 8, 19);
            Map[8, 13] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 8, 13);
            Map[8, 21] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 8, 21);
            Map[8, 22] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 8, 22);
            Map[8, 24] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 8, 24);
            Map[8, 25] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 8, 25);
            Map[8, 26] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 8, 26);
            Map[8, 27] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 8, 27);
            Map[8, 28] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 8, 28);
            Map[8, 30] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 8, 30);

            Map[9, 0] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 9, 0);
            Map[9, 2] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 9, 2);
            Map[9, 3] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 9, 3);
            Map[9, 4] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 9, 4);
            Map[9, 9] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 9, 9);
            Map[9, 10] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 9, 10);
            Map[9, 21] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 9, 21);
            Map[9, 22] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 9, 22);
            Map[9, 27] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 9, 27);
            Map[9, 28] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 9, 28);
            Map[9, 30] = new TransformComponent(
                        new ColliderComponent(Cell.Wall), 9, 30);

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
