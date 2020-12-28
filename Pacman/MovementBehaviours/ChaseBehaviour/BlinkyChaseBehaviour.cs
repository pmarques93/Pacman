using System;
using System.Collections.Generic;
using System.Linq;
using Pacman.Components;

namespace Pacman.MovementBehaviours.ChaseBehaviour
{
    public class BlinkyChaseBehaviour : GhostTargetMovementBehaviour
    {
        public BlinkyChaseBehaviour(Collision collision,
                                    GameObject ghost,
                                    GameObject pacMan,
                                    MapTransformComponent pacmanMapTransform,
                                    MapTransformComponent mapTransform,
                                    int translateModifier = 1) :
                                    base(collision,
                                        ghost,
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