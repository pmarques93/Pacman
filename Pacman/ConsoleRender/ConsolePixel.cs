using System;
namespace Pacman
{
    /// <summary>
    /// Represents a console pixel
    /// </summary>
    public struct ConsolePixel
    {
        // Char that represents the ConsolePixel
        public readonly char shape;
        // Color of the shape
        public readonly ConsoleColor foregroundColor;
        // Background color of the ConsolePixel
        public readonly ConsoleColor backgroundColor;

        /// <summary>
        /// Defines whether the ConsolePixel is renderable
        /// </summary>
        public bool IsRenderable
        {
            get
            {
                // The pixel is renderable if any of its fields is not the
                // default to the specific type
                return !shape.Equals(default(char))
                    && !foregroundColor.Equals(default(ConsoleColor))
                    && !backgroundColor.Equals(default(ConsoleColor));
            }
        }


        public ConsolePixel(char shape,
            ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            this.shape = shape;
            this.foregroundColor = foregroundColor;
            this.backgroundColor = backgroundColor;
        }

        public ConsolePixel(char shape)
        {
            this.shape = shape;
            foregroundColor = Console.ForegroundColor;
            backgroundColor = Console.BackgroundColor;
        }
    }
}