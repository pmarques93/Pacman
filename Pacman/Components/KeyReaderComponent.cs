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
        private BlockingCollection<ConsoleKey> input;

        // Thread in which the key reading will happen
        private Thread inputThread;

        /// <summary>
        /// Event that is fired when the Esc key is pressed
        /// </summary>
        public event Action EscapePressed;

        public ICollection<ConsoleKey> quitKeys;
        public KeyReaderComponent(ConsoleKey quitKey = ConsoleKey.Escape)
        {
            quitKeys = new List<ConsoleKey>();
            quitKeys.Add(quitKey);
        }

        public KeyReaderComponent(ICollection<ConsoleKey> quitKeys)
        {
            this.quitKeys = quitKeys;
        }

        private System.Timers.Timer myTimer;

        /// <summary>
        /// Method that runs once on start.
        /// </summary>
        public override void Start()
        {
            Direction = Direction.None;
            input = new BlockingCollection<ConsoleKey>();
            inputThread = new Thread(ReadKeys);
            Console.CursorVisible = false;
            myTimer = new System.Timers.Timer(100);
            myTimer.AutoReset = true;
            myTimer.Elapsed += ClearTest;
            inputThread.Start();
            myTimer.Start();
        }

        private void ClearTest(object sender, ElapsedEventArgs e)
        {
            // input = new BlockingCollection<ConsoleKey>();
        }

        /// <summary>
        /// Method responsible for what happens when
        ///  KeyReaderComponent is running
        /// </summary>
        public override void Update()
        {
            ConsoleKey key;

            if (input.TryTake(out key))
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
                    default:
                        Direction = Direction.None;
                        break;
                }
            }
            else
            {
                Direction = Direction.None;
            }
            input = new BlockingCollection<ConsoleKey>();
        }

        /// <summary>
        /// Method that runs once on finish.
        /// </summary>
        public override void Finish()
        {
            Console.CursorVisible = true;
            inputThread.Join();
            myTimer.Stop();
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
                input.Add(key);
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