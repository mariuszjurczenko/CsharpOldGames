namespace Tetris.Domain.Models;

public class HighScore
{
    public string PlayerName { get; set; }
    public int Score { get; set; }
    public int LinesCleared { get; set; }
    public DateTime Date { get; set; }
}
