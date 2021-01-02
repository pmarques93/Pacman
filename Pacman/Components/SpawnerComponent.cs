using System.Collections.Generic;

namespace Pacman.Components
{
    /// <summary>
    /// Class responsible for spawning Spawn Structs. Extends Component.
    /// </summary>
    public class SpawnerComponent : Component
    {
        private readonly ICollection<SpawnStruct> gameObjects;

        /// <summary>
        /// Constructor for SpawnerComponent.
        /// </summary>
        public SpawnerComponent()
        {
            gameObjects = new List<SpawnStruct>();
        }

        /// <summary>
        /// Method that runs once on start.
        /// </summary>
        public override void Start()
        {
            foreach (SpawnStruct s in gameObjects)
            {
                s.GameObject.GetComponent<TransformComponent>().Position =
                    s.TransformPosition;

                s.GameObject.GetComponent<MapTransformComponent>().Position =
                    s.MapTransformPosition;
            }
        }

        /// <summary>
        /// Method that adds a SpawnStruct to a collection.
        /// </summary>
        /// <param name="gameObject">SpawnStruct to add.</param>
        public void AddGameObject(SpawnStruct gameObject)
        {
            gameObjects.Add(gameObject);
        }
    }
}