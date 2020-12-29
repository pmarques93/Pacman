using System;
using System.Collections.Generic;
using System.Linq;
using Pacman.Components;

namespace Pacman.MovementBehaviours.ChaseBehaviour
{
    public class ClydeChaseBehaviour : GhostTargetMovementBehaviour
    {
        private PacmanMovementBehaviour pacmanMovementBehaviour;
        public ClydeChaseBehaviour(Collision collision,
                                    PacmanMovementBehaviour pacmanMovementBehaviour,
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
        {
            this.pacmanMovementBehaviour = pacmanMovementBehaviour;
        }

        protected override void GetTargetPosition()
        {
            TargetPosition = targetTransform.Position;
            double dist = GetAbsoluteDistance(mapTransform.Position,
                                              TargetPosition);
            if (dist <= 8)
            {
                int x = 0;
                int y = map.Map.GetLength(1);
                TargetPosition = new Vector2Int(x, y);
            }
        }
    }
}