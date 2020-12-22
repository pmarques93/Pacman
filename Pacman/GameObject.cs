﻿using System.Collections.Generic;
using System.Linq;

namespace Pacman
{
    /// <summary>
    /// Class for every GameObject. Implements IGameObject interface
    /// </summary>
    public class GameObject : IGameObject
    {
        /// <summary>
        /// Property for GameObject's name
        /// </summary>
        public string Name { get; }

        // Collection with every components
        private readonly ICollection<Component> components;

        /// <summary>
        /// Constructor for GameObject
        /// </summary>
        /// <param name="name"></param>
        public GameObject(string name)
        {
            Name = name;
            components = new List<Component>();
        }

        /// <summary>
        /// Adds a component to the components collection in this GameObject
        /// </summary>
        /// <param name="component">Component to add</param>
        public void AddComponent(Component component)
        {
            components.Add(component);
            component.ParentGameObject = this;
        }

        /// <summary>
        /// Gets a certain component from every component attached to 
        /// this GameObject
        /// </summary>
        /// <typeparam name="T">Component to get</typeparam>
        /// <returns></returns>
        public T GetComponent<T>() where T : Component
            => components.FirstOrDefault(component => component is T) as T;

        /// <summary>
        /// Method that runs once on start
        /// </summary>
        public void Start()
        {
            foreach (Component component in components)
                component.Start();
        }

        /// <summary>
        /// Method responsible for what happens when the GameObject is running
        /// </summary>
        public void Update()
        {
            foreach (Component component in components)
                component.Update();
        }

        /// <summary>
        /// Method that runs once on finish
        /// </summary>
        public void Finish()
        {
            foreach (Component component in components)
                component.Finish();
        }

    }
}