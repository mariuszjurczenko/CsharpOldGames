using Tetris.Domain.Interfaces;
using Tetris.Domain.Models;

namespace Tetris.Application.Services;

public class HighScoreService
{
    private readonly IHighScoreRepository _highScoreRepository;
    private const int TopScoresCount = 5;

    public HighScoreService(IHighScoreRepository highScoreRepository)
    {
        _highScoreRepository = highScoreRepository;
    }

    public List<HighScore> GetTopHighScores()
    {
        return _highScoreRepository.GetAllHighScores()
            .OrderByDescending(x => x.Score)
            .ThenByDescending(x => x.Date)
            .Take(TopScoresCount)
            .ToList();
    }

    public void AddHighScore(HighScore newScore)
    {
        if (newScore.Score > 0)
            _highScoreRepository.SaveHighScore(newScore);
    }
}
