using FluentAssertions;
using Moq;
using Tetris.Application.Services;
using Tetris.Domain.Interfaces;
using Tetris.Domain.Models;

namespace Tetris.Application.GameServiceTests;

public class GameServiceTests
{
    private readonly Mock<IGameBoard> _mockGameBoard;
    private readonly Mock<IGameBlockFactory> _mockGameBlockFactory;
    private readonly GameService _gameService;

    public GameServiceTests()
    {
        _mockGameBoard = new Mock<IGameBoard>();
        _mockGameBoard.Setup(b => b.Width).Returns(10);
        _mockGameBoard.Setup(b => b.Height).Returns(20);
        _mockGameBoard.Setup(b => b.Grid).Returns(new Block[10, 20]);

        _mockGameBlockFactory = new Mock<IGameBlockFactory>();
        _mockGameBlockFactory.Setup(f => f.CreateRandomGameBlock()).Returns(() => new GameBlockI());

        _gameService = new GameService(_mockGameBoard.Object, _mockGameBlockFactory.Object);
    }

    [Fact]
    public void SpawnBlock_ShouldCreateNewBlock_AtMiddleTopPosition()
    {
        // Arrange & Act
        _gameService.SpawnBlock();
        var blocks = _gameService.GetBlocks().ToList();

        // Assert
        blocks.Should().NotBeEmpty();
        blocks.All(b => b.X >=4 && b.X <= 7 &&  b.Y == 0).Should().BeTrue();
        blocks.All(b => b.Color == ConsoleColor.Cyan).Should().BeTrue();
    }

    [Fact]
    public void MoveBlockDown_ShouldIncrementY_WhenSpaceBelow()
    {
        // Arange
        _mockGameBoard.Setup(b => b.IsCellEmpty(It.IsAny<int>(), It.IsAny<int>())).Returns(true);

        _gameService.SpawnBlock();
        var initialY = _gameService.GetBlocks().First().Y;

        // Act
        _gameService.MoveBlockDown();

        // Assert
        var newY = _gameService.GetBlocks().First().Y;
        newY.Should().Be(initialY + 1);
    }

    [Fact]
    public void GetBlocks_ShouldReturnAllBlocks_IncludingCurrentAndPlaced()
    {
        // Arrange
        var placedBlocks = new Block[10, 20];
        placedBlocks[5, 18] = new Block { X = 5, Y = 18, Color = ConsoleColor.Blue };
        placedBlocks[5, 19] = new Block { X = 5, Y = 19, Color = ConsoleColor.Red };

        _mockGameBoard.Setup(b => b.Grid).Returns(placedBlocks);
        _gameService.SpawnBlock();

        // Act
        var blocks = _gameService.GetBlocks().ToList();

        // Assert
        blocks.Should().HaveCount(6);
        blocks.Should().Contain(b => b.X == 5 &&  b.Y == 18 && b.Color == ConsoleColor.Blue);
        blocks.Should().Contain(b => b.X == 5 && b.Y == 19 && b.Color == ConsoleColor.Red);
        blocks.Where(b => b.Color == ConsoleColor.Cyan).All(b => b.Y == 0).Should().BeTrue();
    }

    [Fact]
    public void MoveBlockDown_ShouldPlaceBlockAndSpawnNew_WhenBottomReached()
    {
        // Arange
        _mockGameBoard.Setup(b => b.IsCellEmpty(It.IsAny<int>(), It.IsAny<int>())).Returns(false);

        _gameService.SpawnBlock();
        var initialGameBlock = _gameService.GetBlocks().ToList();

        int safetyCounter = 20;
        while (initialGameBlock.Max(b => b.Y) < 18 && safetyCounter-- > 0)
        {
            _gameService.MoveBlockDown();
            initialGameBlock = _gameService.GetBlocks().ToList();
        }

        // Act
        _gameService.MoveBlockDown();
        var newGameBlock = _gameService.GetBlocks().Except(initialGameBlock).ToList();

        // Assert
        newGameBlock.Should().NotBeEmpty();
        newGameBlock.All(b => b.Y == 0).Should().BeTrue();
    }

    [Fact]
    public void MoveBlockDown_ShouldNotMove_WhenBlockBelowExists()
    {
        // Arrange
        _mockGameBoard.Setup(b => b.IsCellEmpty(It.IsAny<int>(), It.IsAny<int>())).Returns((int x, int y) => y < 19);
        _gameService.SpawnBlock();

        var blocks = _gameService.GetBlocks().ToList();

        while (blocks.OrderByDescending(b => b.Y).First().Y < 18)
        {
            _gameService.MoveBlockDown();
            blocks = _gameService.GetBlocks().ToList();
        }

        // Act
        _gameService.MoveBlockDown();

        // Assert
        var newBlocks = _gameService.GetBlocks().ToList();
        newBlocks.OrderByDescending(b => b.Y).First().Y.Should().Be(0);

        foreach (var block in blocks)
        {
            _mockGameBoard.Verify(b => b.PlaceBlock(It.Is<Block>(placedBlock =>
                 placedBlock.X == block.X &&
                 placedBlock.Y == block.Y &&
                 placedBlock.Color == block.Color)), Times.Once);
        }
    }
}
