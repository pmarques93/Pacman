using System;
using System.Collections.Generic;
using System.Linq;
using Pacman.Components;

namespace Pacman.Collisions
{
    /// <summary>
    /// Class that handles collisions. Implements IGameObject.
    /// </summary>
    public class Collision : IGameObject
    {
        /// <summary>
        /// Gets property for Name.
        /// </summary>
        public string Name => "Collision Game Object";

        private readonly MapComponent map;

        private readonly IList<GameObject> gameObjects;

        /// <summary>
        /// Constructor for Collision.
        /// </summary>
        /// <param name="map">Reference to map.</param>
        public Collision(MapComponent map)
        {
            gameObjects = new List<GameObject>();
            this.map = map;
        }

        /// <summary>
        /// Adds GameObjects to a collection.
        /// </summary>
        /// <param name="gameObject">Gameobject to add.</param>
        public void AddGameObject(GameObject gameObject)
            => gameObjects.Add(gameObject);

        /// <summary>
        /// Removes GameObjects from a collection.
        /// </summary>
        /// <param name="gameObject">GameObject to remove.</param>
        public void RemoveGameObject(GameObject gameObject)
            => gameObjects.Remove(gameObject);

        /// <summary>
        /// Start method for Collision. Happens once on Start.
        /// </summary>
        public void Start()
        {
            // Method intentionally left empty.
        }

        /// <summary>
        /// Update method for Collision
        /// Checks if pacman is colliding with anything.
        /// </summary>
        public void Update()
        {
            for (int x = 0; x < map.Map.GetLength(0); x++)
            {
                for (int y = 0; y < map.Map.GetLength(1); y++)
                {
                    if (map.Map[x, y].Collider.Type.HasFlag(Cell.Ghost))
                    {
                        int xMax = map.Map.GetLength(0);
                        int yMax = map.Map.GetLength(1);
                        GameObject tempGO = gameObjects.
                            Where(o => o.GetComponent<ColliderComponent>().
                            Type.HasFlag(Cell.Ghost)).
                            FirstOrDefault(
                            o => o.GetComponent<MapTransformComponent>().
                            Position == new Vector2Int(x, y));

                        if (map.Map[x, y].Collider.Type.HasFlag(Cell.Pacman) ||
                            map.Map[Math.Max(0, x - 1), y].Collider.Type.HasFlag(Cell.Pacman) ||
                            map.Map[Math.Min(xMax - 1, x + 1), y].Collider.Type.HasFlag(Cell.Pacman) ||
                            map.Map[x, Math.Max(0, y - 1)].Collider.Type.HasFlag(Cell.Pacman) ||
                            map.Map[x, Math.Min(yMax - 1, y + 1)].Collider.Type.HasFlag(Cell.Pacman))
                        {
                            OnGhostCollision(tempGO);
                        }
                        if (map.Map[x, y].Collider.Type.
                            HasFlag(Cell.GhostHouseExit))
                        {
                            OnGhostHouseExitCollision(
                                tempGO, map.Map[x, y].Collider.Type);
                        }
                        if (map.Map[x, y].Collider.Type.
                            HasFlag(Cell.GhostHouse))
                        {
                            OnGhostHouseCollision(tempGO);
                        }
                    }
                    else if (map.Map[x, y].Collider.Type.HasFlag(Cell.Pacman) &&
                        (map.Map[x, y].Collider.Type.HasFlag(Cell.Food) ||
                        map.Map[x, y].Collider.Type.HasFlag(Cell.Fruit) ||
                        map.Map[x, y].Collider.Type.HasFlag(Cell.PowerPill)))
                    {
                        GameObject tempGO = gameObjects.
                            Where(o => o.GetComponent<MapTransformComponent>()?.
                                    Position != null).
                            FirstOrDefault(
                            o => o.GetComponent<MapTransformComponent>().
                                    Position == new Vector2Int(x, y));

                        CollisionAction(map.Map[x, y].Collider.Type, tempGO);

                        if (map.Map[x, y].Collider.Type.
                            HasFlag(Cell.Food))
                        {
                            map.Map[x, y].Collider.Type &= ~Cell.Food;
                        }
                        else if (map.Map[x, y].Collider.Type.
                            HasFlag(Cell.Fruit))
                        {
                            map.Map[x, y].Collider.Type &= ~Cell.Fruit;
                        }
                        else if (map.Map[x, y].Collider.Type.
                            HasFlag(Cell.PowerPill))
                        {
                            map.Map[x, y].Collider.Type &= ~Cell.PowerPill;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Calls a certain event depending on the type of collision.
        /// </summary>
        /// <param name="collisionType">Type of collider pacman
        /// collided with.</param>
        /// <param name="collision">Gameobject of that collider.</param>
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
        /// Method that calls OnGhostHouseExitCollision event.
        /// </summary>
        /// <param name="gameObject">Ghost that collided.</param>
        /// <param name="cell">Ghost's Cell.</param>
        protected virtual void OnGhostHouseExitCollision(
            GameObject gameObject, Cell cell) =>
            GhostHouseExitCollision?.Invoke(gameObject, cell);

        /// <summary>
        /// On Ghost collision event method.
        /// </summary>
        /// <param name="gameObject">Ghost that collided.</param>
        public virtual void OnGhostCollision(GameObject gameObject) =>
            GhostCollision?.Invoke(gameObject);

        /// <summary>
        /// On Ghost House Collision event method.
        /// </summary>
        /// <param name="gameObject">Ghost that collided.</param>
        public void OnGhostHouseCollision(GameObject gameObject) =>
            GhostHouseCollision?.Invoke(gameObject);

        /// <summary>
        /// On Score collision event method.
        /// </summary>
        /// <param name="score">Score to add.</param>
        protected virtual void OnScoreCollision(ushort score) =>
            ScoreCollision?.Invoke(score);

        /// <summary>
        /// On PowerPill collision event method.
        /// </summary>
        protected virtual void OnPowerPillCollision() =>
            PowerPillCollision?.Invoke();

        /// <summary>
        /// On Food collision event method.
        /// </summary>
        /// <param name="gameObject">Eaten food.</param>
        protected virtual void OnFoodCollision(GameObject gameObject) =>
            FoodCollision?.Invoke(gameObject);

        /// <summary>
        /// On Food Count event method. Happens when pacman eats a food.
        /// </summary>
        protected virtual void OnFoodCount() =>
            FoodCount?.Invoke();

        /// <summary>
        /// GhostCollision event.
        /// </summary>
        public event Action<GameObject> GhostCollision;

        /// <summary>
        /// GhostHouseCollision event happens when the ghost reaches the house.
        /// </summary>
        public event Action<GameObject> GhostHouseCollision;

        /// <summary>
        /// GhostHouseExitCollision happens when the ghost leaves the house.
        /// </summary>
        public event Action<GameObject, Cell> GhostHouseExitCollision;

        /// <summary>
        /// ScoreCollision happens when pacman eats something that increments
        /// score
        /// </summary>
        public event Action<ushort> ScoreCollision;

        /// <summary>
        /// PowerPillCollision happens when pacman eats a power pill
        /// </summary>
        public event Action PowerPillCollision;

        /// <summary>
        /// FoodCollision happens when pacman eats some kind of food
        /// (powerpills, food, or fruits)
        /// </summary>
        public event Action<GameObject> FoodCollision;

        /// <summary>
        /// FoodCount happens when pacman eats a food
        /// </summary>
        public event Action FoodCount;

        /// <summary>
        /// Method that happens once on finish.
        /// </summary>
        public void Finish()
        {
            // Method intentionally left empty.
        }
    }
}
