namespace Pacman
{
    /// <summary>
    /// Enum with possible movement states.
    /// </summary>
    public enum MovementState
    {
        /// <summary>
        /// No movement state.
        /// </summary>
        None,

        /// <summary>
        /// Chase movement state.
        /// </summary>
        Chase,

        /// <summary>
        /// Frightened movement state.
        /// </summary>
        Frightened,

        /// <summary>
        /// Eaten movement state.
        /// </summary>
        Eaten,

        /// <summary>
        /// Scatter movement state.
        /// </summary>
        Scatter,

        /// <summary>
        /// OnGhostHouse movement state.
        /// </summary>
        OnGhostHouse,

        /// <summary>
        /// OutGhostHouse movement state.
        /// </summary>
        OutGhostHouse,
    }
}
