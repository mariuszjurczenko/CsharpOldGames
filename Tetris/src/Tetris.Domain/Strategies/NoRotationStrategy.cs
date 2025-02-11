namespace Tetris.Domain.Strategies;

public class NoRotationStrategy : IRotationStrategy
{
    public int[,] Rotate(int[,] shape)
    {
        return (int[,])shape.Clone();
    }
}
