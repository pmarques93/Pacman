using System;

namespace Pacman
{
    class PinkyMovementBehaviour : IMovementBehaviour
    {
        // Components
        private readonly TransformComponent transform;
        private readonly MapComponent map;

        /// <summary>
        /// Gets components from pacman gameobject
        /// </summary>
        /// <param name="pinky">Object to get components from</param>
        public PinkyMovementBehaviour(GameObject pinky)
        {
            transform = pinky.GetComponent<TransformComponent>();
            map = pinky.GetComponent<MapComponent>();
        }

        /// <summary>
        /// Gets components from pacman gameobject
        /// </summary>
        /// <param name="pinky">Object to get components from</param>
        /// <param name="moveComponent">Move component to add
        /// this behaviour to</param>
        public PinkyMovementBehaviour(GameObject pinky,
            MoveComponent moveComponent)
        {
            transform = pinky.GetComponent<TransformComponent>();
            map = pinky.GetComponent<MapComponent>();

            moveComponent.AddMovementBehaviour(this);
        }


        // TEMPORARY MOVEMENT
        bool moveRight = true;
        public void Movement(int maxX, int maxY)
        {

            if (transform.Position.X == 19)
                moveRight = false;
            else if (transform.Position.X == 0)
                moveRight = true;

            if (moveRight)
            {
                transform.Position =
                    new Vector2Int(
                    Math.Min(maxX - 1, transform.Position.X + 1),
                    transform.Position.Y);
            }
            else
            {
                transform.Position =
                new Vector2Int(
                Math.Max(0, transform.Position.X - 1),
                transform.Position.Y);
            }
        }
    }
}
