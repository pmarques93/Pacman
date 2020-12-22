namespace Pacman
{
    /// <summary>
    /// Interface for Movement
    /// </summary>
    public interface IMovementBehaviour
    {
        /// <summary>
        /// Movement Behaviour
        /// </summary>
        /// <param name="maxX">X map size</param>
        /// <param name="maxY">Y map size</param>
        void Movement(int xMax, int yMax);
    }
}
