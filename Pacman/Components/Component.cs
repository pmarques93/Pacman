namespace Pacman.Components
{
    /// <summary>
    /// Abstract class for every component. Implements IGameObject.
    /// </summary>
    public abstract class Component : IGameObject
    {
        /// <summary>
        /// Gets name for property.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets or sets parent game object property.
        /// </summary>
        public GameObject ParentGameObject { get; set; }

        /// <summary>
        /// Method that runs once on start.
        /// </summary>
        public virtual void Start()
        {
            // Line intentionally left empty.
        }

        /// <summary>
        /// Method responsible for what happens when the GameObject is running.
        /// </summary>
        public virtual void Update()
        {
            // Line intentionally left empty.
        }

        /// <summary>
        /// Method that runs once on finish.
        /// </summary>
        public virtual void Finish()
        {
            // Line intentionally left empty.
        }
    }
}
