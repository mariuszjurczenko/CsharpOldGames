namespace Tetris.Domain.Models;

public class Score
{
    public int Points { get; private set; }
    public int LinesCleared { get; private set; }

    private readonly Dictionary<int, int> _scoreTable = new()
    {
        { 1, 100 },
        { 2, 300 },
        { 3, 600 },
        { 4, 1000 },
        { 5, 1500 },
        { 6, 3000 }
    };

    public void AddScore(int lines)
    {
        if (lines > 0 && lines <= 6)
        {
            Points += _scoreTable[lines];
            LinesCleared += lines;
        }
    }
}
