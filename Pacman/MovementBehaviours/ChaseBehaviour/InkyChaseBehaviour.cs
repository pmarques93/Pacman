using Pacman.Components;

namespace Pacman.MovementBehaviours.ChaseBehaviour
{
    /// <summary>
    /// Responsible for the chase movement for the ghost Inky.
    /// </summary>
    public class InkyChaseBehaviour : GhostTargetMovementBehaviour
    {
        private readonly PacmanMovementBehaviour targetMovementBehaviour;
        private readonly MapTransformComponent blinkyMapTransform;
        private readonly MapTransformComponent targetMapTransform;

        /// <summary>
        /// Constructor, that creates a new instance of InkyChaseBehaviour
        /// and initializes its fields.
        /// </summary>
        /// <param name="collision">Instance of the component responsible
        /// for the collision handling.</param>
        /// <param name="targetMovementBehaviour">Movement behaviour for
        /// the target.</param>
        /// <param name="map">Map in which the gameobjects are placed.</param>
        /// <param name="targetMapTransform">
        /// <see cref="MapTransformComponent"/> for the target to
        /// be chased.</param>
        /// <param name="blinkyMapTransform">
        /// <see cref="MapTransformComponent"/> for the ghost Blinky.</param>
        /// <param name="inky">Instance of the ghost to be moved.</param>
        /// <param name="mapTransform"><see cref="MapTransformComponent"/>
        /// for the ghost.</param>
        /// <param name="translateModifier">Value to be a compensation of the
        /// map stretch when printed.</param>
        public InkyChaseBehaviour(
                            Collision collision,
                            PacmanMovementBehaviour targetMovementBehaviour,
                            MapComponent map,
                            MapTransformComponent targetMapTransform,
                            MapTransformComponent blinkyMapTransform,
                            GameObject inky,
                            MapTransformComponent mapTransform,
                            int translateModifier = 1)
                            : base(
                                collision,
                                inky,
                                map,
                                mapTransform,
                                translateModifier)
        {
            this.targetMapTransform = targetMapTransform;
            this.targetMovementBehaviour = targetMovementBehaviour;
            this.blinkyMapTransform = blinkyMapTransform;
        }

        /// <summary>
        /// Gets the position of the target on the map.
        /// </summary>
        protected override void GetTargetPosition()
        {
            Vector2Int pacmanPosition = targetMapTransform.Position;
            Vector2Int blinkyPosition = blinkyMapTransform.Position;

            switch (targetMovementBehaviour.PacmanDirection)
            {
                case Direction.Up:
                    pacmanPosition += new Vector2Int(-6, -2);
                    break;
                case Direction.Down:
                    pacmanPosition += new Vector2Int(0, 2);
                    break;
                case Direction.Right:
                    pacmanPosition += new Vector2Int(6, 0);
                    break;
                case Direction.Left:
                    pacmanPosition += new Vector2Int(-6, 0);
                    break;
            }

            Vector2Int tempBlinkyPosition = blinkyPosition - pacmanPosition;
            tempBlinkyPosition = new Vector2Int(
                                        tempBlinkyPosition.X * -1,
                                        tempBlinkyPosition.Y * -1);
            TargetPosition = tempBlinkyPosition + pacmanPosition;
        }
    }
}