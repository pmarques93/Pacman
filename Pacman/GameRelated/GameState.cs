using System;
using System.Collections.Generic;
using Pacman.Components;
using Pacman.MovementBehaviours;
using Pacman.MovementBehaviours.ChaseBehaviour;
using System.Linq;

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
        private readonly GameObject pacman;
        private readonly PacmanMovementBehaviour pacmanMovementBehaviour;

        public GameState(Collision collision,
                         GameObject pacman,
                         ICollection<GameObject> ghosts,
                         MapComponent map,
                         Random random,
                         PacmanMovementBehaviour pacmanMovementBehaviour)
        {
            collisions = collision;
            this.pacman = pacman;
            this.ghosts = ghosts;
            this.map = map;
            this.random = random;
            this.pacmanMovementBehaviour = pacmanMovementBehaviour;
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
                ConsoleSprite consoleSprite = ghost.GetComponent<ConsoleSprite>();
                consoleSprite.ChangeColor(ConsoleColor.Red, ConsoleColor.White);

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

            ConsoleSprite consoleSprite;
            switch (ghost.Name)
            {
                case "blinky":
                    consoleSprite = ghost.GetComponent<ConsoleSprite>();
                    consoleSprite.ChangeColor(ConsoleColor.White, ConsoleColor.Red);
                    break;
                case "pinky":
                    consoleSprite = ghost.GetComponent<ConsoleSprite>();
                    consoleSprite.ChangeColor(ConsoleColor.White, ConsoleColor.DarkMagenta);
                    break;
                case "inky":
                    consoleSprite = ghost.GetComponent<ConsoleSprite>();
                    consoleSprite.ChangeColor(ConsoleColor.White, ConsoleColor.Blue);
                    break;
                case "clyde":
                    consoleSprite = ghost.GetComponent<ConsoleSprite>();
                    consoleSprite.ChangeColor(ConsoleColor.DarkBlue, ConsoleColor.DarkYellow);
                    break;
            }
        }
        private void GhostOnHouseExit(GameObject ghost, Cell cell)
        {
            MoveComponent moveComponent = ghost.GetComponent<MoveComponent>();
            if (moveComponent.MovementState.HasFlag(MovementState.OutGhostHouse) ||
                moveComponent.MovementState.HasFlag(MovementState.Eaten))
            {
                ConsoleSprite consoleSprite;
                switch (ghost.Name)
                {
                    case "blinky":
                        moveComponent.AddMovementBehaviour(
                            new BlinkyChaseBehaviour(
                                collisions,
                                ghost,
                                ghost.GetComponent<MapComponent>(),
                                pacman.GetComponent<MapTransformComponent>(),
                                ghost.GetComponent<MapTransformComponent>(),
                                3));
                        moveComponent.MovementState = MovementState.Chase;

                        consoleSprite = ghost.GetComponent<ConsoleSprite>();
                        consoleSprite.ChangeColor(ConsoleColor.White, ConsoleColor.Red);
                        break;
                    case "pinky":
                        moveComponent.AddMovementBehaviour(
                            new PinkyChaseBehaviour(
                                collisions,
                                pacmanMovementBehaviour,
                                ghost,
                                map,
                                pacman.GetComponent<MapTransformComponent>(),
                                ghost.GetComponent<MapTransformComponent>(),
                                3
                            )
                        );
                        moveComponent.MovementState = MovementState.Chase;

                        consoleSprite = ghost.GetComponent<ConsoleSprite>();
                        consoleSprite.ChangeColor(ConsoleColor.White, ConsoleColor.DarkMagenta);
                        break;

                    case "inky":
                        MapTransformComponent blinkyMapTransform = ghosts.
                                            Where(g => g.Name == "blinky").
                                            FirstOrDefault().
                                            GetComponent<MapTransformComponent>();

                        moveComponent.AddMovementBehaviour(
                            new InkyChaseBehaviour(
                                collisions,
                                pacmanMovementBehaviour,
                                map,
                                pacman.GetComponent<MapTransformComponent>(),
                                blinkyMapTransform,
                                ghost,
                                ghost.GetComponent<MapTransformComponent>(),
                                3));
                        moveComponent.MovementState = MovementState.Chase;

                        consoleSprite = ghost.GetComponent<ConsoleSprite>();
                        consoleSprite.ChangeColor(ConsoleColor.White, ConsoleColor.Blue);
                        break;
                    case "clyde":
                        moveComponent.AddMovementBehaviour(
                            new ClydeChaseBehaviour(
                                collisions,
                                pacmanMovementBehaviour,
                                ghost,
                                map,
                                pacman.GetComponent<MapTransformComponent>(),
                                ghost.GetComponent<MapTransformComponent>(),
                                3));
                        moveComponent.MovementState = MovementState.Chase;
                        consoleSprite = ghost.GetComponent<ConsoleSprite>();
                        consoleSprite.ChangeColor(ConsoleColor.DarkBlue, ConsoleColor.DarkYellow);
                        break;
                }
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

                    ConsoleSprite consoleSprite = ghost.GetComponent<ConsoleSprite>();
                    consoleSprite.ChangeColor(ConsoleColor.Red, ConsoleColor.DarkBlue);

                    MoveComponent moveComponent = ghost.GetComponent<MoveComponent>();
                    moveComponent.MovementState = MovementState.Eaten;
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
