using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

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
        public Direction Direction {get; private set;}

        // Collection of the inputs
        private BlockingCollection<ConsoleKey> input;

        // Thread in which the key reading will happen
        private Thread inputThread;

        /// <summary>
        /// Event that is fired when the Esc key is pressed
        /// </summary>
        public event Action EscapePressed;

        /// <summary>
        /// Method that runs once on start.
        /// </summary>
        public override void Start()
        {
            Direction =  Direction.None;
            input = new BlockingCollection<ConsoleKey>();
            inputThread = new Thread(ReadKeys);
            inputThread.Start();
            Console.CursorVisible = false;
        }

        /// <summary>
        /// Method responsible for what happens when
        ///  KeyReaderComponent is running
        /// </summary>
        public override void Update()
        {
            ConsoleKey key;

            if(input.TryTake(out key))
            {
                switch(key)
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
                    case ConsoleKey.Escape:
                        OnEscapePressed();
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
        }

        /// <summary>
        /// Invokes the EscapePressed event.
        /// </summary>
        private void OnEscapePressed()
        {
            EscapePressed?.Invoke();
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
            }while(key != ConsoleKey.Escape);
        }
    }
}