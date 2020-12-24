using System;

namespace Pacman
{
    /// <summary>
    /// Class for pacman movement. Implements IMovementBehaviour
    /// </summary>
    public class PacmanMovementBehaviour : IMovementBehaviour
    {
        // Last Key pressed for continuous movement
        public Direction pacmanDirection { get; private set; }

        private Direction previousDirection;

        // Components
        private readonly KeyReaderComponent keyReader;
        private readonly TransformComponent transform;

        private int translateModifier;


        private readonly TransformComponent mapTransform;
        private readonly MapComponent map;

        /// <summary>
        /// Gets components from pacman gameobject
        /// </summary>
        /// <param name="pacman">Object to get components from</param>
        public PacmanMovementBehaviour(GameObject pacman,
                                       TransformComponent mapTransform,
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
        /// Gets components from pacman gameobject
        /// </summary>
        /// <param name="pacman">Object to get components from</param>
        /// <param name="moveComponent">Move component to add
        /// this behaviour to</param>
        public PacmanMovementBehaviour(GameObject pacman,
            MoveComponent moveComponent)
        {
            keyReader = pacman.GetComponent<KeyReaderComponent>();
            transform = pacman.GetComponent<TransformComponent>();
            map = pacman.GetComponent<MapComponent>();

            moveComponent.AddMovementBehaviour(this);
        }

        /// <summary>
        /// Movement method for pacman
        /// </summary>
        /// <param name="xMax">X map size</param>
        /// <param name="yMax">Y map size</param>
        public void Movement(int xMax, int yMax)
        {
            Direction keyPressed = keyReader.Direction;

            // When the user presses a key, pacman changes direction
            if (keyPressed != Direction.None && keyPressed != previousDirection)
                pacmanDirection = keyPressed;

            if (pacmanDirection != Direction.None)
            {
                while (true)
                {
                    if (pacmanDirection == Direction.Up)
                    {
                        if (map.Map[mapTransform.Position.X,
                            Math.Max(0, mapTransform.Position.Y - 1)].
                            Collider.Type != Cell.Wall)
                        {
                            transform.Position =
                            new Vector2Int(transform.Position.X,
                            Math.Max(0, transform.Position.Y - 1));

                            mapTransform.Position =
                            new Vector2Int(mapTransform.Position.X,
                            Math.Max(0, mapTransform.Position.Y - 1));
                            previousDirection = pacmanDirection;
                            break;
                        }
                        else
                        {
                            if (pacmanDirection == previousDirection ||
                                previousDirection == Direction.None)
                                break;
                            pacmanDirection = previousDirection;
                            continue;
                        }
                    }
                    if (pacmanDirection == Direction.Right)
                    {
                        if (map.Map[
                            Math.Min(xMax - 1, mapTransform.Position.X + 1),
                            mapTransform.Position.Y].
                            Collider.Type != Cell.Wall)
                        {

                            mapTransform.Position =
                            new Vector2Int(
                            Math.Min(xMax - 1, mapTransform.Position.X + 1),
                            mapTransform.Position.Y);

                            transform.Position =
                                new Vector2Int(
                                Math.Min(xMax * translateModifier - 1,
                                    transform.Position.X + translateModifier),
                                transform.Position.Y);

                            previousDirection = pacmanDirection;
                            break;
                        }
                        else
                        {
                            if (pacmanDirection == previousDirection ||
                                previousDirection == Direction.None)
                                break;
                            pacmanDirection = previousDirection;
                            continue;
                        }
                    }

                    if (pacmanDirection == Direction.Down)
                    {
                        if (map.Map[mapTransform.Position.X,
                            Math.Min(yMax - 1, mapTransform.Position.Y + 1)].
                            Collider.Type != Cell.Wall)
                        {
                            transform.Position =
                            new Vector2Int(transform.Position.X,
                            Math.Min(yMax - 1, transform.Position.Y + 1));

                            mapTransform.Position =
                            new Vector2Int(mapTransform.Position.X,
                            Math.Min(yMax - 1, mapTransform.Position.Y + 1));
                            previousDirection = pacmanDirection;
                            break;
                        }
                        else
                        {
                            if (pacmanDirection == previousDirection ||
                                previousDirection == Direction.None)
                                break;
                            pacmanDirection = previousDirection;
                            continue;
                        }
                    }

                    if (pacmanDirection == Direction.Left)
                    {
                        if (map.Map[
                            Math.Max(0, mapTransform.Position.X - 1),
                            mapTransform.Position.Y].
                            Collider.Type != Cell.Wall)
                        {
                            mapTransform.Position =
                            new Vector2Int(
                            Math.Max(0, mapTransform.Position.X - 1),
                            mapTransform.Position.Y);


                            transform.Position =
                            new Vector2Int(
                            Math.Max(0,
                                    transform.Position.X - translateModifier),
                            transform.Position.Y);
                            previousDirection = pacmanDirection;
                            break;
                        }
                        else
                        {
                            if (pacmanDirection == previousDirection ||
                                previousDirection == Direction.None)
                                break;
                            pacmanDirection = previousDirection;
                            continue;
                        }
                    }
                }
            }
        }
    }
}
