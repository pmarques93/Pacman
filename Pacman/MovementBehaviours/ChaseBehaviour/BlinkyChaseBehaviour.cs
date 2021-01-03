using Pacman.Components;

namespace Pacman.MovementBehaviours.ChaseBehaviour
{
    /// <summary>
    /// Responsible for the chase movement for the ghost Blinky.
    /// </summary>
    public class BlinkyChaseBehaviour : GhostTargetMovementBehaviour
    {
        private readonly MapTransformComponent targetMapTransform;

        /// <summary>
        /// Constructor, that creates a new instance of BlinkyChaseBehaviour
        /// and initializes its fields.
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
        /// <param name="translateModifier">Value to be a compensation of the
        /// map stretch when printed.</param>
        public BlinkyChaseBehaviour(
                    Collision collision,
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