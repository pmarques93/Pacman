using System;
using System.Collections.Generic;

namespace Pacman
{
    public class Collision : IGameObject
    {
        // Name of this gameobject
        public string Name => "Collision Game Object";

        // Gameobjects and pacman
        private IList<GameObject> gameObjects;
        private GameObject pacman;
        // Transforms
        private TransformComponent pacmanTransform;

        /// <summary>
        /// Adds GameObjects to a collection
        /// </summary>
        /// <param name="gameObject">Gameobject to add</param>
        public void AddGameObject(GameObject gameObject)
            => gameObjects.Add(gameObject);

        /// <summary>
        /// Removes GameObjects from a collection
        /// </summary>
        /// <param name="gameObject">GameObject to remove</param>
        public void RemoveGameObject(GameObject gameObject)
            => gameObjects.Remove(gameObject);

        /// <summary>
        /// Adds pacman
        /// </summary>
        /// <param name="pacman">GameObject to add</param>
        public void AddPacman(GameObject pacman)
            => this.pacman = pacman;

        /// <summary>
        /// Constructor for Collision
        /// </summary>
        public Collision()
        {
            gameObjects = new List<GameObject>();
        }

        /// <summary>
        /// Start method for Collision
        /// Gets every TransformComponent from a list of gameobjects
        /// </summary>
        public void Start()
        {
            pacmanTransform = pacman.GetComponent<TransformComponent>();
        }

        /// <summary>
        /// Update method for Collision
        /// If pacman collides with anything sets an event
        /// </summary>
        public void Update()
        {

            for (int i = 0; i < gameObjects.Count; i++)
            {
                if (/*gameObjects?[i]?.GetComponent<TransformComponent>().Position 
                        + new Vector2Int(1, 0) == pacmanTransform.Position ||
                    gameObjects?[i]?.GetComponent<TransformComponent>().Position
                        + new Vector2Int(0, 1) == pacmanTransform.Position ||
                    gameObjects?[i]?.GetComponent<TransformComponent>().Position
                        + new Vector2Int(-1, 0) == pacmanTransform.Position ||
                    gameObjects?[i]?.GetComponent<TransformComponent>().Position
                        + new Vector2Int(0, -1) == pacmanTransform.Position ||*/
                    gameObjects?[i]?.GetComponent<TransformComponent>().Position 
                        == pacmanTransform?.Position)
                {         
                    // Type of collider and gameobject that collided
                    CollisionAction(gameObjects[i].
                        GetComponent<ColliderComponent>().Type,
                        gameObjects?[i]);
                }
            } 
        }

        /// <summary>
        /// Calls a certain event depending on the type of collision
        /// </summary>
        /// <param name="collisionType">Type of collider pacman 
        /// collided with</param>
        /// <param name="collision">Gameobject of that collider</param>
        private void CollisionAction(Cell collisionType, GameObject collision)
        {
            switch (collisionType)
            {
                case Cell.Ghost:
                    OnGhostCollision();
                    break;
                case Cell.Fruit:
                    OnFoodCollision(collision);
                    RemoveGameObject(collision);
                    OnScoreCollision(250);
                    break;
                case Cell.Food:
                    OnFoodCollision(collision);
                    RemoveGameObject(collision);
                    OnScoreCollision(10);
                    break;
                case Cell.PowerPill:
                    OnFoodCollision(collision);
                    RemoveGameObject(collision);
                    OnScoreCollision(50);
                    OnPowerPillCollision();
                    break;
            }
        }

        /// <summary>
        /// On Ghost collision event method
        /// </summary>
        protected virtual void OnGhostCollision() =>
            GhostCollision?.Invoke();

        /// <summary>
        /// On Fruit or Food collision event method
        /// </summary>
        /// <param name="position"></param>
        protected virtual void OnScoreCollision(ushort score) =>
            ScoreCollision?.Invoke(score);

        /// <summary>
        /// On PowerPill collision event method
        /// </summary>
        protected virtual void OnPowerPillCollision() =>
            PowerPillCollision?.Invoke();

        /// <summary>
        /// On Food collision event method
        /// </summary>
        protected virtual void OnFoodCollision(GameObject gameObject) =>
            FoodCollision?.Invoke(gameObject);

        public event Action GhostCollision;
        public event Action<ushort> ScoreCollision;
        public event Action PowerPillCollision;
        public event Action<GameObject> FoodCollision;



        public void Finish() { }
    }
}
