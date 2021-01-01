using System.Timers;
using System;

namespace Pacman
{
    /// <summary>
    /// Class responsible for spawning fruits. Extends Component
    /// </summary>
    class FruitSpawnerComponent: Component
    {
        // Timer
        public Timer FruitTimer { get; set; }

        // Fruit spawn
        public uint FruitName { get; set; }
        public uint FruitSlot { get; set; }
        private uint fruitSpawnTime;

        /// <summary>
        /// Constructor for FruitSpawnerComponent
        /// </summary>
        /// <param name="fruitSpawnTime">Time to spawn each fruit</param>
        public FruitSpawnerComponent(uint fruitSpawnTime)
        {
            this.fruitSpawnTime = fruitSpawnTime;
        }

        /// <summary>
        /// Method that happens once on start
        /// </summary>
        public override void Start()
        {
            FruitName = 0;
            FruitSlot = 0;

            FruitTimer = new Timer(fruitSpawnTime);
            FruitTimer.Enabled = true;

            OnRegisterToTimerEvent();
        }

        /// <summary>
        /// Method that happens once on finish
        /// </summary>
        public override void Finish()
        {
            FruitTimer.Dispose();
        }

        /// <summary>
        /// Method that calls RegisterToTimerEvent event
        /// </summary>
        protected virtual void OnRegisterToTimerEvent() =>
            RegisterToTimerEvent?.Invoke();

        public event Action RegisterToTimerEvent;
    }
}
