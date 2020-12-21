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
        public GameObject ParentGameObject { get; }
        
        /// <summary>
        /// Method that runs once on start
        /// </summary>
        public virtual void Start()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Method responsible for what happens when the GameObject is running
        /// </summary>
        public virtual void Update()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Method that runs once on finish
        /// </summary>
        public virtual void Finish()
        {
            throw new System.NotImplementedException();
        }
    }
}
