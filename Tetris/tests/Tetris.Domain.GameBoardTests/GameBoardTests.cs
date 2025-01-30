using FluentAssertions;
using Tetris.Domain.Models;

namespace Tetris.Domain.GameBoardTests;

public class GameBoardTests
{
    [Theory]
    [InlineData(10, 10)]
    [InlineData(10, 20)]
    [InlineData(20, 50)]
    public void Constructor_ShouldCreateEmptyGrid_WithSpecifiedDimensions(int width, int height)
    {
        // Arrange & Act
        var board = new GameBoard(width, height);

        // Assert
        board.Width.Should().Be(width);
        board.Height.Should().Be(height);
        board.Grid.Should().NotBeNull();
        board.Grid.GetLength(0).Should().Be(width);
        board.Grid.GetLength(1).Should().Be(height);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(9)]
    public void Constructor_ShouldThrowArgumentException_WhenWidthIsInvalid(int width)
    {
        var act = () => new GameBoard(width, 20);
        act.Should().Throw<ArgumentException>().WithMessage("Width must be greater than 9*");
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(9)]
    public void Constructor_ShouldThrowArgumentException_WhenHeightIsInvalid(int height)
    {
        var act = () => new GameBoard(20, height);
        act.Should().Throw<ArgumentException>().WithMessage("Height must be greater than 9*");
    }

    [Fact]
    public void Constructor_ShouldInitializeEmptyGrid_AllCellsShouldBeNull()
    {
        // Arrange & Act
        var board = new GameBoard(10, 20);

        // Assert
        for (int x = 0; x < board.Width; x++)
        {
            for (int y = 0; y < board.Height; y++)
            {
                board.Grid[x, y].Should().BeNull($"Cell at position ({x}, {y}) should be null");
            }
        }
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(3, 5)]
    [InlineData(6, 10)]
    [InlineData(7, 14)]
    [InlineData(9, 19)]
    public void IsCellEmpty_ShouldReturnTrue_WhenCellIsEmpty(int x, int y)
    {
        // Arrange
        var board = new GameBoard(10, 20);

        // Act
        var isEmpty = board.IsCellEmpty(x, y);

        // Assert
        isEmpty.Should().BeTrue();
    }

    [Fact]
    public void IsCellEmpty_ShouldReturnFalse_WhenCellContainsBlock()
    {
        // Arrange
        var board = new GameBoard(10, 20);
        var block = new Block { X = 5, Y = 10, Color =ConsoleColor.Blue };
        board.PlaceBlock(block);

        // Act
        var isEmpty = board.IsCellEmpty(5, 10);

        // Assert
        isEmpty.Should().BeFalse();
    }

    [Theory]
    [InlineData(-1, 0, "Coordinates (-1, 0) are outside the board dimensions (10, 20)")]
    [InlineData(10, 0, "Coordinates (10, 0) are outside the board dimensions (10, 20)")]
    [InlineData(0, -1, "Coordinates (0, -1) are outside the board dimensions (10, 20)")]
    [InlineData(0, 20, "Coordinates (0, 20) are outside the board dimensions (10, 20)")]
    public void IsCellEmpty_ShouldThrowArgumentOutOfRangeException_WhenCoordinatesAreInvalid(int x, int y, string expectedMessage)
    {
        // Arrange
        var board = new GameBoard(10, 20);

        // Act
        var act = () => board.IsCellEmpty(x,y);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>().WithMessage($"*{expectedMessage}*");
    }

    [Fact]
    public void PlaceBlock_ShouldThrowArgumentNullException_WhenBlockIsNull()
    {
        // Arrange
        var board = new GameBoard(10, 20);

        // Act
        var act = () => board.PlaceBlock(null);

        // Assert
        act.Should().Throw<ArgumentNullException>().WithParameterName("block");
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(5, 10)]
    [InlineData(9, 19)]
    public void PlaceBlock_ShouldPlaceBlock_WhenPositionIsValid(int x, int y)
    {
        // Arrange
        var board = new GameBoard(10, 20);
        var block = new Block { X = x, Y = y, Color = ConsoleColor.Red };

        // Act
        board.PlaceBlock(block);

        // Assert
        board.Grid[x,y].Should().NotBeNull();
        board.Grid[x, y].Should().BeSameAs(block);
        board.IsCellEmpty(x,y).Should().BeFalse();
    }

    [Fact]
    public void PlaceBlock_ShouldOverwriteExistingBlock_WhenPositionIsOccupied()
    {
        // Arrange
        var board = new GameBoard(10, 20);
        var block1 = new Block { X = 5, Y = 10, Color = ConsoleColor.Red };
        var block2 = new Block { X = 5, Y = 10, Color = ConsoleColor.Green };

        // Act
        board.PlaceBlock(block1);
        board.PlaceBlock(block2);

        // Assert
        board.Grid[5,10].Should().NotBeNull();
        board.Grid[5,10].Should().BeSameAs(block2);
        board.Grid[5, 10].Should().NotBeSameAs(block1);
        board.Grid[5, 10].Color.Should().Be(ConsoleColor.Green);
    }

    [Theory]
    [InlineData(-1, 0)]
    [InlineData(10, 0)]
    [InlineData(0, -1)]
    [InlineData(0, 20)]
    public void PlaceBlock_ShouldNotPlaceBlock_WhenPositionIsOutOfBounds(int x, int y)
    {
        // Arrange
        var board = new GameBoard(10, 20);
        var block = new Block { X = x, Y = y, Color = ConsoleColor.Red };

        // Act
        board.PlaceBlock(block);

        // Assert
        for (int i = 0; i < board.Width; i++)
        {
            for (int j = 0; j < board.Height; j++)
            {
                board.Grid[i,j].Should().BeNull();
            }
        }
    }
}
