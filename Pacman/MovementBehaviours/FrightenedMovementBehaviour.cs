using System;
using Pacman.Components;

namespace Pacman.MovementBehaviours
{
    public class FrightenedMovementBehaviour : GhostTargetMovementBehaviour
    {
        private Random random;
        public FrightenedMovementBehaviour(Collision collision,
                                    GameObject ghost,
                                    MapComponent map,
                                    MapTransformComponent targetMapTransform,
                                    MapTransformComponent mapTransform,
                                    Random random,
                                    int translateModifier = 1) :
                                    base(collision,
                                        ghost,
                                        map,
                                        targetMapTransform,
                                        mapTransform,
                                        translateModifier)
        {
            this.random = random;
        }

        protected override void GetTargetPosition()
        {
            int xMax = map.Map.GetLength(0);
            int yMax = map.Map.GetLength(1);


            Vector2Int topLeftVector = new Vector2Int(0, 0);

            Vector2Int topRightVector = new Vector2Int(xMax, 0);

            Vector2Int downRightVector = new Vector2Int(xMax, yMax);

            Vector2Int downLeftVector = new Vector2Int(0, yMax);

            Vector2Int[] vectors = new Vector2Int[4]
            {
                topLeftVector, topRightVector,downRightVector,downLeftVector
            };

            int index = random.Next(4);
            TargetPosition = vectors[index];
            // TargetPosition = new Vector2Int(xMax, yMax);

        }
    }
}