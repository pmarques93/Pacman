using System;

namespace Pacman
{
    /// <summary>
    /// Enum with cell types
    /// </summary>
    [Flags]
    public enum Cell
    {
        None = 0,
        Walkable = 1,
        Wall = 2,
        Pacman = 4,
        Ghost = 8,
        Fruit = 16,
        Food = 32,
        PowerPill = 64,
        GhostHouse = 128,
        GhostHouseExit = 256,
    }
}
