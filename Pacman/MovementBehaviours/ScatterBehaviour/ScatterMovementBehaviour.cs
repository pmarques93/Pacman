using Pacman.Components;

namespace Pacman.MovementBehaviours.ScatterBehaviour
{
    public class ScatterMovementBehaviour : GhostTargetMovementBehaviour
    {
        public ScatterMovementBehaviour(Collision collision,
                                    GameObject ghost,
                                    GameObject pacMan,
                                    MapTransformComponent targetMapTransform,
                                    MapTransformComponent mapTransform,
                                    int translateModifier = 1) :
                                    base(collision,
                                        ghost,
                                        pacMan,
                                        targetMapTransform,
                                        mapTransform,
                                        translateModifier)
        { }

        protected override void GetTargetPosition()
        {
            TargetPosition = targetTransform.Position;
        }
    }
}