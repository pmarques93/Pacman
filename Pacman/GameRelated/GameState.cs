using System;
using System.Collections.Generic;
using System.Linq;
using Pacman.Components;
using Pacman.MovementBehaviours;
using Pacman.MovementBehaviours.ChaseBehaviour;

namespace Pacman
{
    /// <summary>
    /// Class with game state information. Extends Component
    /// </summary>
    public class GameState : Component
    {
        private readonly ICollection<GameObject> ghosts;
        private readonly Collision collisions;
        private readonly MapComponent map;
        private readonly Random random;
        private readonly GameObject pacman;
        private readonly PacmanMovementBehaviour pacmanMovementBehaviour;

        /// <summary>
        /// Constructor for GameState.
        /// </summary>
        /// <param name="collision">Reference to collision.</param>
        /// <param name="pacman">Reference to pacman.</param>
        /// <param name="ghosts">Reference to ghosts.</param>
        /// <param name="map">Reference to map.</param>
        /// <param name="random">Reference to random.</param>
        /// <param name="pacmanMovementBehaviour">Reference to pacman
        /// movement behaviour</param>
        public GameState(
            Collision collision,
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

        /// <summary>
        /// Method that runs once on Start. Subscribes to events.
        /// </summary>
        public override void Start()
        {
            collisions.GhostCollision += GhostCollision;
            collisions.GhostHouseCollision += GhostOnHouse;
            collisions.GhostHouseExitCollision += GhostOnHouseExit;
            collisions.PowerPillCollision += PowerPillCollision;
        }

        /// <summary>
        /// Method that runs once on Finish. Unsubscribes to events.
        /// </summary>
        public override void Finish()
        {
            collisions.GhostCollision -= GhostCollision;
            collisions.GhostHouseCollision -= GhostOnHouse;
            collisions.GhostHouseExitCollision -= GhostOnHouseExit;
            collisions.PowerPillCollision -= PowerPillCollision;
        }

        /// <summary>
        /// Method that defines what happens when pacman collides with
        /// power pills
        /// </summary>
        private void PowerPillCollision()
        {
            foreach (GameObject ghost in ghosts)
            {
                ConsoleSprite consoleSprite =
                    ghost.GetComponent<ConsoleSprite>();
                consoleSprite.ChangeColor(ConsoleColor.Red, ConsoleColor.White);

                MapTransformComponent ghostMapTransform =
                    ghost.GetComponent<MapTransformComponent>();

                MoveComponent moveComponent =
                    ghost.GetComponent<MoveComponent>();

                moveComponent.AddMovementBehaviour(
                                                new FrightenedMovementBehaviour(
                                                    collisions,
                                                    ghost,
                                                    map,
                                                    new MapTransformComponent(
                                                        1, 1),
                                                    ghostMapTransform,
                                                    random,
                                                    3));

                MapTransformComponent tempMapTrans =
                    ghost.GetComponent<MapTransformComponent>();

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

        /// <summary>
        /// Method that defines what happens when a ghost is inside the midle
        /// house.
        /// </summary>
        /// <param name="ghost">Ghost to check.</param>
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
                    consoleSprite.ChangeColor(
                        ConsoleColor.White, ConsoleColor.Red);
                    break;
                case "pinky":
                    consoleSprite = ghost.GetComponent<ConsoleSprite>();
                    consoleSprite.ChangeColor(
                        ConsoleColor.White, ConsoleColor.DarkMagenta);
                    break;
                case "inky":
                    consoleSprite = ghost.GetComponent<ConsoleSprite>();
                    consoleSprite.ChangeColor(
                        ConsoleColor.White, ConsoleColor.Blue);
                    break;
                case "clyde":
                    consoleSprite = ghost.GetComponent<ConsoleSprite>();
                    consoleSprite.ChangeColor(
                        ConsoleColor.DarkBlue, ConsoleColor.DarkYellow);
                    break;
            }
        }

        /// <summary>
        /// Method that defines what happens when a ghost leaves the midle
        /// house.
        /// </summary>
        /// <param name="ghost">Ghost to check.</param>
        /// <param name="cell">Cell to check.</param>
        private void GhostOnHouseExit(GameObject ghost, Cell cell)
        {
            MoveComponent moveComponent = ghost.GetComponent<MoveComponent>();

            if (moveComponent.MovementState.
                HasFlag(MovementState.OutGhostHouse) ||
                moveComponent.MovementState.
                HasFlag(MovementState.Eaten))
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
                        consoleSprite.ChangeColor(
                            ConsoleColor.White,
                            ConsoleColor.Red);
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
                                3));
                        moveComponent.MovementState = MovementState.Chase;

                        consoleSprite = ghost.GetComponent<ConsoleSprite>();
                        consoleSprite.ChangeColor(
                            ConsoleColor.White,
                            ConsoleColor.DarkMagenta);
                        break;

                    case "inky":
                        MapTransformComponent blinkyMapTransform =
                            ghosts.FirstOrDefault(g => g.Name == "blinky")?.
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
                        consoleSprite.ChangeColor(
                            ConsoleColor.White,
                            ConsoleColor.Blue);
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
                        consoleSprite.ChangeColor(
                            ConsoleColor.DarkBlue,
                            ConsoleColor.DarkYellow);
                        break;
                }
            }
        }

        /// <summary>
        /// Happens once when there's a collision with a ghost.
        /// </summary>
        private void GhostCollision(GameObject ghost)
        {
            switch (ghost.GetComponent<MoveComponent>().MovementState)
            {
                case MovementState.Chase:
                    OnGhostChaseCollision();
                    break;
                case MovementState.Frightened:

                    ConsoleSprite consoleSprite =
                        ghost.GetComponent<ConsoleSprite>();

                    consoleSprite.ChangeColor(
                        ConsoleColor.Red, ConsoleColor.DarkBlue);

                    MoveComponent moveComponent =
                        ghost.GetComponent<MoveComponent>();

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
        /// On Ghost Chase collision method. Calls GhostChaseCollision event.
        /// </summary>
        private void OnGhostChaseCollision()
            => GhostChaseCollision?.Invoke();

        /// <summary>
        /// GhostChaseCollision happens when there's a collision with a ghost
        /// on chase mode.
        /// </summary>
        public event Action GhostChaseCollision;
    }
}
