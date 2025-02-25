using Tetris.Domain.Models;

namespace Tetris.Domain.Memento;

public class HighScoreMemento
{
    private readonly List<HighScore> _highScores;
    private readonly DateTime _saveDate;

    public HighScoreMemento(List<HighScore> highScores)
    {
        _highScores = new List<HighScore>(highScores);
        _saveDate = DateTime.Now;
    }

    public List<HighScore> GetState() => new List<HighScore>(_highScores);
    public DateTime GetSaveDate() => _saveDate;
}
