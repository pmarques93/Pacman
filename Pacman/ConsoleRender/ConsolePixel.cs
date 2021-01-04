using System;

namespace Pacman
{
    /// <summary>
    /// Struct that represents a console pixel.
    /// </summary>
    public struct ConsolePixel
    {
        /// <summary>
        /// Gets char that represents the ConsolePixel.
        /// </summary>
        public char Shape { get; }

        /// <summary>
        /// Gets color of the shape.
        /// </summary>
        public ConsoleColor ForegroundColor { get; }

        /// <summary>
        /// Gets background color of the ConsolePixel.
        /// </summary>
        public ConsoleColor BackgroundColor { get; }

        /// <summary>
        /// Gets a value indicating whether defines whether the ConsolePixel
        /// is renderable.
        /// </summary>
        public bool IsRenderable
        {
            get
            {
                // The pixel is renderable if any of its fields is not the
                // default to the specific type
                return !Shape.Equals(default(char))
                    && !ForegroundColor.Equals(default(ConsoleColor))
                    && !BackgroundColor.Equals(default(ConsoleColor));
            }
        }

        /// <summary>
        /// Constructor for ConsolePixel.
        /// </summary>
        /// <param name="shape">Char that defines a shape.</param>
        /// <param name="foregroundColor">Foreground Color.</param>
        /// <param name="backgroundColor">Background Color.</param>
        public ConsolePixel(
            char shape,
            ConsoleColor foregroundColor,
            ConsoleColor backgroundColor)
        {
            Shape = shape;
            ForegroundColor = foregroundColor;
            BackgroundColor = backgroundColor;
        }
    }
}