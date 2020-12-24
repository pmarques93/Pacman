using System;
using System.Collections.Generic;
using System.Linq;

namespace Pacman.MovementBehaviours.ChaseBehaviour
{
    public class PinkyChaseBehaviour : GhostTargetMovementBehaviour
    {
        private PacmanMovementBehaviour pacmanMovementBehaviour;
        public PinkyChaseBehaviour(PacmanMovementBehaviour pacmanMovementBehaviour,
                                    GameObject ghost,
                                    GameObject pacMan,
                                    TransformComponent pacmanMapTransform,
                                    TransformComponent mapTransform,
                                    int translateModifier = 1) :
                                    base(ghost,
                                         pacMan,
                                         pacmanMapTransform,
                                         mapTransform,
                                         translateModifier)
        {
            this.pacmanMovementBehaviour = pacmanMovementBehaviour;
        }

        protected override void GetTargetPosition()
        {
            TargetPosition = targetTransform.Position;

            switch (pacmanMovementBehaviour.pacmanDirection)
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