using System;

namespace Pacman
{
    /// <summary>
    /// Class that confirms if the game is over. Extends Component
    /// </summary>
    class GameOverCheckComponent: Component
    {
        private GameObject[] allFoods;

        private Collision collisions;

        ushort foodsEatenCount;

        /// <summary>
        /// Constructor for GameOverCheckComponent
        /// </summary>
        /// <param name="foods">Array with all foods</param>
        /// <param name="collision">Collisions component</param>
        public GameOverCheckComponent(GameObject[] foods, Collision collision)
        {
            allFoods = foods;
            collisions = collision;
        }

        /// <summary>
        /// Method that runs once on start
        /// </summary>
        public override void Start()
        {
            foodsEatenCount = 0;
            collisions.FoodCollision += CheckAllFoods;
        }

        /// <summary>
        /// Method that runs once on finish
        /// </summary>
        public override void Finish()
            => collisions.FoodCollision -= CheckAllFoods;

        /// <summary>
        /// Checks if foodseaten number is the same as the array length
        /// </summary>
        /// <param name="temp">Temporary gameobject parameter</param>
        private void CheckAllFoods(GameObject temp)
        {
            foodsEatenCount++;

            if (foodsEatenCount == allFoods.Length)
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
