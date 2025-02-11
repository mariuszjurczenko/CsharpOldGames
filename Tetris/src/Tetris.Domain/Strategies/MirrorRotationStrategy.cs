namespace Tetris.Domain.Strategies;

public class MirrorRotationStrategy : IRotationStrategy
{
    public int[,] Rotate(int[,] shape)
    {
        int rows = shape.GetLength(0);
        int cols = shape.GetLength(1);

        var rotatedShape = new int[rows, cols];

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                rotatedShape[row, cols - 1 -col] = shape[row,col];
            }
        }
        return rotatedShape;
    }
}
