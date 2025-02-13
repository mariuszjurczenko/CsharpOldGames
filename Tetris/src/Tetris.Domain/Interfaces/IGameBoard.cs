using Tetris.Domain.Models;

namespace Tetris.Domain.Interfaces;

public interface IGameBoard
{
    int Width { get; }
    int Height { get; }
    Block[,] Grid { get; }
    bool IsCellEmpty(int x, int y);
    void PlaceBlock(Block block);
    int ClearFullLines();
}
