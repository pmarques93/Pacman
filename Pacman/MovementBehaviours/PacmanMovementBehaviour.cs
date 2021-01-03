using System;
using Pacman.Components;

namespace Pacman.MovementBehaviours
{
    /// <summary>
    /// Responsible for the movement of the pacman.
    /// </summary>
    public class PacmanMovementBehaviour : IMovementBehaviour
    {
        // Components
        private readonly KeyReaderComponent keyReader;
        private readonly TransformComponent transform;

        private readonly int translateModifier;

        private readonly MapTransformComponent mapTransform;
        private readonly MapComponent map;

        /// <summary>
        /// Gets the pacman direction.
        /// </summary>
        public Direction PacmanDirection { get; private set; }

        private Direction previousDirection;

        /// <summary>
        /// Constructor, that creates a new instance of PacmanMovementBehaviour
        /// and initializes its fields.
        /// </summary>
        /// <param name="pacman">Instance of pacman.</param>
        /// <param name="mapTransform"><see cref="MapTransformComponent"/>
        /// of pacman.</param>
        /// <param name="translateModifier">Value to be a compensation of the
        /// map stretch when printed.</param>
        public PacmanMovementBehaviour(
                        GameObject pacman,
                        MapTransformComponent mapTransform,
                        int translateModifier = 1)
        {
            keyReader = pacman.GetComponent<KeyReaderComponent>();
            transform = pacman.GetComponent<TransformComponent>();
            this.mapTransform = mapTransform;
            map = pacman.GetComponent<MapComponent>();
            this.translateModifier = translateModifier;
            previousDirection = Direction.None;
        }

        /// <summary>
        /// Gets components from pacman gameobject.
        /// </summary>
        /// <param name="pacman">Object to get components from.</param>
        /// <param name="moveComponent">Move component to add
        /// this behaviour to.</param>
        public PacmanMovementBehaviour(
                                        GameObject pacman,
                                        MoveComponent moveComponent)
        {
            keyReader = pacman.GetComponent<KeyReaderComponent>();
            transform = pacman.GetComponent<TransformComponent>();
            map = pacman.GetComponent<MapComponent>();
            moveComponent.AddMovementBehaviour(this);
        }

        /// <summary>
        /// Movement method for pacman.
        /// </summary>
        /// <param name="xMax">Horizontal map size.</param>
        /// <param name="yMax">Vertical map size.</param>
        public void Movement(int xMax, int yMax)
        {
            Direction keyPressed = keyReader.Direction;

            // When the user presses a key, pacman changes direction
            if (keyPressed != Direction.None
                && keyPressed != previousDirection)
            {
                PacmanDirection = keyPressed;
            }

            if (PacmanDirection != Direction.None)
            {
                while (true)
                {
                    if (PacmanDirection == Direction.Up)
                    {
                        if (!map.Map[
                            mapTransform.Position.X,
                            Math.Max(
                            0,
                            mapTransform.Position.Y - 1)].
                            Collider.Type.HasFlag(Cell.Wall))
                        {
                            map.Map[mapTransform.Position.X, mapTransform.Position.Y].Collider.Type &= ~Cell.Pacman;
                            transform.Position =
                            new Vector2Int(
                                transform.Position.X,
                                Math.Max(0, transform.Position.Y - 1));

                            mapTransform.Position =
                            new Vector2Int(
                                mapTransform.Position.X,
                                Math.Max(0, mapTransform.Position.Y - 1));
                            previousDirection = PacmanDirection;

                            map.Map[
                                mapTransform.Position.X,
                                mapTransform.Position.Y].
                                Collider.Type |= Cell.Pacman;

                            break;
                        }
                        else
                        {
                            if (PacmanDirection == previousDirection ||
                                previousDirection == Direction.None)
                                break;
                            PacmanDirection = previousDirection;
                            continue;
                        }
                    }

                    if (PacmanDirection == Direction.Right)
                    {
                        if (!map.Map[
                            Math.Min(xMax - 1, mapTransform.Position.X + 1),
                            mapTransform.Position.Y].
                            Collider.Type.HasFlag(Cell.Wall))
                        {
                            map.Map[
                                mapTransform.Position.X,
                                mapTransform.Position.Y].
                                Collider.Type &= ~Cell.Pacman;

                            mapTransform.Position =
                            new Vector2Int(
                            Math.Min(
                            xMax - 1,
                            mapTransform.Position.X + 1),
                            mapTransform.Position.Y);

                            transform.Position =
                                new Vector2Int(
                                    Math.Min(
                                    (xMax * translateModifier) - 1,
                                    transform.Position.X + translateModifier),
                                    transform.Position.Y);

                            previousDirection = PacmanDirection;
                            map.Map[
                                mapTransform.Position.X,
                                mapTransform.Position.Y].
                                Collider.Type |= Cell.Pacman;
                            break;
                        }
                        else
                        {
                            if (PacmanDirection == previousDirection ||
                                previousDirection == Direction.None)
                                break;
                            PacmanDirection = previousDirection;
                            continue;
                        }
                    }

                    if (PacmanDirection == Direction.Down)
                    {
                        if (!map.Map[
                            mapTransform.Position.X,
                            Math.Min(
                            yMax - 1,
                            mapTransform.Position.Y + 1)].
                            Collider.Type.HasFlag(Cell.Wall))
                        {
                            map.Map[
                                mapTransform.Position.X,
                                mapTransform.Position.Y].
                                Collider.Type &= ~Cell.Pacman;
                            transform.Position =
                            new Vector2Int(
                                transform.Position.X,
                                Math.Min(yMax - 1, transform.Position.Y + 1));

                            mapTransform.Position =
                            new Vector2Int(
                                mapTransform.Position.X,
                                Math.Min(
                                    yMax - 1,
                                    mapTransform.Position.Y + 1));
                            previousDirection = PacmanDirection;
                            map.Map[
                                mapTransform.Position.X,
                                mapTransform.Position.Y].
                                Collider.Type |= Cell.Pacman;
                            break;
                        }
                        else
                        {
                            if (PacmanDirection == previousDirection ||
                                previousDirection == Direction.None)
                                break;
                            PacmanDirection = previousDirection;
                            continue;
                        }
                    }

                    if (PacmanDirection == Direction.Left)
                    {
                        if (!map.Map[
                            Math.Max(0, mapTransform.Position.X - 1),
                            mapTransform.Position.Y].
                            Collider.Type.HasFlag(Cell.Wall))
                        {
                            map.Map[
                                mapTransform.Position.X,
                                mapTransform.Position.Y].
                                Collider.Type &= ~Cell.Pacman;
                            mapTransform.Position =
                            new Vector2Int(
                            Math.Max(0, mapTransform.Position.X - 1),
                            mapTransform.Position.Y);

                            transform.Position =
                            new Vector2Int(
                            Math.Max(
                                    0,
                                    transform.Position.X - translateModifier),
                            transform.Position.Y);
                            previousDirection = PacmanDirection;
                            map.Map[
                                mapTransform.Position.X,
                                mapTransform.Position.Y].
                                Collider.Type |= Cell.Pacman;
                            break;
                        }
                        else
                        {
                            if (PacmanDirection == previousDirection ||
                                previousDirection == Direction.None)
                                break;
                            PacmanDirection = previousDirection;
                        }
                    }
                }
            }
        }
    }
}