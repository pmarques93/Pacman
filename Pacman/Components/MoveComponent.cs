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
        private byte initPosX, initPosY;

        // Components
        private TransformComponent transform;
        private MapComponent map;

        // Type of Movement
        IMovementBehaviour movementBehaviour;

        public MoveComponent(byte initPosX, byte initPosY)
        {
            this.initPosX = initPosX;
            this.initPosY = initPosY;
        }

        /// <summary>
        /// Method that runs once on start
        /// </summary>
        public override void Start()
        {
            transform = ParentGameObject.GetComponent<TransformComponent>();
            map = ParentGameObject.GetComponent<MapComponent>();

            // Adds initial position
            transform.Position = new Vector2Int(initPosX, initPosY);

            maxX = map.Map.GetLength(0);
            maxY = map.Map.GetLength(1);
        }

        public void AddMovementBehaviour(IMovementBehaviour movementBehaviour)
            => this.movementBehaviour = movementBehaviour;

        /// <summary>
        /// Method responsible for what happens when the GameObject is running
        /// </summary>
        public override void Update()
        {
            movementBehaviour.Movement(maxX, maxY);
        }
    }
}
