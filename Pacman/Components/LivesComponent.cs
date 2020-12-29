
namespace Pacman
{
    /// <summary>
    /// Class for lives component. Extends Component
    /// </summary>
    public class LivesComponent : Component
    {
        /// <summary>
        /// Property with lives
        /// </summary>
        public byte Lives { get; set; }

        /// <summary>
        /// Constructor for lives component
        /// </summary>
        /// <param name="lives">Lives to add</param>
        public LivesComponent(byte lives)
        {
            Lives = lives;
        }
    }
}
