using System;
using Pacman.Components;

namespace Pacman.MovementBehaviours
{
    /// <summary>
    /// Responsible for the movement of the ghosts when in frightened mode.
    /// </summary>
    public class FrightenedMovementBehaviour : GhostTargetMovementBehaviour
    {
        private readonly Random random;
        private readonly MapComponent map;

        /// <summary>
        /// Constructor, that creates a new instance of
        /// FrightenedMovementBehaviour and initializes its fields.
        /// </summary>
        /// <param name="collision">Instance of the component responsible
        /// for the collision handling.</param>
        /// <param name="ghost">Instance of the ghost to be moved.</param>
        /// <param name="map">Map in which the gameobjects are placed.</param>
        /// <param name="targetMapTransform">
        /// <see cref="MapTransformComponent"/> for the target to
        /// be chased.</param>
        /// <param name="mapTransform"><see cref="MapTransformComponent"/>
        /// for the ghost.</param>
        /// <param name="random">Instance of a pseudo-random number 
        /// generator.</param>
        /// <param name="translateModifier">Value to be a compensation of the
        /// map stretch when printed.</param>
        public FrightenedMovementBehaviour(
                    Collision collision,
                    GameObject ghost,
                    MapComponent map,
                    MapTransformComponent targetMapTransform,
                    MapTransformComponent mapTransform,
                    Random random,
                    int translateModifier = 1)
                    : base(
                        collision,
                        ghost,
                        map,
                        mapTransform,
                        translateModifier)
        {
            this.map = map;
            this.random = random;
        }

        /// <summary>
        /// Get the position of the target on the map.
        /// </summary>
        protected override void GetTargetPosition()
        {
            int xMax = map.Map.GetLength(0);
            int yMax = map.Map.GetLength(1);

            Vector2Int topLeftVector = new Vector2Int(0, 0);

            Vector2Int topRightVector = new Vector2Int(xMax, 0);

            Vector2Int downRightVector = new Vector2Int(xMax, yMax);

            Vector2Int downLeftVector = new Vector2Int(0, yMax);

            Vector2Int[] vectors = new Vector2Int[4]
            {
                topLeftVector,
                topRightVector,
                downRightVector,
                downLeftVector,
            };

            int index = random.Next(4);
            TargetPosition = vectors[index];
        }
    }
}