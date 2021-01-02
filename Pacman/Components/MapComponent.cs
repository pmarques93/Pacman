namespace Pacman
{
    /// <summary>
    /// Map Component. Extends Component.
    /// </summary>
    public class MapComponent : Component
    {
        /// <summary>
        /// Gets or sets MapStructs to create a map.
        /// </summary>
        public MapStruct[,] Map { get; set; }

        /// <summary>
        /// Constructor for MapComponent.
        /// </summary>
        /// <param name="xDim">X size.</param>
        /// <param name="yDim">Y size.</param>
        public MapComponent(byte xDim, byte yDim)
        {
            Map = new MapStruct[xDim, yDim];

            CreatePacmanMap();
        }

        /// <summary>
        /// Creates map for pacman.
        /// </summary>
        private void CreatePacmanMap()
        {
            for (int i = 0; i < Map.GetLength(0); i++)
            {
                for (int j = 0; j < Map.GetLength(1); j++)
                {
                    Map[i, j] = new MapStruct(
                                        new TransformComponent(i, j),
                                        new ColliderComponent(Cell.Walkable));
                }
            }

            Map[0, 0] = new MapStruct(
                        new TransformComponent(0, 0),
                        new ColliderComponent(Cell.Wall));
            Map[0, 1] = new MapStruct(
                        new TransformComponent(0, 1),
                        new ColliderComponent(Cell.Wall));
            Map[0, 2] = new MapStruct(
                        new TransformComponent(0, 2),
                        new ColliderComponent(Cell.Wall));
            Map[0, 3] = new MapStruct(
                        new TransformComponent(0, 3),
                        new ColliderComponent(Cell.Wall));
            Map[0, 4] = new MapStruct(
                        new TransformComponent(0, 4),
                        new ColliderComponent(Cell.Wall));
            Map[0, 5] = new MapStruct(
                        new TransformComponent(0, 5),
                        new ColliderComponent(Cell.Wall));
            Map[0, 6] = new MapStruct(
                        new TransformComponent(0, 6),
                        new ColliderComponent(Cell.Wall));
            Map[0, 7] = new MapStruct(
                        new TransformComponent(0, 7),
                        new ColliderComponent(Cell.Wall));
            Map[0, 8] = new MapStruct(
                        new TransformComponent(0, 8),
                        new ColliderComponent(Cell.Wall));
            Map[0, 9] = new MapStruct(
                        new TransformComponent(0, 9),
                        new ColliderComponent(Cell.Wall));
            Map[0, 10] = new MapStruct(
                        new TransformComponent(0, 10),
                        new ColliderComponent(Cell.Wall));
            Map[0, 11] = new MapStruct(
                        new TransformComponent(0, 11),
                        new ColliderComponent(Cell.Wall));
            Map[0, 12] = new MapStruct(
                        new TransformComponent(0, 12),
                        new ColliderComponent(Cell.Wall));
            Map[0, 13] = new MapStruct(
                        new TransformComponent(0, 13),
                        new ColliderComponent(Cell.Wall));
            Map[0, 14] = new MapStruct(
                        new TransformComponent(0, 14),
                        new ColliderComponent(Cell.Wall));
            Map[0, 15] = new MapStruct(
                        new TransformComponent(0, 15),
                        new ColliderComponent(Cell.Wall));
            Map[0, 16] = new MapStruct(
                        new TransformComponent(0, 16),
                        new ColliderComponent(Cell.Wall));
            Map[0, 17] = new MapStruct(
                        new TransformComponent(0, 17),
                        new ColliderComponent(Cell.Wall));
            Map[0, 18] = new MapStruct(
                        new TransformComponent(0, 18),
                        new ColliderComponent(Cell.Wall));
            Map[0, 19] = new MapStruct(
                        new TransformComponent(0, 19),
                        new ColliderComponent(Cell.Wall));
            Map[0, 20] = new MapStruct(
                        new TransformComponent(0, 20),
                        new ColliderComponent(Cell.Wall));
            Map[0, 21] = new MapStruct(
                        new TransformComponent(0, 21),
                        new ColliderComponent(Cell.Wall));
            Map[0, 22] = new MapStruct(
                        new TransformComponent(0, 22),
                        new ColliderComponent(Cell.Wall));
            Map[0, 23] = new MapStruct(
                        new TransformComponent(0, 23),
                        new ColliderComponent(Cell.Wall));
            Map[0, 24] = new MapStruct(
                        new TransformComponent(0, 24),
                        new ColliderComponent(Cell.Wall));
            Map[0, 25] = new MapStruct(
                        new TransformComponent(0, 25),
                        new ColliderComponent(Cell.Wall));
            Map[0, 26] = new MapStruct(
                        new TransformComponent(0, 26),
                        new ColliderComponent(Cell.Wall));
            Map[0, 27] = new MapStruct(
                        new TransformComponent(0, 27),
                        new ColliderComponent(Cell.Wall));
            Map[0, 28] = new MapStruct(
                        new TransformComponent(0, 28),
                        new ColliderComponent(Cell.Wall));
            Map[0, 29] = new MapStruct(
                        new TransformComponent(0, 29),
                        new ColliderComponent(Cell.Wall));
            Map[0, 30] = new MapStruct(
                        new TransformComponent(0, 30),
                        new ColliderComponent(Cell.Wall));

            Map[1, 0] = new MapStruct(
                        new TransformComponent(1, 0),
                        new ColliderComponent(Cell.Wall));
            Map[1, 9] = new MapStruct(
                        new TransformComponent(1, 9),
                        new ColliderComponent(Cell.Wall));
            Map[1, 10] = new MapStruct(
                        new TransformComponent(1, 10),
                        new ColliderComponent(Cell.Wall));
            Map[1, 11] = new MapStruct(
                        new TransformComponent(1, 11),
                        new ColliderComponent(Cell.Wall));
            Map[1, 12] = new MapStruct(
                        new TransformComponent(1, 12),
                        new ColliderComponent(Cell.Wall));
            Map[1, 13] = new MapStruct(
                        new TransformComponent(1, 13),
                        new ColliderComponent(Cell.Wall));
            Map[1, 14] = new MapStruct(
                        new TransformComponent(1, 14),
                        new ColliderComponent(Cell.Wall));
            Map[1, 15] = new MapStruct(
                        new TransformComponent(1, 15),
                        new ColliderComponent(Cell.Wall));
            Map[1, 16] = new MapStruct(
                        new TransformComponent(1, 16),
                        new ColliderComponent(Cell.Wall));
            Map[1, 17] = new MapStruct(
                        new TransformComponent(1, 17),
                        new ColliderComponent(Cell.Wall));
            Map[1, 18] = new MapStruct(
                        new TransformComponent(1, 18),
                        new ColliderComponent(Cell.Wall));
            Map[1, 19] = new MapStruct(
                        new TransformComponent(1, 19),
                        new ColliderComponent(Cell.Wall));
            Map[1, 24] = new MapStruct(
                        new TransformComponent(1, 24),
                        new ColliderComponent(Cell.Wall));
            Map[1, 25] = new MapStruct(
                        new TransformComponent(1, 25),
                        new ColliderComponent(Cell.Wall));
            Map[1, 30] = new MapStruct(
                        new TransformComponent(1, 30),
                        new ColliderComponent(Cell.Wall));

            Map[2, 0] = new MapStruct(
                        new TransformComponent(2, 0),
                        new ColliderComponent(Cell.Wall));
            Map[2, 2] = new MapStruct(
                        new TransformComponent(2, 2),
                        new ColliderComponent(Cell.Wall));
            Map[2, 3] = new MapStruct(
                        new TransformComponent(2, 3),
                        new ColliderComponent(Cell.Wall));
            Map[2, 4] = new MapStruct(
                        new TransformComponent(2, 4),
                        new ColliderComponent(Cell.Wall));
            Map[2, 6] = new MapStruct(
                        new TransformComponent(2, 6),
                        new ColliderComponent(Cell.Wall));
            Map[2, 7] = new MapStruct(
                        new TransformComponent(2, 7),
                        new ColliderComponent(Cell.Wall));
            Map[2, 9] = new MapStruct(
                        new TransformComponent(2, 9),
                        new ColliderComponent(Cell.Wall));
            Map[2, 10] = new MapStruct(
                        new TransformComponent(2, 10),
                        new ColliderComponent(Cell.Wall));
            Map[2, 11] = new MapStruct(
                        new TransformComponent(2, 11),
                        new ColliderComponent(Cell.Wall));
            Map[2, 12] = new MapStruct(
                        new TransformComponent(2, 12),
                        new ColliderComponent(Cell.Wall));
            Map[2, 13] = new MapStruct(
                        new TransformComponent(2, 13),
                        new ColliderComponent(Cell.Wall));
            Map[2, 14] = new MapStruct(
                        new TransformComponent(2, 14),
                        new ColliderComponent(Cell.Wall));
            Map[2, 15] = new MapStruct(
                        new TransformComponent(2, 15),
                        new ColliderComponent(Cell.Wall));
            Map[2, 16] = new MapStruct(
                        new TransformComponent(2, 16),
                        new ColliderComponent(Cell.Wall));
            Map[2, 17] = new MapStruct(
                        new TransformComponent(2, 17),
                        new ColliderComponent(Cell.Wall));
            Map[2, 18] = new MapStruct(
                        new TransformComponent(2, 18),
                        new ColliderComponent(Cell.Wall));
            Map[2, 19] = new MapStruct(
                        new TransformComponent(2, 19),
                        new ColliderComponent(Cell.Wall));
            Map[2, 21] = new MapStruct(
                        new TransformComponent(2, 21),
                        new ColliderComponent(Cell.Wall));
            Map[2, 22] = new MapStruct(
                        new TransformComponent(2, 22),
                        new ColliderComponent(Cell.Wall));
            Map[2, 24] = new MapStruct(
                        new TransformComponent(2, 24),
                        new ColliderComponent(Cell.Wall));
            Map[2, 25] = new MapStruct(
                        new TransformComponent(2, 25),
                        new ColliderComponent(Cell.Wall));
            Map[2, 27] = new MapStruct(
                        new TransformComponent(2, 27),
                        new ColliderComponent(Cell.Wall));
            Map[2, 28] = new MapStruct(
                        new TransformComponent(2, 28),
                        new ColliderComponent(Cell.Wall));
            Map[2, 30] = new MapStruct(
                        new TransformComponent(2, 30),
                        new ColliderComponent(Cell.Wall));

            Map[3, 0] = new MapStruct(
                        new TransformComponent(3, 0),
                        new ColliderComponent(Cell.Wall));
            Map[3, 2] = new MapStruct(
                        new TransformComponent(3, 2),
                        new ColliderComponent(Cell.Wall));
            Map[3, 3] = new MapStruct(
                        new TransformComponent(3, 3),
                        new ColliderComponent(Cell.Wall));
            Map[3, 4] = new MapStruct(
                        new TransformComponent(3, 4),
                        new ColliderComponent(Cell.Wall));
            Map[3, 6] = new MapStruct(
                        new TransformComponent(3, 6),
                        new ColliderComponent(Cell.Wall));
            Map[3, 7] = new MapStruct(
                        new TransformComponent(3, 7),
                        new ColliderComponent(Cell.Wall));
            Map[3, 9] = new MapStruct(
                        new TransformComponent(3, 9),
                        new ColliderComponent(Cell.Wall));
            Map[3, 10] = new MapStruct(
                        new TransformComponent(3, 10),
                        new ColliderComponent(Cell.Wall));
            Map[3, 11] = new MapStruct(
                        new TransformComponent(3, 11),
                        new ColliderComponent(Cell.Wall));
            Map[3, 12] = new MapStruct(
                        new TransformComponent(3, 12),
                        new ColliderComponent(Cell.Wall));
            Map[3, 13] = new MapStruct(
                        new TransformComponent(3, 13),
                        new ColliderComponent(Cell.Wall));
            Map[3, 14] = new MapStruct(
                        new TransformComponent(3, 14),
                        new ColliderComponent(Cell.Wall));
            Map[3, 15] = new MapStruct(
                        new TransformComponent(3, 15),
                        new ColliderComponent(Cell.Wall));
            Map[3, 16] = new MapStruct(
                        new TransformComponent(3, 16),
                        new ColliderComponent(Cell.Wall));
            Map[3, 17] = new MapStruct(
                        new TransformComponent(3, 17),
                        new ColliderComponent(Cell.Wall));
            Map[3, 18] = new MapStruct(
                        new TransformComponent(3, 18),
                        new ColliderComponent(Cell.Wall));
            Map[3, 19] = new MapStruct(
                        new TransformComponent(3, 19),
                        new ColliderComponent(Cell.Wall));
            Map[3, 21] = new MapStruct(
                        new TransformComponent(3, 21),
                        new ColliderComponent(Cell.Wall));
            Map[3, 22] = new MapStruct(
                        new TransformComponent(3, 22),
                        new ColliderComponent(Cell.Wall));
            Map[3, 27] = new MapStruct(
                        new TransformComponent(3, 27),
                        new ColliderComponent(Cell.Wall));
            Map[3, 28] = new MapStruct(
                        new TransformComponent(3, 28),
                        new ColliderComponent(Cell.Wall));
            Map[3, 30] = new MapStruct(
                        new TransformComponent(3, 30),
                        new ColliderComponent(Cell.Wall));

            Map[4, 0] = new MapStruct(
                        new TransformComponent(4, 0),
                        new ColliderComponent(Cell.Wall));
            Map[4, 2] = new MapStruct(
                        new TransformComponent(4, 2),
                        new ColliderComponent(Cell.Wall));
            Map[4, 3] = new MapStruct(
                        new TransformComponent(4, 3),
                        new ColliderComponent(Cell.Wall));
            Map[4, 4] = new MapStruct(
                        new TransformComponent(4, 4),
                        new ColliderComponent(Cell.Wall));
            Map[4, 6] = new MapStruct(
                        new TransformComponent(4, 6),
                        new ColliderComponent(Cell.Wall));
            Map[4, 7] = new MapStruct(
                        new TransformComponent(4, 7),
                        new ColliderComponent(Cell.Wall));
            Map[4, 9] = new MapStruct(
                        new TransformComponent(4, 9),
                        new ColliderComponent(Cell.Wall));
            Map[4, 10] = new MapStruct(
                        new TransformComponent(4, 10),
                        new ColliderComponent(Cell.Wall));
            Map[4, 11] = new MapStruct(
                        new TransformComponent(4, 11),
                        new ColliderComponent(Cell.Wall));
            Map[4, 12] = new MapStruct(
                        new TransformComponent(4, 12),
                        new ColliderComponent(Cell.Wall));
            Map[4, 13] = new MapStruct(
                        new TransformComponent(4, 13),
                        new ColliderComponent(Cell.Wall));
            Map[4, 14] = new MapStruct(
                        new TransformComponent(4, 14),
                        new ColliderComponent(Cell.Wall));
            Map[4, 15] = new MapStruct(
                        new TransformComponent(4, 15),
                        new ColliderComponent(Cell.Wall));
            Map[4, 16] = new MapStruct(
                        new TransformComponent(4, 16),
                        new ColliderComponent(Cell.Wall));
            Map[4, 17] = new MapStruct(
                        new TransformComponent(4, 17),
                        new ColliderComponent(Cell.Wall));
            Map[4, 18] = new MapStruct(
                        new TransformComponent(4, 18),
                        new ColliderComponent(Cell.Wall));
            Map[4, 19] = new MapStruct(
                        new TransformComponent(4, 19),
                        new ColliderComponent(Cell.Wall));
            Map[4, 21] = new MapStruct(
                        new TransformComponent(4, 21),
                        new ColliderComponent(Cell.Wall));
            Map[4, 22] = new MapStruct(
                        new TransformComponent(4, 22),
                        new ColliderComponent(Cell.Wall));
            Map[4, 23] = new MapStruct(
                        new TransformComponent(4, 23),
                        new ColliderComponent(Cell.Wall));
            Map[4, 24] = new MapStruct(
                        new TransformComponent(4, 24),
                        new ColliderComponent(Cell.Wall));
            Map[4, 25] = new MapStruct(
                        new TransformComponent(4, 25),
                        new ColliderComponent(Cell.Wall));
            Map[4, 27] = new MapStruct(
                        new TransformComponent(4, 27),
                        new ColliderComponent(Cell.Wall));
            Map[4, 28] = new MapStruct(
                        new TransformComponent(4, 28),
                        new ColliderComponent(Cell.Wall));
            Map[4, 30] = new MapStruct(
                        new TransformComponent(4, 30),
                        new ColliderComponent(Cell.Wall));

            Map[5, 0] = new MapStruct(
                        new TransformComponent(5, 0),
                        new ColliderComponent(Cell.Wall));
            Map[5, 2] = new MapStruct(
                        new TransformComponent(5, 2),
                        new ColliderComponent(Cell.Wall));
            Map[5, 3] = new MapStruct(
                        new TransformComponent(5, 3),
                        new ColliderComponent(Cell.Wall));
            Map[5, 4] = new MapStruct(
                        new TransformComponent(5, 4),
                        new ColliderComponent(Cell.Wall));
            Map[5, 6] = new MapStruct(
                        new TransformComponent(5, 6),
                        new ColliderComponent(Cell.Wall));
            Map[5, 7] = new MapStruct(
                        new TransformComponent(5, 7),
                        new ColliderComponent(Cell.Wall));
            Map[5, 9] = new MapStruct(
                        new TransformComponent(5, 9),
                        new ColliderComponent(Cell.Wall));
            Map[5, 10] = new MapStruct(
                        new TransformComponent(5, 10),
                        new ColliderComponent(Cell.Wall));
            Map[5, 11] = new MapStruct(
                        new TransformComponent(5, 11),
                        new ColliderComponent(Cell.Wall));
            Map[5, 12] = new MapStruct(
                        new TransformComponent(5, 12),
                        new ColliderComponent(Cell.Wall));
            Map[5, 13] = new MapStruct(
                        new TransformComponent(5, 13),
                        new ColliderComponent(Cell.Wall));
            Map[5, 14] = new MapStruct(
                        new TransformComponent(5, 14),
                        new ColliderComponent(Cell.Wall));
            Map[5, 15] = new MapStruct(
                        new TransformComponent(5, 15),
                        new ColliderComponent(Cell.Wall));
            Map[5, 16] = new MapStruct(
                        new TransformComponent(5, 16),
                        new ColliderComponent(Cell.Wall));
            Map[5, 17] = new MapStruct(
                        new TransformComponent(5, 17),
                        new ColliderComponent(Cell.Wall));
            Map[5, 18] = new MapStruct(
                        new TransformComponent(5, 18),
                        new ColliderComponent(Cell.Wall));
            Map[5, 19] = new MapStruct(
                        new TransformComponent(5, 19),
                        new ColliderComponent(Cell.Wall));
            Map[5, 21] = new MapStruct(
                        new TransformComponent(5, 21),
                        new ColliderComponent(Cell.Wall));
            Map[5, 22] = new MapStruct(
                        new TransformComponent(5, 22),
                        new ColliderComponent(Cell.Wall));
            Map[5, 23] = new MapStruct(
                        new TransformComponent(5, 23),
                        new ColliderComponent(Cell.Wall));
            Map[5, 24] = new MapStruct(
                        new TransformComponent(5, 24),
                        new ColliderComponent(Cell.Wall));
            Map[5, 25] = new MapStruct(
                        new TransformComponent(5, 25),
                        new ColliderComponent(Cell.Wall));
            Map[5, 27] = new MapStruct(
                        new TransformComponent(5, 27),
                        new ColliderComponent(Cell.Wall));
            Map[5, 28] = new MapStruct(
                        new TransformComponent(5, 28),
                        new ColliderComponent(Cell.Wall));
            Map[5, 30] = new MapStruct(
                        new TransformComponent(5, 30),
                        new ColliderComponent(Cell.Wall));

            Map[6, 0] = new MapStruct(
                        new TransformComponent(6, 0),
                        new ColliderComponent(Cell.Wall));
            Map[6, 27] = new MapStruct(
                        new TransformComponent(6, 27),
                        new ColliderComponent(Cell.Wall));
            Map[6, 28] = new MapStruct(
                        new TransformComponent(6, 28),
                        new ColliderComponent(Cell.Wall));
            Map[6, 30] = new MapStruct(
                        new TransformComponent(6, 30),
                        new ColliderComponent(Cell.Wall));

            Map[7, 0] = new MapStruct(
                        new TransformComponent(7, 0),
                        new ColliderComponent(Cell.Wall));
            Map[7, 2] = new MapStruct(
                        new TransformComponent(7, 2),
                        new ColliderComponent(Cell.Wall));
            Map[7, 3] = new MapStruct(
                        new TransformComponent(7, 3),
                        new ColliderComponent(Cell.Wall));
            Map[7, 4] = new MapStruct(
                        new TransformComponent(7, 4),
                        new ColliderComponent(Cell.Wall));
            Map[7, 6] = new MapStruct(
                        new TransformComponent(7, 6),
                        new ColliderComponent(Cell.Wall));
            Map[7, 7] = new MapStruct(
                        new TransformComponent(7, 7),
                        new ColliderComponent(Cell.Wall));
            Map[7, 8] = new MapStruct(
                        new TransformComponent(7, 8),
                        new ColliderComponent(Cell.Wall));
            Map[7, 9] = new MapStruct(
                        new TransformComponent(7, 9),
                        new ColliderComponent(Cell.Wall));
            Map[7, 10] = new MapStruct(
                        new TransformComponent(7, 10),
                        new ColliderComponent(Cell.Wall));
            Map[7, 11] = new MapStruct(
                        new TransformComponent(7, 11),
                        new ColliderComponent(Cell.Wall));
            Map[7, 12] = new MapStruct(
                        new TransformComponent(7, 12),
                        new ColliderComponent(Cell.Wall));
            Map[7, 13] = new MapStruct(
                        new TransformComponent(7, 13),
                        new ColliderComponent(Cell.Wall));
            Map[7, 15] = new MapStruct(
                        new TransformComponent(7, 15),
                        new ColliderComponent(Cell.Wall));
            Map[7, 16] = new MapStruct(
                        new TransformComponent(7, 16),
                        new ColliderComponent(Cell.Wall));
            Map[7, 17] = new MapStruct(
                        new TransformComponent(7, 17),
                        new ColliderComponent(Cell.Wall));
            Map[7, 18] = new MapStruct(
                        new TransformComponent(7, 18),
                        new ColliderComponent(Cell.Wall));
            Map[7, 19] = new MapStruct(
                        new TransformComponent(7, 19),
                        new ColliderComponent(Cell.Wall));
            Map[7, 13] = new MapStruct(
                        new TransformComponent(7, 13),
                        new ColliderComponent(Cell.Wall));
            Map[7, 21] = new MapStruct(
                        new TransformComponent(7, 21),
                        new ColliderComponent(Cell.Wall));
            Map[7, 22] = new MapStruct(
                        new TransformComponent(7, 22),
                        new ColliderComponent(Cell.Wall));
            Map[7, 24] = new MapStruct(
                        new TransformComponent(7, 24),
                        new ColliderComponent(Cell.Wall));
            Map[7, 25] = new MapStruct(
                        new TransformComponent(7, 25),
                        new ColliderComponent(Cell.Wall));
            Map[7, 26] = new MapStruct(
                        new TransformComponent(7, 26),
                        new ColliderComponent(Cell.Wall));
            Map[7, 27] = new MapStruct(
                        new TransformComponent(7, 27),
                        new ColliderComponent(Cell.Wall));
            Map[7, 28] = new MapStruct(
                        new TransformComponent(7, 28),
                        new ColliderComponent(Cell.Wall));
            Map[7, 30] = new MapStruct(
                        new TransformComponent(7, 30),
                        new ColliderComponent(Cell.Wall));

            Map[8, 0] = new MapStruct(
                        new TransformComponent(8, 0),
                        new ColliderComponent(Cell.Wall));
            Map[8, 2] = new MapStruct(
                        new TransformComponent(8, 2),
                        new ColliderComponent(Cell.Wall));
            Map[8, 3] = new MapStruct(
                        new TransformComponent(8, 3),
                        new ColliderComponent(Cell.Wall));
            Map[8, 4] = new MapStruct(
                        new TransformComponent(8, 4),
                        new ColliderComponent(Cell.Wall));
            Map[8, 6] = new MapStruct(
                        new TransformComponent(8, 6),
                        new ColliderComponent(Cell.Wall));
            Map[8, 7] = new MapStruct(
                        new TransformComponent(8, 7),
                        new ColliderComponent(Cell.Wall));
            Map[8, 8] = new MapStruct(
                        new TransformComponent(8, 8),
                        new ColliderComponent(Cell.Wall));
            Map[8, 9] = new MapStruct(
                        new TransformComponent(8, 9),
                        new ColliderComponent(Cell.Wall));
            Map[8, 10] = new MapStruct(
                        new TransformComponent(8, 10),
                        new ColliderComponent(Cell.Wall));
            Map[8, 11] = new MapStruct(
                        new TransformComponent(8, 11),
                        new ColliderComponent(Cell.Wall));
            Map[8, 12] = new MapStruct(
                        new TransformComponent(8, 12),
                        new ColliderComponent(Cell.Wall));
            Map[8, 13] = new MapStruct(
                        new TransformComponent(8, 13),
                        new ColliderComponent(Cell.Wall));
            Map[8, 15] = new MapStruct(
                        new TransformComponent(8, 15),
                        new ColliderComponent(Cell.Wall));
            Map[8, 16] = new MapStruct(
                        new TransformComponent(8, 16),
                        new ColliderComponent(Cell.Wall));
            Map[8, 17] = new MapStruct(
                        new TransformComponent(8, 17),
                        new ColliderComponent(Cell.Wall));
            Map[8, 18] = new MapStruct(
                        new TransformComponent(8, 18),
                        new ColliderComponent(Cell.Wall));
            Map[8, 19] = new MapStruct(
                        new TransformComponent(8, 19),
                        new ColliderComponent(Cell.Wall));
            Map[8, 13] = new MapStruct(
                        new TransformComponent(8, 13),
                        new ColliderComponent(Cell.Wall));
            Map[8, 21] = new MapStruct(
                        new TransformComponent(8, 21),
                        new ColliderComponent(Cell.Wall));
            Map[8, 22] = new MapStruct(
                        new TransformComponent(8, 22),
                        new ColliderComponent(Cell.Wall));
            Map[8, 24] = new MapStruct(
                        new TransformComponent(8, 24),
                        new ColliderComponent(Cell.Wall));
            Map[8, 25] = new MapStruct(
                        new TransformComponent(8, 25),
                        new ColliderComponent(Cell.Wall));
            Map[8, 26] = new MapStruct(
                        new TransformComponent(8, 26),
                        new ColliderComponent(Cell.Wall));
            Map[8, 27] = new MapStruct(
                        new TransformComponent(8, 27),
                        new ColliderComponent(Cell.Wall));
            Map[8, 28] = new MapStruct(
                        new TransformComponent(8, 28),
                        new ColliderComponent(Cell.Wall));
            Map[8, 30] = new MapStruct(
                        new TransformComponent(8, 30),
                        new ColliderComponent(Cell.Wall));

            Map[9, 0] = new MapStruct(
                        new TransformComponent(9, 0),
                        new ColliderComponent(Cell.Wall));
            Map[9, 2] = new MapStruct(
                        new TransformComponent(9, 2),
                        new ColliderComponent(Cell.Wall));
            Map[9, 3] = new MapStruct(
                        new TransformComponent(9, 3),
                        new ColliderComponent(Cell.Wall));
            Map[9, 4] = new MapStruct(
                        new TransformComponent(9, 4),
                        new ColliderComponent(Cell.Wall));
            Map[9, 9] = new MapStruct(
                        new TransformComponent(9, 9),
                        new ColliderComponent(Cell.Wall));
            Map[9, 10] = new MapStruct(
                        new TransformComponent(9, 10),
                        new ColliderComponent(Cell.Wall));
            Map[9, 21] = new MapStruct(
                        new TransformComponent(9, 21),
                        new ColliderComponent(Cell.Wall));
            Map[9, 22] = new MapStruct(
                        new TransformComponent(9, 22),
                        new ColliderComponent(Cell.Wall));
            Map[9, 27] = new MapStruct(
                        new TransformComponent(9, 27),
                        new ColliderComponent(Cell.Wall));
            Map[9, 28] = new MapStruct(
                        new TransformComponent(9, 28),
                        new ColliderComponent(Cell.Wall));
            Map[9, 30] = new MapStruct(
                        new TransformComponent(9, 30),
                        new ColliderComponent(Cell.Wall));

            Map[10, 0] = new MapStruct(
                        new TransformComponent(10, 0),
                        new ColliderComponent(Cell.Wall));
            Map[10, 2] = new MapStruct(
                        new TransformComponent(10, 2),
                        new ColliderComponent(Cell.Wall));
            Map[10, 3] = new MapStruct(
                        new TransformComponent(10, 3),
                        new ColliderComponent(Cell.Wall));
            Map[10, 4] = new MapStruct(
                        new TransformComponent(10, 4),
                        new ColliderComponent(Cell.Wall));
            Map[10, 6] = new MapStruct(
                        new TransformComponent(10, 6),
                        new ColliderComponent(Cell.Wall));
            Map[10, 7] = new MapStruct(
                        new TransformComponent(10, 7),
                        new ColliderComponent(Cell.Wall));
            Map[10, 9] = new MapStruct(
                        new TransformComponent(10, 9),
                        new ColliderComponent(Cell.Wall));
            Map[10, 10] = new MapStruct(
                        new TransformComponent(10, 10),
                        new ColliderComponent(Cell.Wall));
            Map[10, 12] = new MapStruct(
                        new TransformComponent(10, 12),
                        new ColliderComponent(Cell.Wall));
            Map[10, 13] = new MapStruct(
                        new TransformComponent(10, 13),
                        new ColliderComponent(Cell.Wall));
            Map[10, 14] = new MapStruct(
                        new TransformComponent(10, 14),
                        new ColliderComponent(Cell.Wall));
            Map[10, 15] = new MapStruct(
                        new TransformComponent(10, 15),
                        new ColliderComponent(Cell.Wall));
            Map[10, 16] = new MapStruct(
                        new TransformComponent(10, 16),
                        new ColliderComponent(Cell.Wall));
            Map[10, 18] = new MapStruct(
                        new TransformComponent(10, 18),
                        new ColliderComponent(Cell.Wall));
            Map[10, 19] = new MapStruct(
                        new TransformComponent(10, 19),
                        new ColliderComponent(Cell.Wall));
            Map[10, 21] = new MapStruct(
                        new TransformComponent(10, 21),
                        new ColliderComponent(Cell.Wall));
            Map[10, 22] = new MapStruct(
                        new TransformComponent(10, 22),
                        new ColliderComponent(Cell.Wall));
            Map[10, 24] = new MapStruct(
                        new TransformComponent(10, 24),
                        new ColliderComponent(Cell.Wall));
            Map[10, 25] = new MapStruct(
                        new TransformComponent(10, 25),
                        new ColliderComponent(Cell.Wall));
            Map[10, 27] = new MapStruct(
                        new TransformComponent(10, 27),
                        new ColliderComponent(Cell.Wall));
            Map[10, 28] = new MapStruct(
                        new TransformComponent(10, 28),
                        new ColliderComponent(Cell.Wall));
            Map[10, 30] = new MapStruct(
                        new TransformComponent(10, 30),
                        new ColliderComponent(Cell.Wall));

            Map[11, 0] = new MapStruct(
                        new TransformComponent(11, 0),
                        new ColliderComponent(Cell.Wall));
            Map[11, 2] = new MapStruct(
                        new TransformComponent(11, 2),
                        new ColliderComponent(Cell.Wall));
            Map[11, 3] = new MapStruct(
                        new TransformComponent(11, 3),
                        new ColliderComponent(Cell.Wall));
            Map[11, 4] = new MapStruct(
                        new TransformComponent(11, 4),
                        new ColliderComponent(Cell.Wall));
            Map[11, 6] = new MapStruct(
                        new TransformComponent(11, 6),
                        new ColliderComponent(Cell.Wall));
            Map[11, 7] = new MapStruct(
                        new TransformComponent(11, 7),
                        new ColliderComponent(Cell.Wall));
            Map[11, 9] = new MapStruct(
                        new TransformComponent(11, 9),
                        new ColliderComponent(Cell.Wall));
            Map[11, 10] = new MapStruct(
                        new TransformComponent(11, 10),
                        new ColliderComponent(Cell.Wall));
            Map[11, 12] = new MapStruct(
                        new TransformComponent(11, 12),
                        new ColliderComponent(Cell.Wall));
            Map[11, 16] = new MapStruct(
                        new TransformComponent(11, 16),
                        new ColliderComponent(Cell.Wall));
            Map[11, 18] = new MapStruct(
                        new TransformComponent(11, 18),
                        new ColliderComponent(Cell.Wall));
            Map[11, 19] = new MapStruct(
                        new TransformComponent(11, 19),
                        new ColliderComponent(Cell.Wall));
            Map[11, 21] = new MapStruct(
                        new TransformComponent(11, 21),
                        new ColliderComponent(Cell.Wall));
            Map[11, 22] = new MapStruct(
                        new TransformComponent(11, 22),
                        new ColliderComponent(Cell.Wall));
            Map[11, 24] = new MapStruct(
                        new TransformComponent(11, 24),
                        new ColliderComponent(Cell.Wall));
            Map[11, 25] = new MapStruct(
                        new TransformComponent(11, 25),
                        new ColliderComponent(Cell.Wall));
            Map[11, 27] = new MapStruct(
                        new TransformComponent(11, 27),
                        new ColliderComponent(Cell.Wall));
            Map[11, 28] = new MapStruct(
                        new TransformComponent(11, 28),
                        new ColliderComponent(Cell.Wall));
            Map[11, 30] = new MapStruct(
                        new TransformComponent(11, 30),
                        new ColliderComponent(Cell.Wall));

            Map[12, 0] = new MapStruct(
                        new TransformComponent(12, 0),
                        new ColliderComponent(Cell.Wall));
            Map[12, 6] = new MapStruct(
                        new TransformComponent(12, 6),
                        new ColliderComponent(Cell.Wall));
            Map[12, 7] = new MapStruct(
                        new TransformComponent(12, 7),
                        new ColliderComponent(Cell.Wall));
            Map[12, 12] = new MapStruct(
                        new TransformComponent(12, 12),
                        new ColliderComponent(Cell.Wall));
            Map[12, 16] = new MapStruct(
                        new TransformComponent(12, 16),
                        new ColliderComponent(Cell.Wall));
            Map[12, 18] = new MapStruct(
                        new TransformComponent(12, 18),
                        new ColliderComponent(Cell.Wall));
            Map[12, 19] = new MapStruct(
                        new TransformComponent(12, 19),
                        new ColliderComponent(Cell.Wall));
            Map[12, 24] = new MapStruct(
                        new TransformComponent(12, 24),
                        new ColliderComponent(Cell.Wall));
            Map[12, 25] = new MapStruct(
                        new TransformComponent(12, 25),
                        new ColliderComponent(Cell.Wall));
            Map[12, 30] = new MapStruct(
                        new TransformComponent(12, 30),
                        new ColliderComponent(Cell.Wall));

            Map[13, 0] = new MapStruct(
                        new TransformComponent(13, 0),
                        new ColliderComponent(Cell.Wall));
            Map[13, 1] = new MapStruct(
                        new TransformComponent(13, 1),
                        new ColliderComponent(Cell.Wall));
            Map[13, 2] = new MapStruct(
                        new TransformComponent(13, 2),
                        new ColliderComponent(Cell.Wall));
            Map[13, 3] = new MapStruct(
                        new TransformComponent(13, 3),
                        new ColliderComponent(Cell.Wall));
            Map[13, 4] = new MapStruct(
                        new TransformComponent(13, 4),
                        new ColliderComponent(Cell.Wall));
            Map[13, 6] = new MapStruct(
                        new TransformComponent(13, 6),
                        new ColliderComponent(Cell.Wall));
            Map[13, 7] = new MapStruct(
                        new TransformComponent(13, 7),
                        new ColliderComponent(Cell.Wall));
            Map[13, 8] = new MapStruct(
                        new TransformComponent(13, 8),
                        new ColliderComponent(Cell.Wall));
            Map[13, 9] = new MapStruct(
                        new TransformComponent(13, 9),
                        new ColliderComponent(Cell.Wall));
            Map[13, 10] = new MapStruct(
                        new TransformComponent(13, 10),
                        new ColliderComponent(Cell.Wall));
            Map[13, 16] = new MapStruct(
                        new TransformComponent(13, 16),
                        new ColliderComponent(Cell.Wall));
            Map[13, 18] = new MapStruct(
                        new TransformComponent(13, 18),
                        new ColliderComponent(Cell.Wall));
            Map[13, 19] = new MapStruct(
                        new TransformComponent(13, 19),
                        new ColliderComponent(Cell.Wall));
            Map[13, 20] = new MapStruct(
                        new TransformComponent(13, 19),
                        new ColliderComponent(Cell.Wall));
            Map[13, 21] = new MapStruct(
                        new TransformComponent(13, 21),
                        new ColliderComponent(Cell.Wall));
            Map[13, 22] = new MapStruct(
                        new TransformComponent(13, 22),
                        new ColliderComponent(Cell.Wall));
            Map[13, 24] = new MapStruct(
                        new TransformComponent(13, 24),
                        new ColliderComponent(Cell.Wall));
            Map[13, 25] = new MapStruct(
                        new TransformComponent(13, 25),
                        new ColliderComponent(Cell.Wall));
            Map[13, 26] = new MapStruct(
                        new TransformComponent(13, 26),
                        new ColliderComponent(Cell.Wall));
            Map[13, 27] = new MapStruct(
                        new TransformComponent(13, 27),
                        new ColliderComponent(Cell.Wall));
            Map[13, 28] = new MapStruct(
                        new TransformComponent(13, 28),
                        new ColliderComponent(Cell.Wall));
            Map[13, 30] = new MapStruct(
                        new TransformComponent(13, 30),
                        new ColliderComponent(Cell.Wall));

            Map[14, 0] = new MapStruct(
                        new TransformComponent(14, 0),
                        new ColliderComponent(Cell.Wall));
            Map[14, 1] = new MapStruct(
                        new TransformComponent(14, 1),
                        new ColliderComponent(Cell.Wall));
            Map[14, 2] = new MapStruct(
                        new TransformComponent(14, 2),
                        new ColliderComponent(Cell.Wall));
            Map[14, 3] = new MapStruct(
                        new TransformComponent(14, 3),
                        new ColliderComponent(Cell.Wall));
            Map[14, 4] = new MapStruct(
                        new TransformComponent(14, 4),
                        new ColliderComponent(Cell.Wall));
            Map[14, 6] = new MapStruct(
                        new TransformComponent(14, 6),
                        new ColliderComponent(Cell.Wall));
            Map[14, 7] = new MapStruct(
                        new TransformComponent(14, 7),
                        new ColliderComponent(Cell.Wall));
            Map[14, 8] = new MapStruct(
                        new TransformComponent(14, 8),
                        new ColliderComponent(Cell.Wall));
            Map[14, 9] = new MapStruct(
                        new TransformComponent(14, 9),
                        new ColliderComponent(Cell.Wall));
            Map[14, 10] = new MapStruct(
                        new TransformComponent(14, 10),
                        new ColliderComponent(Cell.Wall));
            Map[14, 16] = new MapStruct(
                        new TransformComponent(14, 16),
                        new ColliderComponent(Cell.Wall));
            Map[14, 18] = new MapStruct(
                        new TransformComponent(14, 18),
                        new ColliderComponent(Cell.Wall));
            Map[14, 19] = new MapStruct(
                        new TransformComponent(14, 19),
                        new ColliderComponent(Cell.Wall));
            Map[14, 20] = new MapStruct(
                        new TransformComponent(14, 19),
                        new ColliderComponent(Cell.Wall));
            Map[14, 21] = new MapStruct(
                        new TransformComponent(14, 21),
                        new ColliderComponent(Cell.Wall));
            Map[14, 22] = new MapStruct(
                        new TransformComponent(14, 22),
                        new ColliderComponent(Cell.Wall));
            Map[14, 24] = new MapStruct(
                        new TransformComponent(14, 24),
                        new ColliderComponent(Cell.Wall));
            Map[14, 25] = new MapStruct(
                        new TransformComponent(14, 25),
                        new ColliderComponent(Cell.Wall));
            Map[14, 26] = new MapStruct(
                        new TransformComponent(14, 26),
                        new ColliderComponent(Cell.Wall));
            Map[14, 27] = new MapStruct(
                        new TransformComponent(14, 27),
                        new ColliderComponent(Cell.Wall));
            Map[14, 28] = new MapStruct(
                        new TransformComponent(14, 28),
                        new ColliderComponent(Cell.Wall));
            Map[14, 30] = new MapStruct(
                        new TransformComponent(14, 30),
                        new ColliderComponent(Cell.Wall));

            Map[15, 0] = new MapStruct(
                        new TransformComponent(15, 0),
                        new ColliderComponent(Cell.Wall));
            Map[15, 6] = new MapStruct(
                        new TransformComponent(15, 6),
                        new ColliderComponent(Cell.Wall));
            Map[15, 7] = new MapStruct(
                        new TransformComponent(15, 7),
                        new ColliderComponent(Cell.Wall));
            Map[15, 12] = new MapStruct(
                        new TransformComponent(15, 12),
                        new ColliderComponent(Cell.Wall));
            Map[15, 16] = new MapStruct(
                        new TransformComponent(15, 16),
                        new ColliderComponent(Cell.Wall));
            Map[15, 18] = new MapStruct(
                        new TransformComponent(15, 18),
                        new ColliderComponent(Cell.Wall));
            Map[15, 19] = new MapStruct(
                        new TransformComponent(15, 19),
                        new ColliderComponent(Cell.Wall));
            Map[15, 24] = new MapStruct(
                        new TransformComponent(15, 24),
                        new ColliderComponent(Cell.Wall));
            Map[15, 25] = new MapStruct(
                        new TransformComponent(15, 25),
                        new ColliderComponent(Cell.Wall));
            Map[15, 30] = new MapStruct(
                        new TransformComponent(15, 30),
                        new ColliderComponent(Cell.Wall));

            Map[16, 0] = new MapStruct(
                        new TransformComponent(16, 0),
                        new ColliderComponent(Cell.Wall));
            Map[16, 2] = new MapStruct(
                        new TransformComponent(16, 2),
                        new ColliderComponent(Cell.Wall));
            Map[16, 3] = new MapStruct(
                        new TransformComponent(16, 3),
                        new ColliderComponent(Cell.Wall));
            Map[16, 4] = new MapStruct(
                        new TransformComponent(16, 4),
                        new ColliderComponent(Cell.Wall));
            Map[16, 6] = new MapStruct(
                        new TransformComponent(16, 6),
                        new ColliderComponent(Cell.Wall));
            Map[16, 7] = new MapStruct(
                        new TransformComponent(16, 7),
                        new ColliderComponent(Cell.Wall));
            Map[16, 9] = new MapStruct(
                        new TransformComponent(16, 9),
                        new ColliderComponent(Cell.Wall));
            Map[16, 10] = new MapStruct(
                        new TransformComponent(16, 10),
                        new ColliderComponent(Cell.Wall));
            Map[16, 12] = new MapStruct(
                        new TransformComponent(16, 12),
                        new ColliderComponent(Cell.Wall));
            Map[16, 16] = new MapStruct(
                        new TransformComponent(16, 16),
                        new ColliderComponent(Cell.Wall));
            Map[16, 18] = new MapStruct(
                        new TransformComponent(16, 18),
                        new ColliderComponent(Cell.Wall));
            Map[16, 19] = new MapStruct(
                        new TransformComponent(16, 19),
                        new ColliderComponent(Cell.Wall));
            Map[16, 21] = new MapStruct(
                        new TransformComponent(16, 21),
                        new ColliderComponent(Cell.Wall));
            Map[16, 22] = new MapStruct(
                        new TransformComponent(16, 22),
                        new ColliderComponent(Cell.Wall));
            Map[16, 24] = new MapStruct(
                        new TransformComponent(16, 24),
                        new ColliderComponent(Cell.Wall));
            Map[16, 25] = new MapStruct(
                        new TransformComponent(16, 25),
                        new ColliderComponent(Cell.Wall));
            Map[16, 27] = new MapStruct(
                        new TransformComponent(16, 27),
                        new ColliderComponent(Cell.Wall));
            Map[16, 28] = new MapStruct(
                        new TransformComponent(16, 28),
                        new ColliderComponent(Cell.Wall));
            Map[16, 30] = new MapStruct(
                        new TransformComponent(16, 30),
                        new ColliderComponent(Cell.Wall));

            Map[17, 0] = new MapStruct(
                        new TransformComponent(17, 0),
                        new ColliderComponent(Cell.Wall));
            Map[17, 2] = new MapStruct(
                        new TransformComponent(17, 2),
                        new ColliderComponent(Cell.Wall));
            Map[17, 3] = new MapStruct(
                        new TransformComponent(17, 3),
                        new ColliderComponent(Cell.Wall));
            Map[17, 4] = new MapStruct(
                        new TransformComponent(17, 4),
                        new ColliderComponent(Cell.Wall));
            Map[17, 6] = new MapStruct(
                        new TransformComponent(17, 6),
                        new ColliderComponent(Cell.Wall));
            Map[17, 7] = new MapStruct(
                        new TransformComponent(17, 7),
                        new ColliderComponent(Cell.Wall));
            Map[17, 9] = new MapStruct(
                        new TransformComponent(17, 9),
                        new ColliderComponent(Cell.Wall));
            Map[17, 10] = new MapStruct(
                        new TransformComponent(17, 10),
                        new ColliderComponent(Cell.Wall));
            Map[17, 12] = new MapStruct(
                        new TransformComponent(17, 12),
                        new ColliderComponent(Cell.Wall));
            Map[17, 13] = new MapStruct(
                        new TransformComponent(17, 13),
                        new ColliderComponent(Cell.Wall));
            Map[17, 14] = new MapStruct(
                        new TransformComponent(17, 14),
                        new ColliderComponent(Cell.Wall));
            Map[17, 15] = new MapStruct(
                        new TransformComponent(17, 15),
                        new ColliderComponent(Cell.Wall));
            Map[17, 16] = new MapStruct(
                        new TransformComponent(17, 16),
                        new ColliderComponent(Cell.Wall));
            Map[17, 18] = new MapStruct(
                        new TransformComponent(17, 18),
                        new ColliderComponent(Cell.Wall));
            Map[17, 19] = new MapStruct(
                        new TransformComponent(17, 19),
                        new ColliderComponent(Cell.Wall));
            Map[17, 21] = new MapStruct(
                        new TransformComponent(17, 21),
                        new ColliderComponent(Cell.Wall));
            Map[17, 22] = new MapStruct(
                        new TransformComponent(17, 22),
                        new ColliderComponent(Cell.Wall));
            Map[17, 24] = new MapStruct(
                        new TransformComponent(17, 24),
                        new ColliderComponent(Cell.Wall));
            Map[17, 25] = new MapStruct(
                        new TransformComponent(17, 25),
                        new ColliderComponent(Cell.Wall));
            Map[17, 27] = new MapStruct(
                        new TransformComponent(17, 27),
                        new ColliderComponent(Cell.Wall));
            Map[17, 28] = new MapStruct(
                        new TransformComponent(17, 28),
                        new ColliderComponent(Cell.Wall));
            Map[17, 30] = new MapStruct(
                        new TransformComponent(17, 30),
                        new ColliderComponent(Cell.Wall));

            Map[18, 0] = new MapStruct(
                        new TransformComponent(18, 0),
                        new ColliderComponent(Cell.Wall));
            Map[18, 2] = new MapStruct(
                        new TransformComponent(18, 2),
                        new ColliderComponent(Cell.Wall));
            Map[18, 3] = new MapStruct(
                        new TransformComponent(18, 3),
                        new ColliderComponent(Cell.Wall));
            Map[18, 4] = new MapStruct(
                        new TransformComponent(18, 4),
                        new ColliderComponent(Cell.Wall));
            Map[18, 9] = new MapStruct(
                        new TransformComponent(18, 9),
                        new ColliderComponent(Cell.Wall));
            Map[18, 10] = new MapStruct(
                        new TransformComponent(18, 10),
                        new ColliderComponent(Cell.Wall));
            Map[18, 21] = new MapStruct(
                        new TransformComponent(18, 21),
                        new ColliderComponent(Cell.Wall));
            Map[18, 22] = new MapStruct(
                        new TransformComponent(18, 22),
                        new ColliderComponent(Cell.Wall));
            Map[18, 27] = new MapStruct(
                        new TransformComponent(18, 27),
                        new ColliderComponent(Cell.Wall));
            Map[18, 28] = new MapStruct(
                        new TransformComponent(18, 28),
                        new ColliderComponent(Cell.Wall));
            Map[18, 30] = new MapStruct(
                        new TransformComponent(18, 30),
                        new ColliderComponent(Cell.Wall));

            Map[19, 0] = new MapStruct(
                        new TransformComponent(19, 0),
                        new ColliderComponent(Cell.Wall));
            Map[19, 2] = new MapStruct(
                        new TransformComponent(19, 2),
                        new ColliderComponent(Cell.Wall));
            Map[19, 3] = new MapStruct(
                        new TransformComponent(19, 3),
                        new ColliderComponent(Cell.Wall));
            Map[19, 4] = new MapStruct(
                        new TransformComponent(19, 4),
                        new ColliderComponent(Cell.Wall));
            Map[19, 6] = new MapStruct(
                        new TransformComponent(19, 6),
                        new ColliderComponent(Cell.Wall));
            Map[19, 7] = new MapStruct(
                        new TransformComponent(19, 7),
                        new ColliderComponent(Cell.Wall));
            Map[19, 8] = new MapStruct(
                        new TransformComponent(19, 8),
                        new ColliderComponent(Cell.Wall));
            Map[19, 9] = new MapStruct(
                        new TransformComponent(19, 9),
                        new ColliderComponent(Cell.Wall));
            Map[19, 10] = new MapStruct(
                        new TransformComponent(19, 10),
                        new ColliderComponent(Cell.Wall));
            Map[19, 11] = new MapStruct(
                        new TransformComponent(19, 11),
                        new ColliderComponent(Cell.Wall));
            Map[19, 12] = new MapStruct(
                        new TransformComponent(19, 12),
                        new ColliderComponent(Cell.Wall));
            Map[19, 13] = new MapStruct(
                        new TransformComponent(19, 13),
                        new ColliderComponent(Cell.Wall));
            Map[19, 15] = new MapStruct(
                        new TransformComponent(19, 15),
                        new ColliderComponent(Cell.Wall));
            Map[19, 16] = new MapStruct(
                        new TransformComponent(19, 16),
                        new ColliderComponent(Cell.Wall));
            Map[19, 17] = new MapStruct(
                        new TransformComponent(19, 17),
                        new ColliderComponent(Cell.Wall));
            Map[19, 18] = new MapStruct(
                        new TransformComponent(19, 18),
                        new ColliderComponent(Cell.Wall));
            Map[19, 19] = new MapStruct(
                        new TransformComponent(19, 19),
                        new ColliderComponent(Cell.Wall));
            Map[19, 13] = new MapStruct(
                        new TransformComponent(19, 13),
                        new ColliderComponent(Cell.Wall));
            Map[19, 21] = new MapStruct(
                        new TransformComponent(19, 21),
                        new ColliderComponent(Cell.Wall));
            Map[19, 22] = new MapStruct(
                        new TransformComponent(19, 22),
                        new ColliderComponent(Cell.Wall));
            Map[19, 24] = new MapStruct(
                        new TransformComponent(19, 24),
                        new ColliderComponent(Cell.Wall));
            Map[19, 25] = new MapStruct(
                        new TransformComponent(19, 25),
                        new ColliderComponent(Cell.Wall));
            Map[19, 26] = new MapStruct(
                        new TransformComponent(19, 26),
                        new ColliderComponent(Cell.Wall));
            Map[19, 27] = new MapStruct(
                        new TransformComponent(19, 27),
                        new ColliderComponent(Cell.Wall));
            Map[19, 28] = new MapStruct(
                        new TransformComponent(19, 28),
                        new ColliderComponent(Cell.Wall));
            Map[19, 30] = new MapStruct(
                        new TransformComponent(19, 30),
                        new ColliderComponent(Cell.Wall));

            Map[20, 0] = new MapStruct(
                        new TransformComponent(20, 0),
                        new ColliderComponent(Cell.Wall));
            Map[20, 2] = new MapStruct(
                        new TransformComponent(20, 2),
                        new ColliderComponent(Cell.Wall));
            Map[20, 3] = new MapStruct(
                        new TransformComponent(20, 3),
                        new ColliderComponent(Cell.Wall));
            Map[20, 4] = new MapStruct(
                        new TransformComponent(20, 4),
                        new ColliderComponent(Cell.Wall));
            Map[20, 6] = new MapStruct(
                        new TransformComponent(20, 6),
                        new ColliderComponent(Cell.Wall));
            Map[20, 7] = new MapStruct(
                        new TransformComponent(20, 7),
                        new ColliderComponent(Cell.Wall));
            Map[20, 8] = new MapStruct(
                        new TransformComponent(20, 8),
                        new ColliderComponent(Cell.Wall));
            Map[20, 9] = new MapStruct(
                        new TransformComponent(20, 9),
                        new ColliderComponent(Cell.Wall));
            Map[20, 10] = new MapStruct(
                        new TransformComponent(20, 10),
                        new ColliderComponent(Cell.Wall));
            Map[20, 11] = new MapStruct(
                        new TransformComponent(20, 11),
                        new ColliderComponent(Cell.Wall));
            Map[20, 12] = new MapStruct(
                        new TransformComponent(20, 12),
                        new ColliderComponent(Cell.Wall));
            Map[20, 13] = new MapStruct(
                        new TransformComponent(20, 13),
                        new ColliderComponent(Cell.Wall));
            Map[20, 15] = new MapStruct(
                        new TransformComponent(20, 15),
                        new ColliderComponent(Cell.Wall));
            Map[20, 16] = new MapStruct(
                        new TransformComponent(20, 16),
                        new ColliderComponent(Cell.Wall));
            Map[20, 17] = new MapStruct(
                        new TransformComponent(20, 17),
                        new ColliderComponent(Cell.Wall));
            Map[20, 18] = new MapStruct(
                        new TransformComponent(20, 18),
                        new ColliderComponent(Cell.Wall));
            Map[20, 19] = new MapStruct(
                        new TransformComponent(20, 19),
                        new ColliderComponent(Cell.Wall));
            Map[20, 13] = new MapStruct(
                        new TransformComponent(20, 13),
                        new ColliderComponent(Cell.Wall));
            Map[20, 21] = new MapStruct(
                        new TransformComponent(20, 21),
                        new ColliderComponent(Cell.Wall));
            Map[20, 22] = new MapStruct(
                        new TransformComponent(20, 22),
                        new ColliderComponent(Cell.Wall));
            Map[20, 24] = new MapStruct(
                        new TransformComponent(20, 24),
                        new ColliderComponent(Cell.Wall));
            Map[20, 25] = new MapStruct(
                        new TransformComponent(20, 25),
                        new ColliderComponent(Cell.Wall));
            Map[20, 26] = new MapStruct(
                        new TransformComponent(20, 26),
                        new ColliderComponent(Cell.Wall));
            Map[20, 27] = new MapStruct(
                        new TransformComponent(20, 27),
                        new ColliderComponent(Cell.Wall));
            Map[20, 28] = new MapStruct(
                        new TransformComponent(20, 28),
                        new ColliderComponent(Cell.Wall));
            Map[20, 30] = new MapStruct(
                        new TransformComponent(20, 30),
                        new ColliderComponent(Cell.Wall));

            Map[21, 0] = new MapStruct(
                        new TransformComponent(21, 0),
                        new ColliderComponent(Cell.Wall));
            Map[21, 27] = new MapStruct(
                        new TransformComponent(21, 27),
                        new ColliderComponent(Cell.Wall));
            Map[21, 28] = new MapStruct(
                        new TransformComponent(21, 28),
                        new ColliderComponent(Cell.Wall));
            Map[21, 30] = new MapStruct(
                        new TransformComponent(21, 30),
                        new ColliderComponent(Cell.Wall));

            Map[22, 0] = new MapStruct(
                        new TransformComponent(22, 0),
                        new ColliderComponent(Cell.Wall));
            Map[22, 2] = new MapStruct(
                        new TransformComponent(22, 2),
                        new ColliderComponent(Cell.Wall));
            Map[22, 3] = new MapStruct(
                        new TransformComponent(22, 3),
                        new ColliderComponent(Cell.Wall));
            Map[22, 4] = new MapStruct(
                        new TransformComponent(22, 4),
                        new ColliderComponent(Cell.Wall));
            Map[22, 6] = new MapStruct(
                        new TransformComponent(22, 6),
                        new ColliderComponent(Cell.Wall));
            Map[22, 7] = new MapStruct(
                        new TransformComponent(22, 7),
                        new ColliderComponent(Cell.Wall));
            Map[22, 9] = new MapStruct(
                        new TransformComponent(22, 9),
                        new ColliderComponent(Cell.Wall));
            Map[22, 10] = new MapStruct(
                        new TransformComponent(22, 10),
                        new ColliderComponent(Cell.Wall));
            Map[22, 11] = new MapStruct(
                        new TransformComponent(22, 11),
                        new ColliderComponent(Cell.Wall));
            Map[22, 12] = new MapStruct(
                        new TransformComponent(22, 12),
                        new ColliderComponent(Cell.Wall));
            Map[22, 13] = new MapStruct(
                        new TransformComponent(22, 13),
                        new ColliderComponent(Cell.Wall));
            Map[22, 14] = new MapStruct(
                        new TransformComponent(22, 14),
                        new ColliderComponent(Cell.Wall));
            Map[22, 15] = new MapStruct(
                        new TransformComponent(22, 15),
                        new ColliderComponent(Cell.Wall));
            Map[22, 16] = new MapStruct(
                        new TransformComponent(22, 16),
                        new ColliderComponent(Cell.Wall));
            Map[22, 17] = new MapStruct(
                        new TransformComponent(22, 17),
                        new ColliderComponent(Cell.Wall));
            Map[22, 18] = new MapStruct(
                        new TransformComponent(22, 18),
                        new ColliderComponent(Cell.Wall));
            Map[22, 19] = new MapStruct(
                        new TransformComponent(22, 19),
                        new ColliderComponent(Cell.Wall));
            Map[22, 21] = new MapStruct(
                        new TransformComponent(22, 21),
                        new ColliderComponent(Cell.Wall));
            Map[22, 22] = new MapStruct(
                        new TransformComponent(22, 22),
                        new ColliderComponent(Cell.Wall));
            Map[22, 23] = new MapStruct(
                        new TransformComponent(22, 23),
                        new ColliderComponent(Cell.Wall));
            Map[22, 24] = new MapStruct(
                        new TransformComponent(22, 24),
                        new ColliderComponent(Cell.Wall));
            Map[22, 25] = new MapStruct(
                        new TransformComponent(22, 25),
                        new ColliderComponent(Cell.Wall));
            Map[22, 27] = new MapStruct(
                        new TransformComponent(22, 27),
                        new ColliderComponent(Cell.Wall));
            Map[22, 28] = new MapStruct(
                        new TransformComponent(22, 28),
                        new ColliderComponent(Cell.Wall));
            Map[22, 30] = new MapStruct(
                        new TransformComponent(22, 30),
                        new ColliderComponent(Cell.Wall));

            Map[23, 0] = new MapStruct(
                        new TransformComponent(23, 0),
                        new ColliderComponent(Cell.Wall));
            Map[23, 2] = new MapStruct(
                        new TransformComponent(23, 2),
                        new ColliderComponent(Cell.Wall));
            Map[23, 3] = new MapStruct(
                        new TransformComponent(23, 3),
                        new ColliderComponent(Cell.Wall));
            Map[23, 4] = new MapStruct(
                        new TransformComponent(23, 4),
                        new ColliderComponent(Cell.Wall));
            Map[23, 6] = new MapStruct(
                        new TransformComponent(23, 6),
                        new ColliderComponent(Cell.Wall));
            Map[23, 7] = new MapStruct(
                        new TransformComponent(23, 7),
                        new ColliderComponent(Cell.Wall));
            Map[23, 9] = new MapStruct(
                        new TransformComponent(23, 9),
                        new ColliderComponent(Cell.Wall));
            Map[23, 10] = new MapStruct(
                        new TransformComponent(23, 10),
                        new ColliderComponent(Cell.Wall));
            Map[23, 11] = new MapStruct(
                        new TransformComponent(23, 11),
                        new ColliderComponent(Cell.Wall));
            Map[23, 12] = new MapStruct(
                        new TransformComponent(23, 12),
                        new ColliderComponent(Cell.Wall));
            Map[23, 13] = new MapStruct(
                        new TransformComponent(23, 13),
                        new ColliderComponent(Cell.Wall));
            Map[23, 14] = new MapStruct(
                        new TransformComponent(23, 14),
                        new ColliderComponent(Cell.Wall));
            Map[23, 15] = new MapStruct(
                        new TransformComponent(23, 15),
                        new ColliderComponent(Cell.Wall));
            Map[23, 16] = new MapStruct(
                        new TransformComponent(23, 16),
                        new ColliderComponent(Cell.Wall));
            Map[23, 17] = new MapStruct(
                        new TransformComponent(23, 17),
                        new ColliderComponent(Cell.Wall));
            Map[23, 18] = new MapStruct(
                        new TransformComponent(23, 18),
                        new ColliderComponent(Cell.Wall));
            Map[23, 19] = new MapStruct(
                        new TransformComponent(23, 19),
                        new ColliderComponent(Cell.Wall));
            Map[23, 21] = new MapStruct(
                        new TransformComponent(23, 21),
                        new ColliderComponent(Cell.Wall));
            Map[23, 22] = new MapStruct(
                        new TransformComponent(23, 22),
                        new ColliderComponent(Cell.Wall));
            Map[23, 23] = new MapStruct(
                        new TransformComponent(23, 23),
                        new ColliderComponent(Cell.Wall));
            Map[23, 24] = new MapStruct(
                        new TransformComponent(23, 24),
                        new ColliderComponent(Cell.Wall));
            Map[23, 25] = new MapStruct(
                        new TransformComponent(23, 25),
                        new ColliderComponent(Cell.Wall));
            Map[23, 27] = new MapStruct(
                        new TransformComponent(23, 27),
                        new ColliderComponent(Cell.Wall));
            Map[23, 28] = new MapStruct(
                        new TransformComponent(23, 28),
                        new ColliderComponent(Cell.Wall));
            Map[23, 30] = new MapStruct(
                        new TransformComponent(23, 30),
                        new ColliderComponent(Cell.Wall));

            Map[24, 0] = new MapStruct(
                        new TransformComponent(24, 0),
                        new ColliderComponent(Cell.Wall));
            Map[24, 2] = new MapStruct(
                        new TransformComponent(24, 2),
                        new ColliderComponent(Cell.Wall));
            Map[24, 3] = new MapStruct(
                        new TransformComponent(24, 3),
                        new ColliderComponent(Cell.Wall));
            Map[24, 4] = new MapStruct(
                        new TransformComponent(24, 4),
                        new ColliderComponent(Cell.Wall));
            Map[24, 6] = new MapStruct(
                        new TransformComponent(24, 6),
                        new ColliderComponent(Cell.Wall));
            Map[24, 7] = new MapStruct(
                        new TransformComponent(24, 7),
                        new ColliderComponent(Cell.Wall));
            Map[24, 9] = new MapStruct(
                        new TransformComponent(24, 9),
                        new ColliderComponent(Cell.Wall));
            Map[24, 10] = new MapStruct(
                        new TransformComponent(24, 10),
                        new ColliderComponent(Cell.Wall));
            Map[24, 11] = new MapStruct(
                        new TransformComponent(24, 11),
                        new ColliderComponent(Cell.Wall));
            Map[24, 12] = new MapStruct(
                        new TransformComponent(24, 12),
                        new ColliderComponent(Cell.Wall));
            Map[24, 13] = new MapStruct(
                        new TransformComponent(24, 13),
                        new ColliderComponent(Cell.Wall));
            Map[24, 14] = new MapStruct(
                        new TransformComponent(24, 14),
                        new ColliderComponent(Cell.Wall));
            Map[24, 15] = new MapStruct(
                        new TransformComponent(24, 15),
                        new ColliderComponent(Cell.Wall));
            Map[24, 16] = new MapStruct(
                        new TransformComponent(24, 16),
                        new ColliderComponent(Cell.Wall));
            Map[24, 17] = new MapStruct(
                        new TransformComponent(24, 17),
                        new ColliderComponent(Cell.Wall));
            Map[24, 18] = new MapStruct(
                        new TransformComponent(24, 18),
                        new ColliderComponent(Cell.Wall));
            Map[24, 19] = new MapStruct(
                        new TransformComponent(24, 19),
                        new ColliderComponent(Cell.Wall));
            Map[24, 21] = new MapStruct(
                        new TransformComponent(24, 21),
                        new ColliderComponent(Cell.Wall));
            Map[24, 22] = new MapStruct(
                        new TransformComponent(24, 22),
                        new ColliderComponent(Cell.Wall));
            Map[24, 27] = new MapStruct(
                        new TransformComponent(24, 27),
                        new ColliderComponent(Cell.Wall));
            Map[24, 28] = new MapStruct(
                        new TransformComponent(24, 28),
                        new ColliderComponent(Cell.Wall));
            Map[24, 30] = new MapStruct(
                        new TransformComponent(24, 30),
                        new ColliderComponent(Cell.Wall));

            Map[25, 0] = new MapStruct(
                        new TransformComponent(25, 0),
                        new ColliderComponent(Cell.Wall));
            Map[25, 2] = new MapStruct(
                        new TransformComponent(25, 2),
                        new ColliderComponent(Cell.Wall));
            Map[25, 3] = new MapStruct(
                        new TransformComponent(25, 3),
                        new ColliderComponent(Cell.Wall));
            Map[25, 4] = new MapStruct(
                        new TransformComponent(25, 4),
                        new ColliderComponent(Cell.Wall));
            Map[25, 6] = new MapStruct(
                        new TransformComponent(25, 6),
                        new ColliderComponent(Cell.Wall));
            Map[25, 7] = new MapStruct(
                        new TransformComponent(25, 7),
                        new ColliderComponent(Cell.Wall));
            Map[25, 9] = new MapStruct(
                        new TransformComponent(25, 9),
                        new ColliderComponent(Cell.Wall));
            Map[25, 10] = new MapStruct(
                        new TransformComponent(25, 10),
                        new ColliderComponent(Cell.Wall));
            Map[25, 11] = new MapStruct(
                        new TransformComponent(25, 11),
                        new ColliderComponent(Cell.Wall));
            Map[25, 12] = new MapStruct(
                        new TransformComponent(25, 12),
                        new ColliderComponent(Cell.Wall));
            Map[25, 13] = new MapStruct(
                        new TransformComponent(25, 13),
                        new ColliderComponent(Cell.Wall));
            Map[25, 14] = new MapStruct(
                        new TransformComponent(25, 14),
                        new ColliderComponent(Cell.Wall));
            Map[25, 15] = new MapStruct(
                        new TransformComponent(25, 15),
                        new ColliderComponent(Cell.Wall));
            Map[25, 16] = new MapStruct(
                        new TransformComponent(25, 16),
                        new ColliderComponent(Cell.Wall));
            Map[25, 17] = new MapStruct(
                        new TransformComponent(25, 17),
                        new ColliderComponent(Cell.Wall));
            Map[25, 18] = new MapStruct(
                        new TransformComponent(25, 18),
                        new ColliderComponent(Cell.Wall));
            Map[25, 19] = new MapStruct(
                        new TransformComponent(25, 19),
                        new ColliderComponent(Cell.Wall));
            Map[25, 21] = new MapStruct(
                        new TransformComponent(25, 21),
                        new ColliderComponent(Cell.Wall));
            Map[25, 22] = new MapStruct(
                        new TransformComponent(25, 22),
                        new ColliderComponent(Cell.Wall));
            Map[25, 24] = new MapStruct(
                        new TransformComponent(25, 24),
                        new ColliderComponent(Cell.Wall));
            Map[25, 25] = new MapStruct(
                        new TransformComponent(25, 25),
                        new ColliderComponent(Cell.Wall));
            Map[25, 27] = new MapStruct(
                        new TransformComponent(25, 27),
                        new ColliderComponent(Cell.Wall));
            Map[25, 28] = new MapStruct(
                        new TransformComponent(25, 28),
                        new ColliderComponent(Cell.Wall));
            Map[25, 30] = new MapStruct(
                        new TransformComponent(25, 30),
                        new ColliderComponent(Cell.Wall));

            Map[26, 0] = new MapStruct(
                        new TransformComponent(26, 0),
                        new ColliderComponent(Cell.Wall));
            Map[26, 9] = new MapStruct(
                        new TransformComponent(26, 9),
                        new ColliderComponent(Cell.Wall));
            Map[26, 10] = new MapStruct(
                        new TransformComponent(26, 10),
                        new ColliderComponent(Cell.Wall));
            Map[26, 11] = new MapStruct(
                        new TransformComponent(26, 11),
                        new ColliderComponent(Cell.Wall));
            Map[26, 12] = new MapStruct(
                        new TransformComponent(26, 12),
                        new ColliderComponent(Cell.Wall));
            Map[26, 13] = new MapStruct(
                        new TransformComponent(26, 13),
                        new ColliderComponent(Cell.Wall));
            Map[26, 14] = new MapStruct(
                        new TransformComponent(26, 14),
                        new ColliderComponent(Cell.Wall));
            Map[26, 15] = new MapStruct(
                        new TransformComponent(26, 15),
                        new ColliderComponent(Cell.Wall));
            Map[26, 16] = new MapStruct(
                        new TransformComponent(26, 16),
                        new ColliderComponent(Cell.Wall));
            Map[26, 17] = new MapStruct(
                        new TransformComponent(26, 17),
                        new ColliderComponent(Cell.Wall));
            Map[26, 18] = new MapStruct(
                        new TransformComponent(26, 18),
                        new ColliderComponent(Cell.Wall));
            Map[26, 19] = new MapStruct(
                        new TransformComponent(26, 19),
                        new ColliderComponent(Cell.Wall));
            Map[26, 24] = new MapStruct(
                        new TransformComponent(26, 24),
                        new ColliderComponent(Cell.Wall));
            Map[26, 25] = new MapStruct(
                        new TransformComponent(26, 25),
                        new ColliderComponent(Cell.Wall));
            Map[26, 30] = new MapStruct(
                        new TransformComponent(26, 30),
                        new ColliderComponent(Cell.Wall));

            Map[27, 0] = new MapStruct(
                        new TransformComponent(27, 0),
                        new ColliderComponent(Cell.Wall));
            Map[27, 1] = new MapStruct(
                        new TransformComponent(27, 1),
                        new ColliderComponent(Cell.Wall));
            Map[27, 2] = new MapStruct(
                        new TransformComponent(27, 2),
                        new ColliderComponent(Cell.Wall));
            Map[27, 3] = new MapStruct(
                        new TransformComponent(27, 3),
                        new ColliderComponent(Cell.Wall));
            Map[27, 4] = new MapStruct(
                        new TransformComponent(27, 4),
                        new ColliderComponent(Cell.Wall));
            Map[27, 5] = new MapStruct(
                        new TransformComponent(27, 5),
                        new ColliderComponent(Cell.Wall));
            Map[27, 6] = new MapStruct(
                        new TransformComponent(27, 6),
                        new ColliderComponent(Cell.Wall));
            Map[27, 7] = new MapStruct(
                        new TransformComponent(27, 7),
                        new ColliderComponent(Cell.Wall));
            Map[27, 8] = new MapStruct(
                        new TransformComponent(27, 8),
                        new ColliderComponent(Cell.Wall));
            Map[27, 9] = new MapStruct(
                        new TransformComponent(27, 9),
                        new ColliderComponent(Cell.Wall));
            Map[27, 10] = new MapStruct(
                        new TransformComponent(27, 10),
                        new ColliderComponent(Cell.Wall));
            Map[27, 11] = new MapStruct(
                        new TransformComponent(27, 11),
                        new ColliderComponent(Cell.Wall));
            Map[27, 12] = new MapStruct(
                        new TransformComponent(27, 12),
                        new ColliderComponent(Cell.Wall));
            Map[27, 13] = new MapStruct(
                        new TransformComponent(27, 13),
                        new ColliderComponent(Cell.Wall));
            Map[27, 14] = new MapStruct(
                        new TransformComponent(27, 14),
                        new ColliderComponent(Cell.Wall));
            Map[27, 15] = new MapStruct(
                        new TransformComponent(27, 15),
                        new ColliderComponent(Cell.Wall));
            Map[27, 16] = new MapStruct(
                        new TransformComponent(27, 16),
                        new ColliderComponent(Cell.Wall));
            Map[27, 17] = new MapStruct(
                        new TransformComponent(27, 17),
                        new ColliderComponent(Cell.Wall));
            Map[27, 18] = new MapStruct(
                        new TransformComponent(27, 18),
                        new ColliderComponent(Cell.Wall));
            Map[27, 19] = new MapStruct(
                        new TransformComponent(27, 19),
                        new ColliderComponent(Cell.Wall));
            Map[27, 20] = new MapStruct(
                        new TransformComponent(27, 20),
                        new ColliderComponent(Cell.Wall));
            Map[27, 21] = new MapStruct(
                        new TransformComponent(27, 21),
                        new ColliderComponent(Cell.Wall));
            Map[27, 22] = new MapStruct(
                        new TransformComponent(27, 22),
                        new ColliderComponent(Cell.Wall));
            Map[27, 23] = new MapStruct(
                        new TransformComponent(27, 23),
                        new ColliderComponent(Cell.Wall));
            Map[27, 24] = new MapStruct(
                        new TransformComponent(27, 24),
                        new ColliderComponent(Cell.Wall));
            Map[27, 25] = new MapStruct(
                        new TransformComponent(27, 25),
                        new ColliderComponent(Cell.Wall));
            Map[27, 26] = new MapStruct(
                        new TransformComponent(27, 26),
                        new ColliderComponent(Cell.Wall));
            Map[27, 27] = new MapStruct(
                        new TransformComponent(27, 27),
                        new ColliderComponent(Cell.Wall));
            Map[27, 28] = new MapStruct(
                        new TransformComponent(27, 28),
                        new ColliderComponent(Cell.Wall));
            Map[27, 29] = new MapStruct(
                        new TransformComponent(27, 29),
                        new ColliderComponent(Cell.Wall));
            Map[27, 30] = new MapStruct(
                        new TransformComponent(27, 30),
                        new ColliderComponent(Cell.Wall));
        }
    }
}
