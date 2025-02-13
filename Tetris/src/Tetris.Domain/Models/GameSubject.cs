using Tetris.Domain.Interfaces;

namespace Tetris.Domain.Models;

public class GameSubject
{
    private readonly List<IGameObserver> _observers = new();

    public void Attach(IGameObserver observer)
    {
        if (!_observers.Contains(observer))
            _observers.Add(observer);
    }

    public void Detach(IGameObserver observer)
    {
        _observers.Remove(observer);
    }

    protected void NotifyScoreUpdated(int score, int linesCleared)
    {
        foreach (var observer in _observers)
        {
            observer?.OnScoreUpdate(score, linesCleared);
        }
    }

    protected void NotifyGameOver()
    {
        foreach(var observer in _observers)
        {
            observer?.OnGameOver();
        }
    }
}
