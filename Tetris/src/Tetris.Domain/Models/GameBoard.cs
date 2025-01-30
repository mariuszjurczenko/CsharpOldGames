namespace Tetris.Domain.Models;

public class GameBoard : IGameBoard
{
    public int Width { get; }

    public int Height { get; }

    public Block[,] Grid { get; }


    public GameBoard(int width, int height)
    {
        if (width < 10) throw new ArgumentException("Width must be greater than 9*");
        if (height < 10) throw new ArgumentException("Height must be greater than 9*");

        Width = width;
        Height = height;
        Grid = new Block[width, height];
    }

    public bool IsCellEmpty(int x, int y)
    {
        ValidateCoordinates(x, y);
        return Grid[x, y] == null;
    }

    public void PlaceBlock(Block block)
    {
        if (block == null) throw new ArgumentNullException(nameof(block));

        if (IsValidPosition(block.X, block.Y))
            Grid[block.X, block.Y] = block;
    }

    private void ValidateCoordinates(int x, int y)
    {
        if (!IsValidPosition(x, y))
        {
            throw new ArgumentOutOfRangeException($"Coordinates ({x}, {y}) are outside the board dimensions ({Width}, {Height})");
        }
    }

    private bool IsValidPosition(int x, int y)
    {
        return (x >= 0 && x < Width && y >= 0 && y < Height);
    }
}
