using FluentAssertions;
using Moq;
using Tetris.Application.Services;
using Tetris.Domain.Models;

namespace Tetris.Application.GameServiceTests;

public class GameServiceTests
{
    private readonly Mock<IGameBoard> _mockGameBoard;
    private readonly GameService _gameService;

    public GameServiceTests()
    {
        _mockGameBoard = new Mock<IGameBoard>();
        _mockGameBoard.Setup(b => b.Width).Returns(10);
        _mockGameBoard.Setup(b => b.Height).Returns(20);
        _mockGameBoard.Setup(b => b.Grid).Returns(new Block[10, 20]);
        _gameService = new GameService(_mockGameBoard.Object);
    }

    [Fact]
    public void SpawnBlock_ShouldCreateNewBlock_AtMiddleTopPosition()
    {
        // Arrange & Act
        _gameService.SpawnBlock();
        var blocks = _gameService.GetBlocks();

        // Assert
        blocks.Should().ContainSingle();
        var block = blocks.First();
        block.X.Should().Be(_mockGameBoard.Object.Width / 2);
        block.Y.Should().Be(0);
        block.Color.Should().Be(ConsoleColor.Green);
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
    public void MoveBlockDown_ShouldPlaceBlockAndSpawnNew_WhenBottomReached()
    {
        // Arange
        _mockGameBoard.Setup(b => b.IsCellEmpty(It.IsAny<int>(), It.IsAny<int>())).Returns(false);

        _gameService.SpawnBlock();
        var initialBlock = _gameService.GetBlocks().First();

        // Act
        _gameService.MoveBlockDown();

        // Assert
        _mockGameBoard.Verify(b => b.PlaceBlock(It.Is<Block>(block => block.X == initialBlock.X &&
            block.Y == initialBlock.Y && block.Color == initialBlock.Color)), Times.Once);

        var newBlock = _gameService.GetBlocks().First();
        newBlock.Should().NotBeSameAs(initialBlock);
        newBlock.Y.Should().Be(0);
    }

    [Fact]
    public void MoveBlockDown_ShouldNotMove_WhenBlockBelowExists()
    {
        // Arrange
        _mockGameBoard.Setup(b => b.IsCellEmpty(It.IsAny<int>(), It.IsAny<int>()))
            .Returns((int x, int y) => y < 19);

        _gameService.SpawnBlock();

        while(_gameService.GetBlocks().First().Y < 18)
        {
            _gameService.MoveBlockDown();
        }

        // Act
        _gameService.MoveBlockDown();

        // Assert
        _mockGameBoard.Verify(b => b.PlaceBlock(It.Is<Block>(block => block.Y == 18)), Times.Once);
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
        blocks.Should().HaveCount(3);
        blocks.Should().Contain(b => b.X == 5 &&  b.Y == 18 && b.Color == ConsoleColor.Blue);
        blocks.Should().Contain(b => b.X == 5 && b.Y == 19 && b.Color == ConsoleColor.Red);
        blocks.Should().Contain(b => b.X == 5 && b.Y == 0 && b.Color == ConsoleColor.Green);
    }
}
