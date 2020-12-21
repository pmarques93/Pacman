using System.Collections.Generic;

namespace Pacman
{
    public interface IObservable<T>
    {
        void RegisterObserver(
            IEnumerable<T> whatToObserve, IObserver<T> observer);
        void RemoveObserver(T whatToObserve, IObserver<T> observer);
        void RemoveObserver(IObserver<T> observer);
    }
}