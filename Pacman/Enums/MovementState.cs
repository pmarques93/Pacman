using System;

namespace Pacman
{
    /// <summary>
    /// Enum with possible movement states.
    /// </summary>
    [Flags]
    public enum MovementState
    {
        /// <summary>
        /// No movement state.
        /// </summary>
        None = 0,

        /// <summary>
        /// Chase movement state.
        /// </summary>
        Chase = 1,

        /// <summary>
        /// Frightened movement state.
        /// </summary>
        Frightened = 2,

        /// <summary>
        /// Eaten movement state.
        /// </summary>
        Eaten = 3,

        /// <summary>
        /// Scatter movement state.
        /// </summary>
        Scatter = 4,

        /// <summary>
        /// OnGhostHouse movement state.
        /// </summary>
        OnGhostHouse = 5,

        /// <summary>
        /// OutGhostHouse movement state.
        /// </summary>
        OutGhostHouse = 6,
    }
}
