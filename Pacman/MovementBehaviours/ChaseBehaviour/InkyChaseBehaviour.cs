
using Pacman.Components;

namespace Pacman.MovementBehaviours.ChaseBehaviour
{
    public class InkyChaseBehaviour : GhostTargetMovementBehaviour
    {
        private PacmanMovementBehaviour pacmanMovementBehaviour;
        private MapTransformComponent blinkyMapTransform;
        public InkyChaseBehaviour(Collision collision,
                                    PacmanMovementBehaviour pacmanMovementBehaviour,
                                    MapComponent map,
                                    MapTransformComponent pacmanMapTransform,
                                    MapTransformComponent blinkyMapTransform,
                                    GameObject inky,
                                    MapTransformComponent mapTransform,
                                    int translateModifier = 1) :
                                    base(collision,
                                        inky,
                                        map,
                                        pacmanMapTransform,
                                        mapTransform,
                                        translateModifier)
        {
            this.pacmanMovementBehaviour = pacmanMovementBehaviour;
            this.blinkyMapTransform = blinkyMapTransform;
        }

        protected override void GetTargetPosition()
        {
            Vector2Int pacmanPosition = targetTransform.Position;
            Vector2Int blinkyPosition = blinkyMapTransform.Position;

            switch (pacmanMovementBehaviour.pacmanDirection)
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

            Vector2Int tempPacmanPosition = new Vector2Int(0, 0);
            Vector2Int tempBlinkyPosition = blinkyPosition - pacmanPosition;
            tempBlinkyPosition = new Vector2Int(tempBlinkyPosition.X * -1,
                                                tempBlinkyPosition.Y * -1);
            // tempBlinkyPosition += pacmanPosition;
            TargetPosition = tempBlinkyPosition + pacmanPosition;


        }
    }
}