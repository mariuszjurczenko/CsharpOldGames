using FluentAssertions;
using Tetris.Domain.Enums;
using Tetris.Domain.Factories;
using Tetris.Domain.Models;

namespace Tetris.Application.GameServiceTests;

public class GameBlockFactoryTests
{
    private readonly GameBlockFactory _factory;

    public GameBlockFactoryTests()
    {
        _factory = new GameBlockFactory();
    }

    [Theory]
    [InlineData(GameBlockType.I, typeof(GameBlockI), ConsoleColor.Cyan)]
    [InlineData(GameBlockType.O, typeof(GameBlockO), ConsoleColor.Yellow)]
    [InlineData(GameBlockType.T, typeof(GameBlockT), ConsoleColor.Magenta)]
    [InlineData(GameBlockType.S, typeof(GameBlockS), ConsoleColor.Green)]
    [InlineData(GameBlockType.Z, typeof(GameBlockZ), ConsoleColor.Red)]
    [InlineData(GameBlockType.J, typeof(GameBlockJ), ConsoleColor.DarkBlue)]
    [InlineData(GameBlockType.L, typeof(GameBlockL), ConsoleColor.Blue)]
    public void CreateGameBlock_ShouldCreateCorrectType(GameBlockType type, Type expectedType, ConsoleColor expectedColor)
    {
        // Act
        var gameBlock = _factory.CreateGameBlock(type);

        // Assert
        gameBlock.Color.Should().Be(expectedColor);
        gameBlock.Should().BeOfType(expectedType);
    }

    [Fact]
    public void CreateGameBlock_ShouldThrowArgumentException_ForInvalidType()
    {
        // Arrange
        var invalidType = (GameBlockType)999;

        // Act
        var action = () => _factory.CreateGameBlock(invalidType);

        // Assert
        action.Should().Throw<ArgumentException>().WithMessage($"Unkown GameBlock type: {invalidType}");
    }

    [Fact]
    public void CreateRandomGameBlock_ShouldReturnValidGameBlock()
    {
        // Act
        var gameBlock = _factory.CreateRandomGameBlock();

        // Assert
        gameBlock.Should().NotBeNull();
        gameBlock.Should().BeAssignableTo<GameBlock>();
    }

    [Fact]
    public void CreateRandomGameBlock_ShouldCreateDifferentTypes()
    {
        // Arrange
        var createdTypes = new HashSet<Type>();
        var attemps = 100;
        var miniumExpectedUniqueTypes = 4;

        // Act
        for (var i = 0; i < attemps; i++)
        {
            var gameBlock = _factory.CreateRandomGameBlock();
            createdTypes.Add(gameBlock.GetType());
        }

        // Assert
        createdTypes.Count.Should().BeGreaterThanOrEqualTo(miniumExpectedUniqueTypes, "Random creation should generate various GameBlock types");
    }

    [Theory]
    [InlineData(GameBlockType.I, 4, 1)]
    [InlineData(GameBlockType.O, 2, 2)]
    [InlineData(GameBlockType.T, 3, 2)]
    [InlineData(GameBlockType.S, 3, 2)]
    [InlineData(GameBlockType.Z, 3, 2)]
    [InlineData(GameBlockType.J, 3, 2)]
    [InlineData(GameBlockType.L, 3, 2)]
    public void CreateGameBlock_ShouldHaveCorrectDimensions(GameBlockType type, int expectedWidth, int expectedHeight)
    {
        // Act
        var gameBlock = _factory.CreateGameBlock(type);
        var coordinates = gameBlock.GetCoordinates().ToList();

        // Assert
        var width = coordinates.Max(c => c.X) - coordinates.Min(c => c.X) + 1;
        var height = coordinates.Max(c => c.Y) - coordinates.Min(c => c.Y) + 1;

        width.Should().Be(expectedWidth);
        height.Should().Be(expectedHeight);
    }

    [Fact]
    public void CreateGameBlock_AllTypesShouldHaveFourBlocks()
    {
        // Arrange
        var allTypes = Enum.GetValues<GameBlockType>();

        foreach (var type in allTypes)
        {
            // Act
            var gameBlock = _factory.CreateGameBlock(type);
            var blockCount = gameBlock.GetCoordinates().Count();

            // Assert
            blockCount.Should().Be(4, $"GameBlock of type {type} should have exactly 4 blocks");
        }
    }
}
