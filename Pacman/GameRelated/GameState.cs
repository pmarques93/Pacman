using System;

namespace Pacman
{
    /// <summary>
    /// Class with game state information
    /// </summary>
    public class GameState: Component
    {
        private readonly Collision collisions;

        private MovementState movementState;

        public GameState(Collision collision)
        {
            collisions = collision;
            movementState = MovementState.Chase;
        }

        public override void Start()
            => collisions.GhostCollision += GhostCollision;
        public override void Finish()
            => collisions.GhostCollision -= GhostCollision;

        /// <summary>
        /// Happens once when there's a collision with a ghost
        /// </summary>
        private void GhostCollision()
        {
            switch (movementState)
            {
                case MovementState.Chase:
                    OnGhostChaseCollision();
                    break;
                case MovementState.Frightened:
                    /////////////////////////
                    ///Ghost volta para o centro
                    //////////////////////////
                    break;
            }
        }

        /// <summary>
        /// On Ghost Chase collision method. Calls GhostChaseCollision event
        /// </summary>
        protected virtual void OnGhostChaseCollision()
            => GhostChaseCollision?.Invoke();

        public event Action GhostChaseCollision;
    }
}
