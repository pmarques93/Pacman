using System;
using Pacman.Components;
using Pacman.MovementBehaviours.ChaseBehaviour;

namespace Pacman
{
    /// <summary>
    /// Class with game state information
    /// </summary>
    public class GameState : Component
    {
        private readonly Collision collisions;

        private MovementState movementState;
        private GameObject pacman;

        public GameState(Collision collision, GameObject pacman)
        {
            collisions = collision;
            movementState = MovementState.Chase;
            this.pacman = pacman;
        }

        public override void Start()
        {
            collisions.GhostCollision += GhostCollision;
            collisions.GhostHouseCollision += GhostOnHouse;
            collisions.GhostHouseExitCollision += GhostOnHouseExit;
            collisions.PowerPillCollision += () => movementState = MovementState.Frightened;
        }
        public override void Finish()
        {
            collisions.GhostCollision -= GhostCollision;
            collisions.GhostHouseCollision -= GhostOnHouse;
            collisions.GhostHouseExitCollision -= GhostOnHouseExit;
            collisions.PowerPillCollision -= () => movementState = MovementState.Frightened;
        }

        private void GhostOnHouse(GameObject ghost)
        {
            MoveComponent moveComponent = ghost.GetComponent<MoveComponent>();
            if (moveComponent.movementState.HasFlag(MovementState.OnGhostHouse))
            {
                moveComponent.AddMovementBehaviour(
                       new BlinkyChaseBehaviour(
                           collisions,
                           ghost,
                           ghost.GetComponent<MapComponent>(),
                           new MapTransformComponent(13, 11),
                           ghost.GetComponent<MapTransformComponent>(),
                           3));
                moveComponent.movementState = MovementState.OutGhostHouse;
            }
        }
        private void GhostOnHouseExit(GameObject ghost, Cell cell)
        {
            ghost.GetComponent<MoveComponent>().AddMovementBehaviour(
                    new BlinkyChaseBehaviour(
                        collisions,
                        ghost,
                        ghost.GetComponent<MapComponent>(),
                        pacman.GetComponent<MapTransformComponent>(),
                        ghost.GetComponent<MapTransformComponent>(),
                        3));
        }

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
        private void OnGhostChaseCollision()
            => GhostChaseCollision?.Invoke();

        private void OnGhostFrightenedCollision()
            => GhostFrightenedCollision?.Invoke();

        public event Action GhostChaseCollision;
        public event Action GhostFrightenedCollision;
    }
}
