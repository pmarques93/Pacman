namespace Pacman.MovementBehaviours
{
    /// <summary>
    /// Interface for Movement.
    /// </summary>
    public interface IMovementBehaviour
    {
        /// <summary>
        /// Movement Behaviour.
        /// </summary>
        /// <param name="xMax">Horizontal map size.</param>
        /// <param name="yMax">Vertical map size.</param>
        void Movement(int xMax, int yMax);
    }
}
