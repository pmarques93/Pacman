using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Timers;

namespace Pacman
{
    /// <summary>
    /// Component responsible for reading keys from the terminal.
    /// </summary>
    public class KeyReaderComponent : Component
    {
        /// <summary>
        /// Direction to which the pressed key corresponds.
        /// </summary>
        public Direction Direction { get; private set; }

        // Collection of the inputs
        private Queue<ConsoleKey> input;

        // Thread in which the key reading will happen
        private Thread inputThread;

        /// <summary>
        /// Event that is fired when the Esc key is pressed
        /// </summary>
        public event Action EscapePressed;

        public ICollection<ConsoleKey> quitKeys;

        private Object threadLock;
        public KeyReaderComponent(ConsoleKey quitKey = ConsoleKey.Escape)
        {
            quitKeys = new List<ConsoleKey>();
            quitKeys.Add(quitKey);
            threadLock = new object();
        }

        public KeyReaderComponent(ICollection<ConsoleKey> quitKeys)
        {
            this.quitKeys = quitKeys;
            threadLock = new object();
        }

        /// <summary>
        /// Method that runs once on start.
        /// </summary>
        public override void Start()
        {
            Direction = Direction.None;
            input = new Queue<ConsoleKey>();
            inputThread = new Thread(ReadKeys);
            Console.CursorVisible = false;
            inputThread.Start();
        }


        /// <summary>
        /// Method responsible for what happens when
        ///  KeyReaderComponent is running
        /// </summary>
        public override void Update()
        {
            ConsoleKey key;
            lock (threadLock)
            {
                if (input.TryDequeue(out key))
                {

                    // Console.WriteLine($"key: {key}");
                    switch (key)
                    {
                        case ConsoleKey.W:
                            Direction = Direction.Up;
                            break;
                        case ConsoleKey.A:
                            Direction = Direction.Left;
                            break;
                        case ConsoleKey.S:
                            Direction = Direction.Down;
                            break;
                        case ConsoleKey.D:
                            Direction = Direction.Right;
                            break;
                        case ConsoleKey.Enter:
                            OnEnterPressed();
                            break;
                        case ConsoleKey.Escape:
                            OnEscapePressed();
                            break;
                        case ConsoleKey.Spacebar:
                            OnSpacePressed();
                            break;
                    }
                }
                input.Clear();
            }
        }

        /// <summary>
        /// Method that runs once on finish.
        /// </summary>
        public override void Finish()
        {
            Console.CursorVisible = true;
            inputThread.Join();
        }

        /// <summary>
        /// Reads the keys from the terminal.
        /// </summary>
        private void ReadKeys()
        {
            ConsoleKey key;

            do
            {
                key = Console.ReadKey(true).Key;
                lock (threadLock)
                {
                    input.Enqueue(key);
                }
            } while (!quitKeys.Contains(key));
        }


        /// <summary>
        /// Invokes the EscapePressed event.
        /// </summary>
        protected virtual void OnEscapePressed()
            => EscapePressed?.Invoke();

        /// <summary>
        /// Invokes the EnterPressed event.
        /// </summary>
        protected virtual void OnEnterPressed()
        {
            EnterPressed?.Invoke();
        }

        private void OnSpacePressed()
        {
            SpaceBarPressed?.Invoke();
        }

        public event Action EnterPressed;
        public event Action SpaceBarPressed;
    }
}