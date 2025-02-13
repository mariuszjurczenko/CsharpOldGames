using Tetris.Domain.Interfaces;

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

    public int ClearFullLines()
    {
        var fullLines = FindFullLines();

        if (!fullLines.Any())
            return 0;

        RemoveLines(fullLines);
        MoveBlockDown(fullLines);       

        return fullLines.Count;
    }

    private List<int> FindFullLines()
    {
        int height = Height;
        var fullLines = new List<int>(capacity: height);

        for (int row = height - 1 ; row >= 0; row--)
        {
            if (IsLineFull(row))
            {
                fullLines.Add(row);
            }
        }

        return fullLines;
    }

    private bool IsLineFull(int row)
    {
        for (int col = 0 ; col < Width; col++)
        {
            if (Grid[col,row] == null)
            {
                return false;
            }
        }

        return true;
    }

    private void RemoveLines(List<int> fullLines)
    {
        foreach (var row in fullLines)
        {
            for (int col = 0; col < Width; col++)
            {
                Grid[col, row] = null;
            }
        }
    }

    private void MoveBlockDown(List<int> removedLines)
    {
        foreach (var removedLine in removedLines.OrderBy(x => x))
        {
            for (int row = removedLine - 1; row >= 0; row--)
            {
                for (int col = 0; col < Width; col++)
                {
                    if (Grid[col,row] != null)
                    {
                        var block = Grid[col, row];
                        Grid[col, row] = null;
                        block.Y += 1;
                        Grid[col, block.Y] = block;
                    }
                }
            }
        }
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
