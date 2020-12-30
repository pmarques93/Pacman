using System.Collections.Generic;

namespace Pacman.Components
{
    public class SpawnerComponent : Component
    {
        private ICollection<SpawnStruct> gameObjects;
        public SpawnerComponent()
        {
            gameObjects = new List<SpawnStruct>();
        }
        public override void Start()
        {
            foreach (SpawnStruct s in gameObjects)
            {
                s.GameObject.GetComponent<TransformComponent>().Position = s.TransformPosition;
                s.GameObject.GetComponent<MapTransformComponent>().Position = s.MapTransformPosition;
            }
        }
        public void AddGameObject(SpawnStruct gameObject)
        {
            gameObjects.Add(gameObject);
        }
    }
}