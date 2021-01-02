using System;
using System.Collections.Generic;
using Pacman.Components;
using Pacman.MovementBehaviours;
using Pacman.MovementBehaviours.ChaseBehaviour;

namespace Pacman
{
    /// <summary>
    /// Class with game state information
    /// </summary>
    public class GameState : Component
    {
        private readonly ICollection<GameObject> ghosts;
        private readonly Collision collisions;
        private readonly MapComponent map;
        private readonly Random random;
        private GameObject pacman;
        /// NOT AVAILABLE YET ////////////////////////////////////
        private readonly GameObject blinky;
        private readonly GameObject pinky;
        private readonly GameObject inky;
        private readonly GameObject clyde;

        public GameState(Collision collision,
                         GameObject pacman,
                         ICollection<GameObject> ghosts,
                         MapComponent map,
                         Random random)
        {
            collisions = collision;
            this.pacman = pacman;
            this.ghosts = ghosts;
            this.map = map;
            this.random = random;
        }

        public override void Start()
        {
            collisions.GhostCollision += GhostCollision;
            collisions.GhostHouseCollision += GhostOnHouse;
            collisions.GhostHouseExitCollision += GhostOnHouseExit;
            collisions.PowerPillCollision += PowerPillCollision;
        }
        public override void Finish()
        {
            collisions.GhostCollision -= GhostCollision;
            collisions.GhostHouseCollision -= GhostOnHouse;
            collisions.GhostHouseExitCollision -= GhostOnHouseExit;
            collisions.PowerPillCollision -= PowerPillCollision;
        }

        private void PowerPillCollision()
        {
            foreach (GameObject ghost in ghosts)
            {
                MapTransformComponent ghostMapTransform = ghost.GetComponent<MapTransformComponent>();
                MoveComponent moveComponent = ghost.GetComponent<MoveComponent>();
                moveComponent.AddMovementBehaviour(
                                                new FrightenedMovementBehaviour(
                                                    collisions,
                                                    ghost,
                                                    map,
                                                    new MapTransformComponent(1, 1),
                                                    ghostMapTransform, random, 3));
                MapTransformComponent tempMapTrans = ghost.GetComponent<MapTransformComponent>();
                moveComponent.MovementState = MovementState.Frightened;
                switch (tempMapTrans.Direction)
                {
                    case Direction.Down:
                        tempMapTrans.Direction = Direction.Up;
                        break;
                    case Direction.Up:
                        tempMapTrans.Direction = Direction.Down;
                        break;
                    case Direction.Left:
                        tempMapTrans.Direction = Direction.Right;
                        break;
                    case Direction.Right:
                        tempMapTrans.Direction = Direction.Left;
                        break;
                }
            }
        }

        private void GhostOnHouse(GameObject ghost)
        {
            MoveComponent moveComponent = ghost.GetComponent<MoveComponent>();

            moveComponent.AddMovementBehaviour(
                   new BlinkyChaseBehaviour(
                       collisions,
                       ghost,
                       ghost.GetComponent<MapComponent>(),
                       new MapTransformComponent(13, 11),
                       ghost.GetComponent<MapTransformComponent>(),
                       3));
            moveComponent.MovementState = MovementState.OutGhostHouse;
            // moveComponent.MovementState = MovementState.Chase;

        }
        private void GhostOnHouseExit(GameObject ghost, Cell cell)
        {
            MoveComponent moveComponent = ghost.GetComponent<MoveComponent>();
            if (moveComponent.MovementState.HasFlag(MovementState.OutGhostHouse))
            {
                moveComponent.AddMovementBehaviour(
                        new BlinkyChaseBehaviour(
                            collisions,
                            ghost,
                            ghost.GetComponent<MapComponent>(),
                            pacman.GetComponent<MapTransformComponent>(),
                            ghost.GetComponent<MapTransformComponent>(),
                            3));
            }
        }

        /// <summary>
        /// Happens once when there's a collision with a ghost
        /// </summary>
        private void GhostCollision(GameObject ghost)
        {
            switch (ghost.GetComponent<MoveComponent>().MovementState)
            {
                case MovementState.Chase:
                    OnGhostChaseCollision();
                    break;
                case MovementState.Frightened:
                    MoveComponent moveComponent = ghost.GetComponent<MoveComponent>();
                    moveComponent.AddMovementBehaviour(
                    new BlinkyChaseBehaviour(
                        collisions,
                        ghost,
                        map,
                        new MapTransformComponent(13, 11),
                        ghost.GetComponent<MapTransformComponent>(),
                        3));
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
