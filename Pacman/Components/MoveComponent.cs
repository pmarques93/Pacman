using Pacman.MovementBehaviours;

namespace Pacman.Components
{
    /// <summary>
    /// Movement Component. Extends Component.
    /// </summary>
    public class MoveComponent : Component
    {
        // Position related variables
        private int maxX;

        // Position related variables
        private int maxY;

        /// <summary>
        /// Gets or sets movementState.
        /// </summary>
        public MovementState MovementState { get; set; }

        // Type of Movement
        private IMovementBehaviour movementBehaviour;

        /// <summary>
        /// Constructor for MoveComponent.
        /// </summary>
        public MoveComponent()
        {
            MovementState = MovementState.None;
        }

        /// <summary>
        /// Method that runs once on start.
        /// </summary>
        public override void Start()
        {
            MapComponent map = ParentGameObject.GetComponent<MapComponent>();

            maxX = map.Map.GetLength(0);
            maxY = map.Map.GetLength(1);
        }

        /// <summary>
        /// Adds current movement to this class.
        /// </summary>
        /// <param name="movementBehaviour">Movement Behaviour to add.</param>
        public void AddMovementBehaviour(IMovementBehaviour movementBehaviour)
            => this.movementBehaviour = movementBehaviour;

        /// <summary>
        /// Method responsible for what happens when the GameObject is running.
        /// Runs movement behavior movement method.
        /// </summary>
        public override void Update()
        {
            movementBehaviour.Movement(maxX, maxY);
        }
    }
}
