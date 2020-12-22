using System;

namespace Pacman
{
    /// <summary>
    /// Movement Component. Extends Component
    /// </summary>
    public class MoveComponent: Component
    {
        // Position related variables
        private int maxX, maxY;

        // Type of Movement
        private IMovementBehaviour movementBehaviour;

        /// <summary>
        /// Method that runs once on start
        /// </summary>
        public override void Start()
        {
            MapComponent map = ParentGameObject.GetComponent<MapComponent>();

            maxX = map.MapTest.GetLength(0);
            maxY = map.MapTest.GetLength(1);
        }

        /// <summary>
        /// Adds current movement to this class
        /// </summary>
        /// <param name="movementBehaviour">Movement Behaviour to add</param>
        public void AddMovementBehaviour(IMovementBehaviour movementBehaviour)
            => this.movementBehaviour = movementBehaviour;

        /// <summary>
        /// Method responsible for what happens when the GameObject is running
        /// Runs movement behavior movement method
        /// </summary>
        public override void Update()
        {
            movementBehaviour.Movement(maxX, maxY);
        }
    }
}
