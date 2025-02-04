namespace Tetris.Domain.Models;

public class GameBlock
{
    public int X { get; set; }
    public int Y { get; set; }
    public ConsoleColor Color { get; set; }

    protected int[,] Shape;

    public GameBlock(int[,] shape, ConsoleColor color)
    {
        X = 0;
        Y = 0;
        Shape = shape;
        Color = color;
    }

    public IEnumerable<(int X, int Y)> GetCoordinates()
    {
        var coordinates = new List<(int, int)>();
        
        for (int row = 0; row < Shape.GetLength(0); row++)
        {
            for (int col = 0; col < Shape.GetLength(1); col++)
            {
                if (Shape[row,col] == 1)
                {
                    coordinates.Add((X + col, Y + row));
                }
            }
        }
        return coordinates;
    }

    public void Rotate()
    {
        int rows = Shape.GetLength(0);
        int cols = Shape.GetLength(1);

        var newShape = new int[cols, rows];

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                newShape[col, rows - 1 - row] = Shape[row, col];
            }
        }

        Shape = newShape;
    }
}

public class GameBlockI : GameBlock
{
    private static readonly int[,] InitialShape =
    {
        { 1, 1, 1, 1 }
    };

    public GameBlockI() : base(InitialShape, ConsoleColor.Cyan) {}
}

public class GameBlockO : GameBlock
{
    private static readonly int[,] InitialShape =
    {
        { 1, 1 },
        { 1, 1 },
    };

    public GameBlockO() : base(InitialShape, ConsoleColor.Yellow) { }
}

public class GameBlockL : GameBlock
{
    private static readonly int[,] InitialShape =
    {
        { 1, 0, 0 },
        { 1, 1, 1 },
    };

    public GameBlockL() : base(InitialShape, ConsoleColor.Blue) { }
}

public class GameBlockJ : GameBlock
{
    private static readonly int[,] InitialShape =
    {
        { 0, 0, 1 },
        { 1, 1, 1 }
    };

    public GameBlockJ() : base(InitialShape, ConsoleColor.DarkBlue) { }
}

public class GameBlockT : GameBlock
{
    private static readonly int[,] InitialShape =
    {
        { 0, 1, 0 },
        { 1, 1, 1 }
    };

    public GameBlockT() : base(InitialShape, ConsoleColor.Magenta) { }
}

public class GameBlockS : GameBlock
{
    private static readonly int[,] InitialShape =
    {
        { 0, 1, 1 },
        { 1, 1, 0 }
    };

    public GameBlockS() : base(InitialShape, ConsoleColor.Green) { }
}

public class GameBlockZ : GameBlock
{
    private static readonly int[,] InitialShape =
    {
        { 1, 1, 0 },
        { 0, 1, 1 }
    };

    public GameBlockZ() : base(InitialShape, ConsoleColor.Red) { }
}
