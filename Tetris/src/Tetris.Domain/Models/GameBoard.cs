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
        Grid = new Block[height, width];
    }

    public bool IsCellEmpty(int x, int y) => Grid[y, x] == null;

    public void PlaceBlock(Block block)
    {
        if (block.Y >= 0 && block.Y < Height && block.X >= 0 && block.X < Width)
            Grid[block.Y, block.X] = block;
    }
}
