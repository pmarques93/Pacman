using System;

namespace Pacman
{
    /// <summary>
    /// Class for lives component. Extends Component
    /// </summary>
    public class LivesComponent: Component
    {
        /// <summary>
        /// Property with lives
        /// </summary>
        public byte Lives { get; }

        /// <summary>
        /// Constructor for lives component
        /// </summary>
        /// <param name="lives">Lives to add</param>
        public LivesComponent(byte lives)
        {
            Lives = lives;
        }

        /// <summary>
        /// Method that runs once on the beggining
        /// </summary>
        public override void Start()
        {
            if (Lives < 1)
            {
                OnEndGame();
            }
        }

        /// <summary>
        /// Method that calls EndGame event
        /// </summary>
        protected virtual void OnEndGame()
            => EndGame?.Invoke();

        public event Action EndGame;
        
    }
}
