using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using Tetris.Application.Services;
using Tetris.Domain.Factories;
using Tetris.Domain.Interfaces;
using Tetris.Domain.Models;
using Tetris.Infrastructure.Repositories;

namespace Tetris.Presentation;

public partial class MainWindow : Window, IGameObserver
{
    private GameService _gameService;
    private DispatcherTimer _gameTimer;
    private IGameBlockFactory _gameBlockFactory;
    private IHighScoreRepository _highScoreRepository;
    private HighScoreService _highScoreService;

    private const int BlockSize = 20;

    public MainWindow()
    {
        InitializeComponent();
        InitializeGame();
    }

    public void OnScoreUpdate(int score, int linesCleared)
    {
        Dispatcher.Invoke(() =>
        {
            ScoreText.Text = score.ToString();
            LinesText.Text = linesCleared.ToString();
        });
    }

    public void OnGameOver()
    {
        _gameTimer?.Stop();

        Dispatcher.Invoke(() =>
        {
            var dialog = new InputDialog("Game Over!", "Podaj Swoje Imię:");

            if (dialog.ShowDialog() == true)
            {
                var highScore = new HighScore
                {
                    PlayerName = dialog.PlayerName,
                    Score = _gameService.GetScore(),
                    LinesCleared = _gameService.GetLinesClered(),
                    Date = DateTime.Now,
                };
                _highScoreService.AddHighScore(highScore);
                UpdateHighScoresList();
            }

            var result = MessageBox.Show(
                "Chcesz rozpocząć nowa grę ?",
                "Game Over!",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                InitializeGame();
            }
            else
            {
                Close();
            }
        });
    }

    private void InitializeGame()
    {
        var gameBoard = new GameBoard(10, 20);
        _gameBlockFactory = new GameBlockFactory();
        _gameService = new GameService(gameBoard, _gameBlockFactory);
        _highScoreRepository = new JsonHighScoreRepository();
        _highScoreService = new HighScoreService(_highScoreRepository);
        _gameService.Attach(this);

        _gameTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(500)
        };

        _gameTimer.Tick += GameTick;

        ScoreText.Text = "0";
        LinesText.Text = "0";
        GameCanvas.Children.Clear();

        _gameService.SpawnBlock();
        _gameTimer.Start();
        UpdateHighScoresList();
    }

    private void UpdateHighScoresList()
    {
        var highScores = _highScoreService.GetTopHighScores();
        HighScoresList.Children.Clear();

        foreach (var score in highScores)
        {
            var scoreText = new TextBlock
            {
                Text = $"{score.PlayerName}: {score.Score} punktów ({score.LinesCleared} lini)",
                Margin = new Thickness(0, 0, 0, 5)
            };
            HighScoresList.Children.Add(scoreText);
        }
    }

    private void GameTick(object? sender, EventArgs e)
    {
        _gameService.MoveBlockDown();
        Render();
    }

    private void Render()
    {
        GameCanvas.Children.Clear();

        foreach (var block in _gameService.GetBlocks())
        {
            var rect = CreateBlockRectangle(block);

            // Ustawienie pozycji bloku na Canvas
            Canvas.SetLeft(rect, block.X * BlockSize);
            Canvas.SetTop(rect, block.Y * BlockSize);

            GameCanvas.Children.Add(rect);
        }
    }

    private Rectangle CreateBlockRectangle(Block block)
    {
        return new Rectangle
        {
            Width = BlockSize,
            Height = BlockSize,
            Fill = new SolidColorBrush(GetColor(block.Color)),
            Stroke = new SolidColorBrush(Colors.Black),
            StrokeThickness = 1
        };
    }

    private Color GetColor(ConsoleColor color)
    {
        return color switch
        {
            ConsoleColor.Cyan => Colors.Cyan,
            ConsoleColor.Yellow => Colors.Yellow,
            ConsoleColor.Blue => Colors.Blue,
            ConsoleColor.DarkBlue => Colors.DarkBlue,
            ConsoleColor.Magenta => Colors.Magenta,
            ConsoleColor.Green => Colors.Green,
            ConsoleColor.Red => Colors.Red,
            _ => Colors.White,
        };
    }

    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.Left:
                _gameService.MoveBlockLeft();
                break;
            case Key.Right:
                _gameService.MoveBlockRight();
                break;
            case Key.Up:
                _gameService.RotateCurrentBlock();
                break;
            case Key.Down:
                _gameService.MoveBlockDown();
                break;
        }
        Render();
    }
}