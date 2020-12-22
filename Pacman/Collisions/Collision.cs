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
        private IList<TransformComponent> gameObjectsTransforms;
        private TransformComponent pacmanTransform;

        // Collision information
        private Cell collision;
        private GameObject collisionObject;

        // Object to delete - information
        private Scene scene;
        private ConsoleRenderer render;

        /// <summary>
        /// Adds GameObjects to a collection
        /// </summary>
        /// <param name="gameObject"></param>
        public void AddGameObject(GameObject gameObject)
            => gameObjects.Add(gameObject);

        /// <summary>
        /// Adds pacman
        /// </summary>
        /// <param name="pacman"></param>
        public void AddPacman(GameObject pacman)
            => this.pacman = pacman;

        /// <summary>
        /// Constructor for Collision
        /// </summary>
        public Collision(Scene scene, ConsoleRenderer render)
        {
            gameObjects = new List<GameObject>();
            gameObjectsTransforms = new List<TransformComponent>();
            this.scene = scene;
            this.render = render;
        }

        /// <summary>
        /// Start method for Collision
        /// Gets every TransformComponent from a list of gameobjects
        /// </summary>
        public void Start()
        {
            pacmanTransform = pacman.GetComponent<TransformComponent>();

            for (int i = 0; i < gameObjects.Count; i++)
            {
                gameObjectsTransforms.Add(new TransformComponent());
                gameObjectsTransforms[i] = 
                    gameObjects[i].GetComponent<TransformComponent>();
            }
        }

        /// <summary>
        /// Update method for Collision
        /// If pacman collides with anything sets an event
        /// </summary>
        public void Update()
        {
            foreach (TransformComponent transform in gameObjectsTransforms)
            {
                collision = CheckCollision(transform).Item1;
                collisionObject = CheckCollision(transform).Item2;
            }

            switch (collision)
            {
                case Cell.Ghost:
                    OnGhostCollision();
                    break;
                case Cell.Fruit:
                    scene.RemoveGameObject(collisionObject);
                    render.RemoveGameObject(collisionObject);
                    break;
            }
        }

        /// <summary>
        /// Checks for collisions
        /// </summary>
        /// <param name="transform">Transform to compare</param>
        /// <returns></returns>
        private (Cell, GameObject) CheckCollision(
            TransformComponent transform)
        {
            if (transform.Position + new Vector2Int(1, 0) ==
                pacmanTransform.Position ||
                transform.Position + new Vector2Int(0, 1) ==
                pacmanTransform.Position ||
                transform.Position + new Vector2Int(-1, 0) ==
                pacmanTransform.Position ||
                transform.Position + new Vector2Int(0, -1) ==
                pacmanTransform.Position ||
                transform.Position == pacmanTransform.Position)

                return ((Cell)transform?.ParentGameObject?.
                    GetComponent<TransformComponent>()?.Collider?.Type,
                    transform.ParentGameObject);

            return (Cell.Walkable, transform.ParentGameObject);
        }

        /// <summary>
        /// On Ghost collision event method
        /// </summary>
        protected virtual void OnGhostCollision() =>
            GhostCollision?.Invoke();

        /// <summary>
        /// On Fruit collision event method
        /// </summary>
        /// <param name="position"></param>
        protected virtual void OnFruitCollision() =>
            FruitCollision?.Invoke();

        public event Action GhostCollision;
        public event Action FruitCollision;

        public void Finish() { }
    }
}
