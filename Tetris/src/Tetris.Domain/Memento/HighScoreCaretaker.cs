using System.Text.Json;

namespace Tetris.Domain.Memento;

public class HighScoreCaretaker
{
    private readonly string _storageFile;
    private HighScoreMemento _memento;

    public HighScoreCaretaker(string storageFile)
    {
        _storageFile = storageFile;
    }

    public void SaveMemento(HighScoreMemento memento)
    {
        _memento = memento;
        var mementoData = new MementoData
        {
            SaveDate = memento.GetSaveDate(),
            HighScores = memento.GetState()
        };

        var json = JsonSerializer.Serialize(mementoData, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        File.WriteAllText(_storageFile, json);
    }

    public HighScoreMemento RestoreMemento()
    {
        if(!File.Exists(_storageFile)) return null;

        var json = File.ReadAllText(_storageFile);

        var mementoData = JsonSerializer.Deserialize<MementoData>(json);

        if (mementoData == null) return null;

        return new HighScoreMemento(mementoData.HighScores);
    }
}
