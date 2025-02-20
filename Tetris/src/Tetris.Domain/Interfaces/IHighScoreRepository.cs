using Tetris.Domain.Models;

namespace Tetris.Domain.Interfaces;

public interface IHighScoreRepository
{
    List<HighScore> GetAllHighScores();
    void SaveHighScore(HighScore highScore);
}
