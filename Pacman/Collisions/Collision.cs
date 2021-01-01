using System;
using System.Collections.Generic;
using System.Linq;
using Pacman.Components;

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
        private MapComponent map;
        private int counter = 0;

        /// <summary>
        /// Constructor for Collision
        /// </summary>
        public Collision(MapComponent map)
        {
            gameObjects = new List<GameObject>();
            this.map = map;
        }

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
            for (int x = 0; x < map.Map.GetLength(0); x++)
            {
                for (int y = 0; y < map.Map.GetLength(1); y++)
                {
                    if (map.Map[x, y].Collider.Type.HasFlag(Cell.Pacman) && map.Map[x, y].Collider.Type.HasFlag(Cell.Ghost))
                    {
                        OnGhostCollision();
                        return;
                    }
                    if (map.Map[x, y].Collider.Type.HasFlag(Cell.Pacman))
                    {
                        GameObject tempGO = gameObjects.
                            Where(o => o.GetComponent<MapTransformComponent>()?.
                                    Position != null).
                            Where(o => o.GetComponent<MapTransformComponent>().
                                    Position == new Vector2Int(x, y)).
                            FirstOrDefault();

                        CollisionAction(map.Map[x, y].Collider.Type, tempGO);
                        map.Map[x, y].Collider.Type &= ~map.Map[x, y].Collider.Type;
                        map.Map[x, y].Collider.Type |= Cell.Pacman;
                        return;
                    }
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
            if (collisionType.HasFlag(Cell.Fruit))
            {
                OnFoodCollision(collision);
                RemoveGameObject(collision);
                OnScoreCollision(100);
            }
            else if (collisionType.HasFlag(Cell.Food))
            {
                OnFoodCount();
                OnFoodCollision(collision);
                RemoveGameObject(collision);
                OnScoreCollision(10);
            }
            else if (collisionType.HasFlag(Cell.PowerPill))
            {
                OnFoodCollision(collision);
                RemoveGameObject(collision);
                OnScoreCollision(50);
                OnPowerPillCollision();
            }
        }

        /// <summary>
        /// On Ghost collision event method
        /// </summary>
        public virtual void OnGhostCollision() =>
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

        protected virtual void OnFoodCount() =>
            FoodCount?.Invoke();

        public event Action GhostCollision;
        public event Action<ushort> ScoreCollision;
        public event Action PowerPillCollision;
        public event Action<GameObject> FoodCollision;
        public event Action FoodCount;


        public void Finish() { }
    }
}
