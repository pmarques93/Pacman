using System;

namespace Pacman
{
    /// <summary>
    /// Enum with cell types.
    /// </summary>
    [Flags]
    public enum Cell
    {
        /// <summary>
        /// No cell.
        /// </summary>
        None = 0,

        /// <summary>
        /// Walkable Cell
        /// </summary>
        Walkable = 1,

        /// <summary>
        /// Wall cell
        /// </summary>
        Wall = 2,

        /// <summary>
        /// Pacman Cell
        /// </summary>
        Pacman = 4,

        /// <summary>
        /// Ghost Cell
        /// </summary>
        Ghost = 8,

        /// <summary>
        /// Fruit Cell
        /// </summary>
        Fruit = 16,
        
        /// <summary>
        /// Food Cell
        /// </summary>
        Food = 32,

        /// <summary>
        /// PowerPill cell
        /// </summary>
        PowerPill = 64,

        /// <summary>
        /// GhostHouse cell
        /// </summary>
        GhostHouse = 128,

        /// <summary>
        /// GhostHouseExit Cell
        /// </summary>
        GhostHouseExit = 256,
    }
}
