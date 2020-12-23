namespace Pacman
{
    /// <summary>
    /// Map Component. Extends Component
    /// </summary>
    public class MapComponent : Component
    {
        public MapStruct[,] Map { get; }

        /// <summary>
        /// Constructor for MapComponent
        /// </summary>
        /// <param name="xDim">X size</param>
        /// <param name="yDim">Y size</param>
        public MapComponent(byte xDim, byte yDim)
        {

            Map = new MapStruct[xDim, yDim];
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
