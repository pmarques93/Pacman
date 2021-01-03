using System;
using System.Collections.Generic;
using System.Threading;

namespace Pacman.Components
{
    /// <summary>
    /// Component responsible for reading keys from the terminal.
    /// </summary>
    public class KeyReaderComponent : Component
    {
        /// <summary>
        /// Gets direction to which the pressed key corresponds.
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

        /// <summary>
        /// Gets quitKeys collection.
        /// </summary>
        public ICollection<ConsoleKey> QuitKeys { get; }

        private readonly object threadLock;

        /// <summary>
        /// Constructor for KeyReaderComponent.
        /// </summary>
        /// <param name="quitKey">Key needed to quit the game.</param>
        public KeyReaderComponent(ConsoleKey quitKey = ConsoleKey.Escape)
        {
            QuitKeys = new List<ConsoleKey>
            {
                quitKey,
            };

            threadLock = new object();
        }

        /// <summary>
        /// Constructor for KeyReaderComponent.
        /// </summary>
        /// <param name="quitKeys">Collection with console key.</param>
        public KeyReaderComponent(ICollection<ConsoleKey> quitKeys)
        {
            QuitKeys = quitKeys;
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
        /// KeyReaderComponent is running.
        /// </summary>
        public override void Update()
        {
            ConsoleKey key;
            lock (threadLock)
            {
                if (input.TryDequeue(out key))
                {
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
            } while (!QuitKeys.Contains(key));
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
            => EnterPressed?.Invoke();

        private void OnSpacePressed()
            => SpaceBarPressed?.Invoke();

        /// <summary>
        /// EnterPressed happens when enter is pressed.
        /// </summary>
        public event Action EnterPressed;

        /// <summary>
        /// SpaceBarPressed happens when space bar is pressed.
        /// </summary>
        public event Action SpaceBarPressed;
    }
}