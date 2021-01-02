using System;
using System.Collections.Generic;
using System.Linq;
using Pacman.Components;

namespace Pacman.MovementBehaviours
{
    public abstract class GhostTargetMovementBehaviour : IMovementBehaviour
    {

        private readonly TransformComponent ghostTransform;
        protected readonly MapComponent map;
        protected readonly MapTransformComponent mapTransform;

        private readonly Collision collision;
        private int translateModifier;

        private Direction CurrentDirection { get; set; }

        protected readonly MapTransformComponent targetTransform;
        protected Vector2Int TargetPosition { get; set; }

        public GhostTargetMovementBehaviour(Collision collision,
                                    GameObject ghost,
                                    MapComponent map,
                                    MapTransformComponent targetMapTransform,
                                    MapTransformComponent mapTransform,
                                    int translateModifier = 1)
        {
            this.collision = collision;
            ghostTransform = ghost.GetComponent<TransformComponent>();
            targetTransform = targetMapTransform;
            this.mapTransform = mapTransform;
            this.translateModifier = translateModifier;
            this.map = map;
            CurrentDirection = Direction.None;
        }

        public void Movement(int xMax, int yMax)
        {
            GetTargetPosition();
            Dictionary<Direction, double> directions =
                                        new Dictionary<Direction, double>();
            Dictionary<Direction, Vector2Int> directionVector =
                                        new Dictionary<Direction, Vector2Int>();

            Vector2Int upVector = new Vector2Int(mapTransform.Position.X,
                                                    Math.Max(0,
                                                        mapTransform.
                                                        Position.Y - 1));

            Vector2Int leftVector = new Vector2Int(Math.Max(0,
                                                    mapTransform.
                                                    Position.X - 1),
                                                    mapTransform.Position.Y);

            Vector2Int downVector = new Vector2Int(mapTransform.Position.X,
                                                    Math.Min(yMax - 1,
                                                        mapTransform.
                                                        Position.Y + 1));

            Vector2Int rightVector = new Vector2Int(Math.Min(xMax - 1,
                                                    mapTransform.
                                                    Position.X + 1),
                                                    mapTransform.Position.Y);

            directions.Add(Direction.Up, GetAbsoluteDistance(upVector,
                                                    TargetPosition));

            directionVector.Add(Direction.Up, upVector);


            directions.Add(Direction.Left, GetAbsoluteDistance(
                                            leftVector,
                                            TargetPosition));

            directionVector.Add(Direction.Left, leftVector);


            directions.Add(Direction.Down, GetAbsoluteDistance(
                                            downVector,
                                            TargetPosition));

            directionVector.Add(Direction.Down, downVector);


            directions.Add(Direction.Right, GetAbsoluteDistance(
                                            rightVector,
                                            TargetPosition));

            directionVector.Add(Direction.Right, rightVector);


            IEnumerable<Direction> test = directions.
                                            OrderBy(p => p.Value).
                                            Select(p => p.Key);

            foreach (Direction d in test)
            {
                if (!map.Map[directionVector[d].X,
                        directionVector[d].Y].Collider.Type.HasFlag(Cell.Wall) ||
                    (map.Map[
                        directionVector[d].X,
                        directionVector[d].Y].Collider.Type.HasFlag(Cell.GhostHouse) &&
                    map.Map[
                        mapTransform.Position.X,
                        mapTransform.Position.Y].Collider.Type.HasFlag(Cell.GhostHouse)))
                {
                    if (mapTransform.Direction == Direction.Left
                        && d == Direction.Right
                        || mapTransform.Direction == Direction.Right
                        && d == Direction.Left
                        || mapTransform.Direction == Direction.Up
                        && d == Direction.Down
                        || mapTransform.Direction == Direction.Down
                        && d == Direction.Up)
                    {
                        continue;
                    }

                    map.Map[mapTransform.Position.X, mapTransform.Position.Y].Collider.Type &= ~Cell.Ghost;
                    mapTransform.Position = directionVector[d];
                    ghostTransform.Position = new Vector2Int(directionVector[d].X
                                                        * translateModifier,
                                                        directionVector[d].Y);
                    mapTransform.Direction = d;
                    map.Map[mapTransform.Position.X, mapTransform.Position.Y].Collider.Type |= Cell.Ghost;
                    if (map.Map[directionVector[d].X,
                            directionVector[d].Y].Collider.Type.HasFlag(Cell.Pacman))
                    {
                        collision.OnGhostCollision();
                    }
                    break;
                }
            }
        }

        protected abstract void GetTargetPosition();
        protected double GetAbsoluteDistance(Vector2Int pos1, Vector2Int pos2)
        {
            double x = Math.Pow((pos1.X - pos2.X), 2);
            double y = Math.Pow((pos1.Y - pos2.Y), 2);
            return Math.Sqrt(x + y);
        }
    }
}