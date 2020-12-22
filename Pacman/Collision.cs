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

        int test = 0;
        public void Update()
        {
            
            foreach (GameObject gameObject in gameObjects)
            {
                foreach (GameObject gameObject2 in gameObjects)
                {
                    if (gameObject.Name != gameObject2.Name)
                    {
                        if (gameObject.GetComponent<TransformComponent>().Position ==
                            gameObject2.GetComponent<TransformComponent>().Position)
                            System.Console.WriteLine(test++);
                    }
                }
            }
        }

        public void Finish() { }
    }
}
