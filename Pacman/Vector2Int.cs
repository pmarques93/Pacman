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

        public static bool operator ==(Vector2Int v1, Vector2Int v2)
            => v1.Equals(v2);

        public static bool operator !=(Vector2Int v1, Vector2Int v2)
            => !v1.Equals(v2);

        public static Vector2Int operator +(Vector2Int v1, Vector2Int v2)
        {
            return new Vector2Int(v1.X + v2.X, v1.Y + v2.Y);
        }

        public static Vector2Int operator -(Vector2Int v1, Vector2Int v2)
        {
            return new Vector2Int(v1.X - v2.X, v1.Y - v2.Y);
        }

        public override bool Equals(object obj)
        {
            Vector2Int other = (Vector2Int)obj;
            return X == other.X && Y == other.Y;
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }

        public override string ToString()
        {
            return $"x: {X}, y: {Y}";
        }
    }
}
