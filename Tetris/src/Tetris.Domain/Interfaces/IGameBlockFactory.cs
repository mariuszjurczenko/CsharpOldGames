using Tetris.Domain.Enums;
using Tetris.Domain.Models;

namespace Tetris.Domain.Interfaces;

public interface IGameBlockFactory
{
    GameBlock CreateGameBlock(GameBlockType gameBlockType);
    GameBlock CreateRandomGameBlock();
}
