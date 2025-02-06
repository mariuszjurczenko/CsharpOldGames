using Tetris.Domain.Enums;
using Tetris.Domain.Interfaces;
using Tetris.Domain.Models;

namespace Tetris.Domain.Factories;

public class GameBlockFactory : IGameBlockFactory
{
    private static readonly Random _random = new();

    public GameBlock CreateGameBlock(GameBlockType gameBlockType)
    {
        return gameBlockType switch
        {
            GameBlockType.I => new GameBlockI(),
            GameBlockType.J => new GameBlockJ(),
            GameBlockType.O => new GameBlockO(),
            GameBlockType.T => new GameBlockT(),
            GameBlockType.S => new GameBlockS(),
            GameBlockType.Z => new GameBlockZ(),
            GameBlockType.L => new GameBlockL(),
            _ => throw new ArgumentException($"Unkown GameBlock type: {gameBlockType}")
        };
    }

    public GameBlock CreateRandomGameBlock()
    {
        var values = Enum.GetValues<GameBlockType>();
        var randomType = values[_random.Next(values.Length)];
        return CreateGameBlock(randomType);
    }
}
