using System;
using System.Collections.Generic;
using System.Linq;

namespace Pacman.MovementBehaviours.ChaseBehaviour
{
    public class BlinkyChaseBehaviour : IMovementBehaviour
    {

        private readonly TransformComponent transform;
        private readonly MapComponent map;
        private readonly TransformComponent mapTransform;
        private int translateModifier;

        private readonly TransformComponent targetTransform;

        public BlinkyChaseBehaviour(GameObject blinky,
                                    GameObject pacMan,
                                    TransformComponent pacmanMapTransform,
                                    TransformComponent mapTransform,
                                    int translateModifier = 1)
        {
            transform = blinky.GetComponent<TransformComponent>();
            targetTransform = pacmanMapTransform;
            this.mapTransform = mapTransform;
            this.translateModifier = translateModifier;
            map = pacMan.GetComponent<MapComponent>();
        }

        public void Movement(int xMax, int yMax)
        {
            Dictionary<Direction, double> directions = new Dictionary<Direction, double>();
            Dictionary<Direction, Vector2Int> directionVector = new Dictionary<Direction, Vector2Int>();

            Vector2Int upVector = new Vector2Int(mapTransform.Position.X,
                                                Math.Max(0, mapTransform.Position.Y - 1));

            Vector2Int leftVector = new Vector2Int(Math.Max(0, mapTransform.Position.X - 1),
                                                mapTransform.Position.Y);

            Vector2Int downVector = new Vector2Int(mapTransform.Position.X,
                                                Math.Min(yMax - 1, mapTransform.Position.Y - 1));

            Vector2Int rightVector = new Vector2Int(Math.Min(xMax - 1, mapTransform.Position.X + 1),
                                                mapTransform.Position.Y);


            directions.Add(Direction.Up, GetAbsoluteDistance(
                                            upVector,
                                            targetTransform.Position));

            directionVector.Add(Direction.Up, upVector);


            directions.Add(Direction.Left, GetAbsoluteDistance(
                                            leftVector,
                                            targetTransform.Position));

            directionVector.Add(Direction.Left, leftVector);


            directions.Add(Direction.Down, GetAbsoluteDistance(
                                            downVector,
                                            targetTransform.Position));

            directionVector.Add(Direction.Down, downVector);


            directions.Add(Direction.Right, GetAbsoluteDistance(
                                            rightVector,
                                            targetTransform.Position));

            directionVector.Add(Direction.Right, rightVector);

            IEnumerable<Direction> test = directions.
                                            OrderBy(p => p.Value).
                                            Select(p => p.Key);

            foreach (Direction d in test)
            {
                if (d == Direction.Right)
                {
                    if (map.Map[
                        Math.Min(xMax - 1, mapTransform.Position.X + 1),
                        mapTransform.Position.Y].
                        Collider.Type != Cell.Wall)
                    {
                        Console.WriteLine($"Map[{mapTransform.Position.X + 1} {mapTransform.Position.Y}]");
                        mapTransform.Position =
                        new Vector2Int(
                        Math.Min(xMax - 1, mapTransform.Position.X + 1),
                        mapTransform.Position.Y);

                        transform.Position =
                            new Vector2Int(
                            Math.Min(xMax * translateModifier - 1,
                                transform.Position.X + translateModifier),
                            transform.Position.Y);
                        break;
                    }
                }

                else if (d == Direction.Left)
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
                        break;
                    }

                }
                else if (d == Direction.Down)
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
                        break;
                    }
                }
            }
        }

        private double GetAbsoluteDistance(Vector2Int pos1, Vector2Int pos2)
        {
            return Math.Sqrt(Math.Pow(Math.Abs(pos1.X - pos2.X), 2) +
                            Math.Pow(Math.Abs(pos1.Y - pos2.Y), 2));
        }
    }
}