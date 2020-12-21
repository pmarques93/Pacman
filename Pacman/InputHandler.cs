using System;
using System.Collections.Generic;
using System.Threading;

namespace Pacman
{
    /// <summary>
    /// Responsible for handling the user input.
    /// </summary>
    public class InputHandler : IObservable<ConsoleKey>
    {
        // Observers for specific keys
        private Dictionary<ConsoleKey, ICollection<IObserver<ConsoleKey>>> observers;
        // The input thread
        private Thread inputThread;

        /// <summary>
        /// Constructor, that creates a new instance of InputHandler and
        /// initializes its fields.
        /// </summary>
        public InputHandler()
        {
            observers = new Dictionary<ConsoleKey, ICollection<IObserver<ConsoleKey>>>();
        }

        /// <summary>
        /// Subscribes an observer to the InputHandler.
        /// </summary>
        /// <param name="whatToObserve">Collection of keys to which the
        /// observer will be listen to.</param>
        /// <param name="observer">Observer to be subscribed.</param>
        public void RegisterObserver(IEnumerable<ConsoleKey> whatToObserve,
                                     IObserver<ConsoleKey> observer)
        {
            foreach (ConsoleKey key in whatToObserve)
            {
                if (!observers.ContainsKey(key))
                {
                    observers[key] = new List<IObserver<ConsoleKey>>();
                }
                observers[key].Add(observer);
            }
        }

        /// <summary>
        /// Unsubscribes an observer to InputHandler.
        /// </summary>
        /// <param name="whatToObserve">Key to which the observer 
        ///  listens to.</param>
        /// <param name="observer">Observer to be unsubscribed.</param>
        public void RemoveObserver(ConsoleKey whatToObserve,
                                   IObserver<ConsoleKey> observer)
        {
            if (observers.ContainsKey(whatToObserve))
            {
                observers[whatToObserve].Remove(observer);
            }
        }

        /// <summary>
        /// Unsubscribes an observer to InputHandler.
        /// </summary>
        /// <param name="observer">Observer to be unsubscribed.</param>
        public void RemoveObserver(IObserver<ConsoleKey> observer)
        {
            foreach (ICollection<IObserver<ConsoleKey>> theseObservers in
                                                            observers.Values)
            {
                theseObservers.Remove(observer);
            }
        }
    }
}