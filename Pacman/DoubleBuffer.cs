using System;

namespace Pacman
{
    /// <summary>
    /// Class for double buffer
    /// </summary>
    /// <typeparam name="T">The type of T</typeparam>
    public class DoubleBuffer<T>
    {
        private T[,] current, next;

        /// <summary>
        /// Property with the X size of the current T
        /// </summary>
        public int X => current.GetLength(0);

        /// <summary>
        /// Property with the Y size of the current T
        /// </summary>
        public int Y => current.GetLength(1);

        /// <summary>
        /// T Indexer
        /// </summary>
        /// <param name="x">X value</param>
        /// <param name="y">Y Value</param>
        /// <returns>Returns the value of [x, y]</returns>
        public T this[int x, int y]
        {
            get => current[x, y];
            set => next[x, y] = value;
        }

        /// <summary>
        /// Clears T next
        /// </summary>
        public void Clear()
            => Array.Clear(next, 0, X * Y);

        /// <summary>
        /// Constructor for double buffer
        /// </summary>
        /// <param name="x">X size</param>
        /// <param name="y">Y size</param>
        public DoubleBuffer(int x, int y)
        {
            current = new T[x, y];
            next = new T[x, y];
        }

        /// <summary>
        /// Swaps current T with next T
        /// </summary>
        public void Swap()
        {
            T[,] temp = current;
            current = next;
            next = temp;
        }
    }
}
