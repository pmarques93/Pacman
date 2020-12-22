using System.Collections.Generic;

namespace Pacman
{
    public class Collision : IGameObject
    {
        public string Name => "Collision Game Object";

        private ICollection<GameObject> gameObjects;

        public void AddGameObject(GameObject gameObject)
            => gameObjects.Add(gameObject);


        public Collision()
            => gameObjects = new List<GameObject>();

        public void Start() { }


        public void Update()
        {
            foreach (GameObject gameObject in gameObjects)
            {
                foreach (GameObject gameObject2 in gameObjects)
                {
                    if (gameObject.Name != gameObject2.Name)
                    {
                        if (gameObject.GetComponent<TransformComponent>().Position.X ==
                            gameObject2.GetComponent<TransformComponent>().Position.X &&
                            gameObject.GetComponent<TransformComponent>().Position.Y ==
                            gameObject2.GetComponent<TransformComponent>().Position.Y)
                            System.Console.WriteLine("testeeeeeeeeeeeee");
                    }
                }
            }
        }

        public void Finish() { }
    }
}
