using System.Text.Json;
using Tetris.Domain.Interfaces;
using Tetris.Domain.Models;

namespace Tetris.Infrastructure.Repositories;

public class JsonHighScoreRepository : IHighScoreRepository
{
    private const string HighScoreFile = "highscores.json";

    public List<HighScore> GetAllHighScores()
    {
        if (!File.Exists(HighScoreFile))
            return new List<HighScore>();

        var json = File.ReadAllText(HighScoreFile);

        return JsonSerializer.Deserialize<List<HighScore>>(json) ?? new List<HighScore>();
    }

    public void SaveHighScore(HighScore highScore)
    {
        var allScores = GetAllHighScores();

        allScores.Add(highScore);

        var json = JsonSerializer.Serialize(allScores, new JsonSerializerOptions
        {
            WriteIndented = true,
        });

        File.WriteAllText(HighScoreFile, json);
    }
}
