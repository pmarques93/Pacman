using System.Timers;
using System;

namespace Pacman
{
    /// <summary>
    /// Class responsible for spawning fruits. Extends Component.
    /// </summary>
    public class FruitSpawnerComponent : Component
    {
        /// <summary>
        /// Gets fruit Timer.
        /// </summary>
        public Timer FruitTimer { get; private set; }

        /// <summary>
        /// Gets or sets fruitName.
        /// </summary>
        public uint FruitName { get; set; }

        /// <summary>
        /// Gets or sets fruitSlot.
        /// </summary>
        public uint FruitSlot { get; set; }

        private readonly uint fruitSpawnTime;

        /// <summary>
        /// Constructor for FruitSpawnerComponent.
        /// </summary>
        /// <param name="fruitSpawnTime">Time to spawn each fruit.</param>
        public FruitSpawnerComponent(uint fruitSpawnTime)
        {
            this.fruitSpawnTime = fruitSpawnTime;
        }

        /// <summary>
        /// Method that happens once on start.
        /// </summary>
        public override void Start()
        {
            FruitName = 0;
            FruitSlot = 0;

            FruitTimer = new Timer(fruitSpawnTime)
            {
                Enabled = true,
            };

            OnRegisterToTimerEvent();
        }

        /// <summary>
        /// Method that happens once on finish.
        /// </summary>
        public override void Finish()
        {
            FruitTimer.Dispose();
        }

        /// <summary>
        /// Method that invokes RegisterToTimerEvent event.
        /// </summary>
        protected virtual void OnRegisterToTimerEvent() =>
            RegisterToTimerEvent?.Invoke();

        /// <summary>
        /// RegisterToTimerEvent is used on a class that wants to register
        /// to this event
        /// </summary>
        public event Action RegisterToTimerEvent;
    }
}
