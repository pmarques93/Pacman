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
        private KeyReaderComponent keyReader;
        private TransformComponent transform;
        private MapComponent map;

        // Type of Movement
        IMovementBehaviour movementBehaviour;



        public MoveComponent(byte initPosX, byte initPosY, IMovementBehaviour movementBehaviour)
        {
            this.initPosX = initPosX;
            this.initPosY = initPosY;

            this.movementBehaviour = movementBehaviour;
        }

        /// <summary>
        /// Method that runs once on start
        /// </summary>
        public override void Start()
        {
            keyReader = ParentGameObject.GetComponent<KeyReaderComponent>();
            transform = ParentGameObject.GetComponent<TransformComponent>();
            map = ParentGameObject.GetComponent<MapComponent>();

            movementBehaviour.KeyReader = keyReader;
            movementBehaviour.Map = map;
            movementBehaviour.Transform = transform;

            // Adds initial position
            transform.Position = new Vector2Int(initPosX, initPosY);

            maxX = map.Map.GetLength(0);
            maxY = map.Map.GetLength(1);
        }

        /// <summary>
        /// Method responsible for what happens when the GameObject is running
        /// </summary>
        public override void Update()
        {
            movementBehaviour.Movement(maxX, maxY);
        }
    }
}
