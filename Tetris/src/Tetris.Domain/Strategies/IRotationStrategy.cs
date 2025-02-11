namespace Tetris.Domain.Strategies;

public interface IRotationStrategy
{
    int[,] Rotate(int[,] shape);
}
