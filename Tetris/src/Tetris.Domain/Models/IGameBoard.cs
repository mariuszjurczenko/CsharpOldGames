namespace Tetris.Domain.Models;

public interface IGameBoard
{
    int Width { get; }
    int Height { get; }
    Block[,] Grid { get; }
    bool IsCellEmpty(int x, int y);
    void PlaceBlock(Block block);
}
