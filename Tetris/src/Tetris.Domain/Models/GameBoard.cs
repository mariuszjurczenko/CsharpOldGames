namespace Tetris.Domain.Models;

public class GameBoard
{
    public int Width { get; }

    public int Height { get; }

    public Block[,] Grid { get; }


    public GameBoard(int width, int height)
    {
        Width = width;
        Height = height;
        Grid = new Block[width, height];
    }

    public bool IsCellEmpty(int x, int y) => Grid[x, y] == null;

    public void PlaceBlock(Block block)
    {
        if (block.X >= 0 && block.X < Width && block.Y >= 0 && block.Y < Height)
            Grid[block.X, block.Y] = block;
    }
}
