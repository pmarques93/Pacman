using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Pacman.Collisions;
using Pacman.Components;
using Pacman.ConsoleRender;
using Pacman.MovementBehaviours;
using Pacman.MovementBehaviours.ChaseBehaviour;
using Pacman.MovementBehaviours.ScatterBehaviour;

namespace Pacman.GameRelated
{
    /// <summary>
    /// Class with game state information. Extends Component.
    /// </summary>
    public class GhostBehaviourHandler : Component
    {
        private readonly ICollection<GameObject> ghosts;
        private readonly Collision collisions;
        private readonly MapComponent map;
        private readonly Random random;
        private readonly GameObject pacman;
        private readonly PacmanMovementBehaviour pacmanMovementBehaviour;
        private readonly Timer movementChangeTimer;
        private readonly int frightenedModeTime;
        private readonly int defaultTime;
        private MovementState ghostsMovementState;

        /// <summary>
        /// Constructor for GhostBehaviourHandler.
        /// </summary>
        /// <param name="collision">Reference to collision.</param>
        /// <param name="pacman">Reference to pacman.</param>
        /// <param name="ghosts">Reference to ghosts.</param>
        /// <param name="map">Reference to map.</param>
        /// <param name="random">Reference to random.</param>
        /// <param name="pacmanMovementBehaviour">Reference to pacman
        /// movement behaviour.</param>
        public GhostBehaviourHandler(
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
            frightenedModeTime = 10000;
            defaultTime = 15000;
            movementChangeTimer = new Timer(defaultTime)
            {
                Enabled = true,
            };
            ghostsMovementState = MovementState.Scatter;
        }

        private void OnTimerChangeMovement(object sender, ElapsedEventArgs e)
        {
            switch (ghostsMovementState)
            {
                case MovementState.Chase:
                    ghostsMovementState = MovementState.Scatter;
                    ResetTimer(defaultTime);
                    break;
                case MovementState.Scatter:
                    ghostsMovementState = MovementState.Chase;
                    ResetTimer(defaultTime);
                    break;
            }

            foreach (GameObject ghost in ghosts)
            {
                MoveComponent ghostMoveComponent =
                            ghost.GetComponent<MoveComponent>();

                if (
                ghostMoveComponent.MovementState == MovementState.Scatter
                || ghostMoveComponent.MovementState == MovementState.Frightened)
                {
                    SwitchChaseMode(ghost);
                    ChangeSpriteColor(ghost);
                    InvertGhostDirection(ghost);
                    ghostMoveComponent.MovementState = MovementState.Chase;
                }
                else if (
                ghostMoveComponent.MovementState == MovementState.Chase
                || ghostMoveComponent.MovementState == MovementState.Frightened)
                {
                    SwitchScatterMode(ghost);
                    ChangeSpriteColor(ghost);
                    InvertGhostDirection(ghost);
                    ghostMoveComponent.MovementState = MovementState.Scatter;
                }
            }
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
            movementChangeTimer.Elapsed += OnTimerChangeMovement;
            OnRegisterToTimerEvent();
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

        private void OnRegisterToTimerEvent() =>
            RegisterToTimerEvent?.Invoke();

        /// <summary>
        /// Method that defines what happens when pacman collides with
        /// power pills.
        /// </summary>
        private void PowerPillCollision()
        {
            ResetTimer(frightenedModeTime);
            foreach (GameObject ghost in ghosts)
            {
                ConsoleSprite consoleSprite =
                    ghost.GetComponent<ConsoleSprite>();
                consoleSprite.ChangeColor(
                                ConsoleColor.Red,
                                ConsoleColor.White);
                ghost.GetComponent<MoveComponent>().MovementState =
                    MovementState.Frightened;
                SwitchFrightenedMode(ghost);
                InvertGhostDirection(ghost);
            }
        }

        /// <summary>
        /// Inverts the direction to which the ghost is currently pointed.
        /// </summary>
        /// <param name="ghost">Instance of the ghost whose direction
        /// will be changed.</param>
        private void InvertGhostDirection(GameObject ghost)
        {
            MapTransformComponent tempMapTrans =
                ghost.GetComponent<MapTransformComponent>();

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
            ChangeSpriteColor(ghost);
            moveComponent.MovementState = MovementState.OutGhostHouse;
        }

        /// <summary>
        /// Method that defines what happens when a ghost leaves the ghost
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
                if (ghostsMovementState == MovementState.Chase)
                {
                    SwitchChaseMode(ghost);
                    InvertGhostDirection(ghost);
                    moveComponent.MovementState = MovementState.Chase;
                }
                else if (ghostsMovementState == MovementState.Scatter
                    || moveComponent.MovementState.
                        HasFlag(MovementState.OutGhostHouse))
                {
                    SwitchScatterMode(ghost);
                    if (!moveComponent.MovementState.
                        HasFlag(MovementState.OutGhostHouse))
                    {
                        InvertGhostDirection(ghost);
                    }

                    moveComponent.MovementState = MovementState.Scatter;
                }

                ChangeSpriteColor(ghost);
                moveComponent.MovementState = MovementState.Chase;
            }
        }

