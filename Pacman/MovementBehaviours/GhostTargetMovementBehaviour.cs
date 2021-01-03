using System;
using System.Collections.Generic;
using System.Linq;
using Pacman.Collisions;
using Pacman.Components;

namespace Pacman.MovementBehaviours
{
    /// <summary>
    /// Base of the movement of every ghost.
    /// </summary>
    public abstract class GhostTargetMovementBehaviour : IMovementBehaviour
    {
        private readonly TransformComponent ghostTransform;
        private readonly GameObject ghost;
        private readonly MapComponent map;
        private readonly MapTransformComponent mapTransform;
        private readonly Collision collision;
        private readonly int translateModifier;

        /// <summary>
        /// Gets or sets the position of the target.
        /// </summary>
        protected Vector2Int TargetPosition { get; set; }

        /// <summary>
        /// Constructor, that creates a new instance of
        /// GhostTargetMovementBehaviour and initializes its fields.
        /// </summary>
        /// <param name="collision">Instance of the component responsible
        /// for the collision handling.</param>
        /// <param name="ghost">Instance of the ghost to be moved.</param>
        /// <param name="map">Map in which the gameobjects are placed.</param>
        /// <param name="mapTransform"><see cref="MapTransformComponent"/>
        /// for the ghost.</param>
        /// <param name="translateModifier">Value to be a compensation of the
        /// map stretch when printed.</param>
        protected GhostTargetMovementBehaviour(
                    Collision collision,
                    GameObject ghost,
                    MapComponent map,
                    MapTransformComponent mapTransform,
                    int translateModifier = 1)
        {
            this.ghost = ghost;
            this.collision = collision;
            ghostTransform = ghost.GetComponent<TransformComponent>();
            this.mapTransform = mapTransform;
            this.translateModifier = translateModifier;
            this.map = map;
        }

        /// <summary>
        /// Moves the ghost.
        /// </summary>
        /// <param name="xMax">Horizontal limit of the map.</param>
        /// <param name="yMax">Vertical limit of the map.</param>
        public void Movement(int xMax, int yMax)
        {
            GetTargetPosition();
            Dictionary<Direction, double> directions =
                                        new Dictionary<Direction, double>();
            Dictionary<Direction, Vector2Int> directionVector =
                                    new Dictionary<Direction, Vector2Int>();

            Vector2Int upVector = new Vector2Int(
                                        mapTransform.Position.X,
                                        Math.Max(
                                        0,
                                        mapTransform.Position.Y - 1));

            Vector2Int leftVector = new Vector2Int(
                                                Math.Max(
                                                0,
                                                mapTransform.Position.X - 1),
                                                mapTransform.Position.Y);

            Vector2Int downVector = new Vector2Int(
                                            mapTransform.Position.X,
                                            Math.Min(
                                            yMax - 1,
                                            mapTransform.Position.Y + 1));

            Vector2Int rightVector = new Vector2Int(
                                                Math.Min(
                                                xMax - 1,
                                                mapTransform.Position.X + 1),
                                                mapTransform.Position.Y);

            directions.Add(
                        Direction.Up,
                        GetAbsoluteDistance(upVector, TargetPosition));

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
                if (!map.Map[
                        directionVector[d].X,
                        directionVector[d].Y].Collider.Type.HasFlag(Cell.Wall))
                {
                    if (map.Map[
                        directionVector[d].X,
                        directionVector[d].Y].
                        Collider.Type.HasFlag(Cell.GhostHouse) &&
                        !map.Map[
                        mapTransform.Position.X,
                        mapTransform.Position.Y].
                        Collider.Type.HasFlag(Cell.GhostHouse))
                    {
                        continue;
                    }

                    if ((mapTransform.Direction == Direction.Left
                        && d == Direction.Right)
                        || (mapTransform.Direction == Direction.Right
                        && d == Direction.Left)
                        || (mapTransform.Direction == Direction.Up
                        && d == Direction.Down)
                        || (mapTransform.Direction == Direction.Down
                        && d == Direction.Up))
                    {
                        continue;
                    }

                    map.Map[
                        mapTransform.Position.X,
                        mapTransform.Position.Y].Collider.Type &= ~Cell.Ghost;
                    mapTransform.Position = directionVector[d];
                    ghostTransform.Position =
                        new Vector2Int(
                                    directionVector[d].X * translateModifier,
                                    directionVector[d].Y);
                    mapTransform.Direction = d;
                    map.Map[
                        mapTransform.Position.X,
                        mapTransform.Position.Y].Collider.Type |= Cell.Ghost;

                    if (map.Map[
                        directionVector[d].X,
                        directionVector[d].Y].
                        Collider.Type.HasFlag(Cell.Pacman))
                    {
                        collision.OnGhostCollision(ghost);
                    }

                    break;
                }
            }
        }

        /// <summary>
        /// Gets the position of the target on the map.
        /// </summary>
        protected abstract void GetTargetPosition();

        /// <summary>
        /// Gets the absolute distance between two points.
        /// </summary>
        /// <param name="pos1">First point.</param>
        /// <param name="pos2">Second point.</param>
        /// <returns>The absoluta distance between the given points.</returns>
        protected double GetAbsoluteDistance(Vector2Int pos1, Vector2Int pos2)
        {
            double x = Math.Pow(pos1.X - pos2.X, 2);
            double y = Math.Pow(pos1.Y - pos2.Y, 2);
            return Math.Sqrt(x + y);
        }
    }
}