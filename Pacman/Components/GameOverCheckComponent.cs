using System;

namespace Pacman
{
    /// <summary>
    /// Class that confirms if the game is over. Extends Component.
    /// </summary>
    public class GameOverCheckComponent : Component
    {
        private readonly ushort numberOfFoodsToCheck;

        private readonly Collision collisions;

        private ushort foodsEatenCount;

        /// <summary>
        /// Gets foosEatenCount.
        /// </summary>
        public ushort FoodsEaten => foodsEatenCount;

        /// <summary>
        /// Constructor for GameOverCheckComponent.
        /// </summary>
        /// <param name="numberOfFoodsToCheck">Number of foods to check.</param>
        /// <param name="collision">Collisions component.</param>
        public GameOverCheckComponent(
            ushort numberOfFoodsToCheck, Collision collision)
        {
            this.numberOfFoodsToCheck = numberOfFoodsToCheck;
            collisions = collision;
        }

        /// <summary>
        /// Method that runs once on start.
        /// </summary>
        public override void Start()
        {
            foodsEatenCount = 0;
            collisions.FoodCount += CheckAllFoods;
        }

        /// <summary>
        /// Method that runs once on finish.
        /// </summary>
        public override void Finish()
            => collisions.FoodCount -= CheckAllFoods;

        /// <summary>
        /// Checks if foodseaten number is the same as the array length.
        /// </summary>
        private void CheckAllFoods()
        {
            foodsEatenCount++;

            if (foodsEatenCount == numberOfFoodsToCheck)
                OnNoFoodsLeft();
        }

        /// <summary>
        /// Method that invokes NoFoodsLeft event.
        /// </summary>
        protected virtual void OnNoFoodsLeft()
            => NoFoodsLeft?.Invoke();

        /// <summary>
        /// NoFoodsLeft happens where there aren't any foods left.
        /// </summary>
        public event Action NoFoodsLeft;
    }
}
