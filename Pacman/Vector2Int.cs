namespace Pacman
{
    /// <summary>
    /// Struct for Vector2Int.
    /// </summary>
    public struct Vector2Int
    {
        /// <summary>
        /// Gets X value.
        /// </summary>
        public int X { get; }

        /// <summary>
        /// Gets Y value.
        /// </summary>
        public int Y { get; }

        /// <summary>
        /// Constructor for Vector2Int.
        /// </summary>
        /// <param name="x">X value.</param>
        /// <param name="y">Y value.</param>
        public Vector2Int(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Method that overrides == operator.
        /// </summary>
        /// <param name="v1">Vector 1.</param>
        /// <param name="v2">Vector 2.</param>
        /// <returns>Returns true if the vectors are equal.</returns>
        public static bool operator ==(Vector2Int v1, Vector2Int v2)
            => v1.Equals(v2);

        /// <summary>
        /// Method that overrides != operator.
        /// </summary>
        /// <param name="v1">Vector 1.</param>
        /// <param name="v2">Vector 2.</param>
        /// <returns>Returns true if the vectors are not equal.</returns>
        public static bool operator !=(Vector2Int v1, Vector2Int v2)
            => !v1.Equals(v2);

        /// <summary>
        /// Method that overrides + operator.
        /// </summary>
        /// <param name="v1">Vector 1.</param>
        /// <param name="v2">Vector 2.</param>
        /// <returns>Returns a new vector.</returns>
        public static Vector2Int operator +(Vector2Int v1, Vector2Int v2)
        {
            return new Vector2Int(v1.X + v2.X, v1.Y + v2.Y);
        }

        /// <summary>
        /// Method that overrides - operator.
        /// </summary>
        /// <param name="v1">Vector 1.</param>
        /// <param name="v2">Vector 2.</param>
        /// <returns>Returns a new vector.</returns>
        public static Vector2Int operator -(Vector2Int v1, Vector2Int v2)
        {
            return new Vector2Int(v1.X - v2.X, v1.Y - v2.Y);
        }

        /// <summary>
        /// Method that overridas Equals.
        /// </summary>
        /// <param name="obj">Object to compare.</param>
        /// <returns>Returns true if both vectors are equal.</returns>
        public override bool Equals(object obj)
        {
            Vector2Int other = (Vector2Int)obj;
            return X == other.X && Y == other.Y;
        }

        /// <summary>
        /// Method that overrides GetHashCode.
        /// </summary>
        /// <returns>Returns an int.</returns>
        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }

        /// <summary>
        /// Method that overrides ToString.
        /// </summary>
        /// <returns>Returns a string.</returns>
        public override string ToString()
        {
            return $"x: {X}, y: {Y}";
        }
    }
}
