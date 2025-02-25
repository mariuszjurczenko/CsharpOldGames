using Tetris.Domain.Models;

namespace Tetris.Domain.Memento;

public class MementoData
{
    public DateTime SaveDate { get; set; }
    public List<HighScore> HighScores { get; set; } = new();
}