        /// <summary>
        /// Changes the ghost movement to frightened.
        /// </summary>
        /// <param name="ghost">Instance of the ghost whose movement
        /// will be changed.</param>
        private void SwitchFrightenedMode(GameObject ghost)
        {
            MoveComponent moveComponent =
                ghost.GetComponent<MoveComponent>();

            MapTransformComponent ghostMapTransform =
                ghost.GetComponent<MapTransformComponent>();

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
        }

        /// <summary>
        /// Changes the ghost movement to chase.
        /// </summary>
        /// <param name="ghost">Instance of the ghost whose movement
        /// will be changed.</param>
        private void SwitchChaseMode(GameObject ghost)
        {
            MoveComponent moveComponent = ghost.GetComponent<MoveComponent>();
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
                    break;
            }
        }

        /// <summary>
        /// Changes the ghost movement to scatter.
        /// </summary>
        /// <param name="ghost">Instance of the ghost whose movement
        /// will be changed.</param>
        private void SwitchScatterMode(GameObject ghost)
        {
            MoveComponent moveComponent = ghost.GetComponent<MoveComponent>();
            int xMax = map.Map.GetLength(0);
            int yMax = map.Map.GetLength(1);

            switch (ghost.Name)
            {
                case "blinky":
                    moveComponent.AddMovementBehaviour(
                        new ScatterMovementBehaviour(
                            collisions,
                            ghost,
                            ghost.GetComponent<MapComponent>(),
                            new MapTransformComponent(0, 0),
                            ghost.GetComponent<MapTransformComponent>(),
                            3));
                    break;

                case "pinky":
                    moveComponent.AddMovementBehaviour(
                        new ScatterMovementBehaviour(
                            collisions,
                            ghost,
                            ghost.GetComponent<MapComponent>(),
                            new MapTransformComponent(xMax, 0),
                            ghost.GetComponent<MapTransformComponent>(),
                            3));
                    break;

                case "inky":
                    moveComponent.AddMovementBehaviour(
                        new ScatterMovementBehaviour(
                            collisions,
                            ghost,
                            ghost.GetComponent<MapComponent>(),
                            new MapTransformComponent(0, yMax),
                            ghost.GetComponent<MapTransformComponent>(),
                            3));
                    break;

                case "clyde":
                    moveComponent.AddMovementBehaviour(
                        new ScatterMovementBehaviour(
                            collisions,
                            ghost,
                            ghost.GetComponent<MapComponent>(),
                            new MapTransformComponent(xMax, yMax),
                            ghost.GetComponent<MapTransformComponent>(),
                            3));
                    break;
            }
        }

        /// <summary>
        /// Changes the color of the sprite of the ghosts.
        /// </summary>
        /// <param name="ghost">Instance of the ghost whose sprite color
        /// will be changed.</param>
        private void ChangeSpriteColor(GameObject ghost)
        {
            ConsoleSprite consoleSprite;
            switch (ghost.Name)
            {
                case "blinky":
                    consoleSprite = ghost.GetComponent<ConsoleSprite>();
                    consoleSprite.ChangeColor(
                        ConsoleColor.White,
                        ConsoleColor.Red);
                    break;

                case "pinky":
                    consoleSprite = ghost.GetComponent<ConsoleSprite>();
                    consoleSprite.ChangeColor(
                        ConsoleColor.White,
                        ConsoleColor.DarkMagenta);
                    break;

                case "inky":
                    consoleSprite = ghost.GetComponent<ConsoleSprite>();
                    consoleSprite.ChangeColor(
                        ConsoleColor.White,
                        ConsoleColor.Blue);
                    break;

                case "clyde":
                    consoleSprite = ghost.GetComponent<ConsoleSprite>();
                    consoleSprite.ChangeColor(
                        ConsoleColor.DarkBlue,
                        ConsoleColor.DarkYellow);
                    break;
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
                case MovementState.Scatter:
                    OnGhostChaseCollision();
                    break;
                case MovementState.Frightened:
                    ResetTimer(frightenedModeTime);
                    ConsoleSprite consoleSprite =
                        ghost.GetComponent<ConsoleSprite>();

                    consoleSprite.ChangeColor(
                        ConsoleColor.Red, ConsoleColor.DarkBlue);

                    MoveComponent moveComponent =
                        ghost.GetComponent<MoveComponent>();
                    InvertGhostDirection(ghost);
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
        /// Resets the timer and sets its interval to a given value.
        /// </summary>
        /// <param name="timerInterval">New interval for the Timer
        /// object.</param>
        private void ResetTimer(int timerInterval)
        {
            movementChangeTimer.Interval = timerInterval;
            movementChangeTimer.Stop();
            movementChangeTimer.Start();
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

        /// <summary>
        /// RegisterToTimerEvent is used on a class that wants to register
        /// to this event
        /// </summary>
        public event Action RegisterToTimerEvent;
    }
}
