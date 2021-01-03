using System;
using System.Collections.Generic;
using System.Linq;
using Pacman.Collisions;
using Pacman.Components;

namespace Pacman.MovementBehaviours.ChaseBehaviour
{
    /// <summary>
    /// Responsible for the chase movement for the ghost Pinky.
    /// </summary>
    public class PinkyChaseBehaviour : GhostTargetMovementBehaviour
    {
        private readonly PacmanMovementBehaviour targetMovementBehaviour;
        private readonly MapTransformComponent targetMapTransform;

        /// <summary>
        /// Constructor, that creates a new instance of PinkyChaseBehaviour
        /// and initializes its fields.
        /// </summary>
        /// <param name="collision">Instance of the component responsible
        /// for the collision handling.</param>
        /// <param name="targetMovementBehaviour">Movement behaviour for
        /// the target.</param>
        /// <param name="ghost">Instance of the ghost to be moved.</param>
        /// <param name="map">Map in which the gameobjects are placed.</param>
        /// <param name="targetMapTransform">
        /// <see cref="MapTransformComponent"/> for the target to
        /// be chased.</param>
        /// <param name="mapTransform"><see cref="MapTransformComponent"/>
        /// for the ghost.</param>
        /// <param name="translateModifier">Value to be a compensation of the
        /// map stretch when printed.</param>
        public PinkyChaseBehaviour(
                    Collision collision,
                    PacmanMovementBehaviour targetMovementBehaviour,
                    GameObject ghost,
                    MapComponent map,
                    MapTransformComponent targetMapTransform,
                    MapTransformComponent mapTransform,
                    int translateModifier = 1)
                    : base(
                        collision,
                        ghost,
                        map,
                        mapTransform,
                        translateModifier)
        {
            this.targetMovementBehaviour = targetMovementBehaviour;
            this.targetMapTransform = targetMapTransform;
        }

        /// <summary>
        /// Gets the position of the target on the map.
        /// </summary>
        protected override void GetTargetPosition()
        {
            TargetPosition = targetMapTransform.Position;

            switch (targetMovementBehaviour.PacmanDirection)
            {
                case Direction.Up:
                    TargetPosition += new Vector2Int(-12, -4);
                    break;
                case Direction.Down:
                    TargetPosition += new Vector2Int(0, 4);
                    break;
                case Direction.Right:
                    TargetPosition += new Vector2Int(12, 0);
                    break;
                case Direction.Left:
                    TargetPosition += new Vector2Int(-12, 0);
                    break;
            }
        }
    }
}