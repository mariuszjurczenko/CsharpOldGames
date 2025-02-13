namespace Tetris.Domain.Interfaces;

public interface IGameObserver
{
    void OnScoreUpdate(int score, int linesCleared);
    void OnGameOver();
}
