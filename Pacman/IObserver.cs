namespace Pacman
{
    public interface IObserver<T>
    {
        void Notify(T notification);
    }
}