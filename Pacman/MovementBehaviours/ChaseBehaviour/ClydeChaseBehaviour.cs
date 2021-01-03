using System;
using System.Collections.Generic;
using System.Linq;
using Pacman.Components;

namespace Pacman.MovementBehaviours.ChaseBehaviour
{
    /// <summary>
    /// Responsible for the chase movement for the ghost Clyde.
    /// </summary>
    public class ClydeChaseBehaviour : GhostTargetMovementBehaviour
    {
        private readonly PacmanMovementBehaviour targetMovementBehaviour;
        private readonly MapTransformComponent targetMapTransform;
        private readonly MapComponent map;
        private readonly MapTransformComponent mapTransform;

        /// <summary>
        /// Constructor, that creates a new instance of ClydeChaseBehaviour
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
        public ClydeChaseBehaviour(
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
            this.targetMapTransform = targetMapTransform;
            this.targetMovementBehaviour = targetMovementBehaviour;
            this.map = map;
            this.mapTransform = mapTransform;
        }

        /// <summary>
        /// Gets the position of the target on the map.
        /// </summary>
        protected override void GetTargetPosition()
        {
            TargetPosition = targetMapTransform.Position;
            double dist = GetAbsoluteDistance(
                                mapTransform.Position,
                                TargetPosition);
            if (dist <= 4)
            {
                int x = 0;
                int y = map.Map.GetLength(1);
                TargetPosition = new Vector2Int(x, y);
            }
        }
    }
}