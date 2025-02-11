using Tetris.Domain.Strategies;

namespace Tetris.Domain.Models;

public class GameBlock
{
    public int X { get; set; }
    public int Y { get; set; }
    public ConsoleColor Color { get; set; }

    protected int[,] Shape;
    private readonly IRotationStrategy _rotationStrategy;

    public GameBlock(int[,] shape, ConsoleColor color, IRotationStrategy rotationStrategy)
    {
        X = 0;
        Y = 0;
        Shape = shape;
        Color = color;
        _rotationStrategy = rotationStrategy;
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
        Shape = GetRotatedShape();
    }

    public int[,] GetRotatedShape()
    {
        return _rotationStrategy.Rotate(Shape);
    }
}

public class GameBlockI : GameBlock
{
    private static readonly int[,] InitialShape =
    {
        { 1, 1, 1, 1 }
    };

    public GameBlockI() : base(InitialShape, ConsoleColor.Cyan, new MirrorRotationStrategy()) {}
}

public class GameBlockO : GameBlock
{
    private static readonly int[,] InitialShape =
    {
        { 1, 1 },
        { 1, 1 },
    };

    public GameBlockO() : base(InitialShape, ConsoleColor.Yellow, new NoRotationStrategy()) { }
}

public class GameBlockL : GameBlock
{
    private static readonly int[,] InitialShape =
    {
        { 1, 0, 0 },
        { 1, 1, 1 },
    };

    public GameBlockL() : base(InitialShape, ConsoleColor.Blue, new MirrorRotationStrategy()) { }
}

public class GameBlockJ : GameBlock
{
    private static readonly int[,] InitialShape =
    {
        { 0, 0, 1 },
        { 1, 1, 1 }
    };

    public GameBlockJ() : base(InitialShape, ConsoleColor.DarkBlue, new MirrorRotationStrategy()) { }
}

public class GameBlockT : GameBlock
{
    private static readonly int[,] InitialShape =
    {
        { 0, 1, 0 },
        { 1, 1, 1 }
    };

    public GameBlockT() : base(InitialShape, ConsoleColor.Magenta, new MirrorRotationStrategy()) { }
}

public class GameBlockS : GameBlock
{
    private static readonly int[,] InitialShape =
    {
        { 0, 1, 1 },
        { 1, 1, 0 }
    };

    public GameBlockS() : base(InitialShape, ConsoleColor.Green, new MirrorRotationStrategy()) { }
}

public class GameBlockZ : GameBlock
{
    private static readonly int[,] InitialShape =
    {
        { 1, 1, 0 },
        { 0, 1, 1 }
    };

    public GameBlockZ() : base(InitialShape, ConsoleColor.Red, new MirrorRotationStrategy()) { }
}
