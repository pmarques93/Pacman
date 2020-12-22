namespace Pacman
{
    /// <summary>
    /// Abstract class for every component. Implements IGameObject
    /// </summary>
    public abstract class Component : IGameObject
    {
        /// <summary>
        /// Property with this component's parent GameObject
        /// </summary>
        public GameObject ParentGameObject { get; set; }
        
        /// <summary>
        /// Method that runs once on start
        /// </summary>
        public virtual void Start()
        {

        }

        /// <summary>
        /// Method responsible for what happens when the GameObject is running
        /// </summary>
        public virtual void Update()
        {

        }

        /// <summary>
        /// Method that runs once on finish
        /// </summary>
        public virtual void Finish()
        {

        }
    }
}
