using System;
using System.Collections.Generic;
using System.Linq;

namespace Pacman.MovementBehaviours.ChaseBehaviour
{
    public class BlinkyChaseBehaviour : GhostTargetMovementBehaviour
    {
        public BlinkyChaseBehaviour(GameObject ghost,
                                    GameObject pacMan,
                                    TransformComponent pacmanMapTransform,
                                    TransformComponent mapTransform,
                                    int translateModifier = 1) :
                                    base(ghost,
                                         pacMan,
                                         pacmanMapTransform,
                                         mapTransform,
                                         translateModifier)
       { }

        protected override void GetTargetPosition()
        {
            TargetPosition = targetTransform.Position;
        }
    }
}