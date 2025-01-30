using Tetris.Domain.Models;

namespace Tetris.Application.Services;

public class GameService
{
    private readonly GameBoard _gameBoard;
    private Block _currentBlock;

    public GameService(GameBoard gameBoard)
    {
        _gameBoard = gameBoard;
    }

    public void SpawnBlock()
    {
        _currentBlock = new Block { X = _gameBoard.Width / 2, Y = 0, Color = ConsoleColor.Green };
    }

    public void MoveBlockDown()
    {
        if (_currentBlock == null) return;

        if (CanMove(_currentBlock.X, _currentBlock.Y + 1))
        {
            _currentBlock.Y++;
        }
        else
        {
            _gameBoard.PlaceBlock(_currentBlock);
            SpawnBlock();
        }
    }

    public IEnumerable<Block> GetBlocks()
    {
        var blocks = new List<Block>();

        if (_currentBlock != null)
        {
            blocks.Add(_currentBlock);
        }

        for (int x = 0; x < _gameBoard.Width; x++)
        {
            for (int y = 0; y < _gameBoard.Height; y++)
            {
                var block = _gameBoard.Grid[x, y];
                if (block != null)
                {
                    blocks.Add(block);
                }
            }
        }

        return blocks;
    }

    private bool CanMove(int x, int y)
    {
        return y<_gameBoard.Height && _gameBoard.IsCellEmpty(x, y);
    }
}
