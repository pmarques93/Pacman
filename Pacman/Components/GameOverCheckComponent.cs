using System;

namespace Pacman
{
    /// <summary>
    /// Class that confirms if the game is over. Extends Component
    /// </summary>
    class GameOverCheckComponent: Component
    {
        private ushort numberOfFoodsToCheck;

        private Collision collisions;

        private ushort foodsEatenCount;
        public ushort FoodsEaten => foodsEatenCount;

        /// <summary>
        /// Constructor for GameOverCheckComponent
        /// </summary>
        /// <param name="numberOfFoodsToCheck">Number of foods to check</param>
        /// <param name="collision">Collisions component</param>
        public GameOverCheckComponent(ushort numberOfFoodsToCheck, 
            Collision collision)
        {
            this.numberOfFoodsToCheck = numberOfFoodsToCheck;
            collisions = collision;
        }

        /// <summary>
        /// Method that runs once on start
        /// </summary>
        public override void Start()
        {
            foodsEatenCount = 0;
            collisions.FoodCount += CheckAllFoods;
        }

        /// <summary>
        /// Method that runs once on finish
        /// </summary>
        public override void Finish()
            => collisions.FoodCount -= CheckAllFoods;

        /// <summary>
        /// Checks if foodseaten number is the same as the array length
        /// </summary>
        private void CheckAllFoods()
        {
            foodsEatenCount++;

            // Foods array has 4 more empty spaces because of power pills
            if (foodsEatenCount == numberOfFoodsToCheck)
                OnNoFoodsLeft();
        }

        /// <summary>
        /// Method that invokes NoFoodsLeft event
        /// </summary>
        protected virtual void OnNoFoodsLeft()
            => NoFoodsLeft?.Invoke();

        public event Action NoFoodsLeft;
    }
}
