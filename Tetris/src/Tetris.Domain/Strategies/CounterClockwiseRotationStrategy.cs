namespace Tetris.Domain.Strategies;

public class CounterClockwiseRotationStrategy : IRotationStrategy
{
    public int[,] Rotate(int[,] shape)
    {
        int rows = shape.GetLength(0);
        int cols = shape.GetLength(1);

        var rotatedShape = new int[cols, rows];

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                rotatedShape[cols - 1 -col, row] = shape[row, col];
            }
        }
        return rotatedShape;
    }
}
