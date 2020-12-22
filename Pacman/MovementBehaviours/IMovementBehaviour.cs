namespace Pacman
{
    /// <summary>
    /// Interface for Movement
    /// </summary>
    public interface IMovementBehaviour
    {
        public KeyReaderComponent KeyReader { get; set; }

        public TransformComponent Transform { get; set; }

        public MapComponent Map { get; set; }

        /// <summary>
        /// Movement Behaviour
        /// </summary>
        /// <param name="maxX">X map size</param>
        /// <param name="maxY">Y map size</param>
        void Movement(int maxX, int maxY);
    }
}
