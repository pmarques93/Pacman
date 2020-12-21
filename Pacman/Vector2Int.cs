namespace Pacman
{
    /// <summary>
    /// Struct for Vector2Int
    /// </summary>
    public struct Vector2Int
    {
        /// <summary>
        /// Property for X value
        /// </summary>
        public int X { get; }

        /// <summary>
        /// Property for Y value
        /// </summary>
        public int Y { get; }

        /// <summary>
        /// Constructor for Vector2Int
        /// </summary>
        /// <param name="x">X value</param>
        /// <param name="y">Y value</param>
        public Vector2Int(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
