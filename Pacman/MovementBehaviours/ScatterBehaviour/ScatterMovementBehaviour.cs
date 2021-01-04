using Pacman.Collisions;
using Pacman.Components;

namespace Pacman.MovementBehaviours.ScatterBehaviour
{
    /// <summary>
    /// Responsible for the scatter movement for the ghosts.
    /// </summary>
    public class ScatterMovementBehaviour : GhostTargetMovementBehaviour
    {
        private readonly MapTransformComponent targetMapTransform;

        /// <summary>
        /// Constructor, that creates a new instance of
        /// ScatterMovementBehaviour and initializes its fields.
        /// </summary>
        /// <param name="ghost">Instance of the ghost to be moved.</param>
        /// <param name="map">Map in which the gameobjects are placed.</param>
        /// <param name="targetMapTransform">
        /// <see cref="MapTransformComponent"/> for the target to
        /// be chased.</param>
        /// <param name="mapTransform"><see cref="MapTransformComponent"/>
        /// for the ghost.</param>
        /// <param name="translateModifier">Value to be a compensation of the
        /// map stretch when printed.</param>
        public ScatterMovementBehaviour(
                                    GameObject ghost,
                                    MapComponent map,
                                    MapTransformComponent targetMapTransform,
                                    MapTransformComponent mapTransform,
                                    int translateModifier = 1)
                                    : base(
                                        ghost,
                                        map,
                                        mapTransform,
                                        translateModifier)
        {
            this.targetMapTransform = targetMapTransform;
        }

        /// <summary>
        /// Gets the position of the target on the map.
        /// </summary>
        protected override void GetTargetPosition()
        {
            TargetPosition = targetMapTransform.Position;
        }
    }
}