using Tetris.Domain.Interfaces;
using Tetris.Domain.Memento;
using Tetris.Domain.Models;

namespace Tetris.Infrastructure.Repositories;

public class MementoHighScoreRepository : IHighScoreRepository
{
    private const string HighScoreFile = "highscore.json";
    private List<HighScore> _highScores;
    private readonly HighScoreCaretaker _caretaker;

    public MementoHighScoreRepository()
    {
        _caretaker = new HighScoreCaretaker(HighScoreFile);
        _highScores = LoadHighScores();

    }

    private List<HighScore>? LoadHighScores()
    {
        var memento = _caretaker.RestoreMemento();
        return memento?.GetState() ?? new List<HighScore>();
    }

    public List<HighScore> GetAllHighScores()
    {
        return new List<HighScore>(_highScores);
    }

    public void SaveHighScore(HighScore score)
    {
        _highScores.Add(score);
        var memento = new HighScoreMemento(_highScores);
        _caretaker.SaveMemento(memento);
    }
}
