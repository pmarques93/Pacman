﻿using System.Timers;

namespace Pacman
{
    /// <summary>
    /// Class responsible for spawning fruits. Extends Component
    /// </summary>
    class FruitSpawnerComponent: Component
    {
        // Timer
        private Timer fruitTimer;

        // Fruit spawn
        public uint FruitName { get; set; }
        public uint FruitSlot { get; set; }
        private uint fruitSpawnTime;

        // Component
        private LevelCreation lvl;

        /// <summary>
        /// Constructor for FruitSpawnerComponent
        /// </summary>
        /// <param name="lvl"></param>
        public FruitSpawnerComponent(LevelCreation lvl)
        {
            this.lvl = lvl;

            fruitSpawnTime = 15000;
        }

        /// <summary>
        /// Method that happens once on start
        /// Creates a timer and registers an event to spawn fruits
        /// </summary>
        public override void Start()
        {
            FruitName = 0;
            FruitSlot = 0;

            fruitTimer = new Timer(fruitSpawnTime);
            fruitTimer.Elapsed += lvl.FruitCreation;
            fruitTimer.Enabled = true;
        }

        /// <summary>
        /// Method that happens once on finish
        /// </summary>
        public override void Finish()
        {
            fruitTimer.Elapsed -= lvl.FruitCreation;
            fruitTimer.Dispose();
        }
    }
}