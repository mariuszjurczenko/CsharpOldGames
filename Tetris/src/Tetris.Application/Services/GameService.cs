using Tetris.Domain.Interfaces;
using Tetris.Domain.Models;

namespace Tetris.Application.Services;

public class GameService
{
    private readonly IGameBoard _gameBoard;
    private readonly IGameBlockFactory _gameBlockFactory;
    private GameBlock _currentGameBlock;
    private bool _isGameOver;

    public GameService(IGameBoard gameBoard, IGameBlockFactory gameBlockFactory)
    {
        _gameBoard = gameBoard;
        _gameBlockFactory = gameBlockFactory;
    }

    public void SpawnBlock()
    {
        _currentGameBlock = _gameBlockFactory.CreateRandomGameBlock();
        _currentGameBlock.X = _gameBoard.Width / 2 - 1;
        _currentGameBlock.Y = 0;

        if(!CanMove(0, 0))
        {
            _isGameOver = true;
        }
    }

    public void MoveBlockDown()
    {
        if (_isGameOver || _currentGameBlock == null) return;

        if (CanMove(0, 1))
        {
            _currentGameBlock.Y++;
        }
        else
        {
            PlaceGameBlock();
            SpawnBlock();
        }
    }

    public IEnumerable<Block> GetBlocks()
    {
        var blocks = new List<Block>();

        if (_currentGameBlock != null)
        {
            blocks.AddRange(_currentGameBlock.GetCoordinates().Select(coord => new Block
            {
                X = coord.X,
                Y = coord.Y,
                Color = _currentGameBlock.Color
            }));
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

    public void RotateCurrentBlock()
    {
        if (_currentGameBlock == null)
            return; 

        if (CanRotate())
        {
            _currentGameBlock.Rotate();
        }
    }
    
    public void MoveBlockLeft()
    {
        if (_currentGameBlock == null)
            return;

        if (CanMove(-1,0))
        {
            _currentGameBlock.X--;
        }
    }

    public void MoveBlockRight()
    {
        if (_currentGameBlock == null)
            return;

        if (CanMove(1, 0))
        {
            _currentGameBlock.X++;
        }
    }

    private bool CanMove(int x, int y)
    {
        return _currentGameBlock.GetCoordinates()
            .All(cord =>
            {
                int newX = cord.X + x;
                int newY = cord.Y + y;

                if (newX < 0 || newY < 0 || newX >= _gameBoard.Width || newY >= _gameBoard.Height)
                    return false;

                return _gameBoard.IsCellEmpty(newX,newY);
            });
    }

    private void PlaceGameBlock()
    {
        foreach (var (x,y) in _currentGameBlock.GetCoordinates())
        {
            _gameBoard.PlaceBlock(new Block
            {
                X = x,
                Y = y,
                Color = _currentGameBlock.Color
            });
        }
    }

    private bool CanRotate()
    {
        if (_currentGameBlock == null) return false;

        var rotatedShape = _currentGameBlock.GetRotatedShape();

        foreach (var (x,y) in GetCoordinatesForShape(rotatedShape, _currentGameBlock.X, _currentGameBlock.Y))
        {
            if (x < 0 || x >= _gameBoard.Width || y < 0 || y > _gameBoard.Height)
                return false;

            if (!_gameBoard.IsCellEmpty(x,y))
                return false;
        }

        return true;
    }

    private IEnumerable<(int x, int y)> GetCoordinatesForShape(int[,] rotatedShape, int x, int y)
    {
        for (var row = 0; row < rotatedShape.GetLength(0); row++)
        {
            for (var col = 0; col < rotatedShape.GetLength(1); col++)
            {
                if (rotatedShape[row,col] == 1)
                {
                    yield return (x + col, y + row);
                }
            }
        }
    }
}
