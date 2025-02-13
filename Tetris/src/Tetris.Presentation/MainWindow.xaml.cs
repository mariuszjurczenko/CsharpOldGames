using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using Tetris.Application.Services;
using Tetris.Domain.Factories;
using Tetris.Domain.Models;

namespace Tetris.Presentation;

public partial class MainWindow : Window
{
    private readonly GameService _gameService;
    private readonly DispatcherTimer _gameTimer;

    private const int BlockSize = 20;

    public MainWindow()
    {
        InitializeComponent();

        var gameBoard = new GameBoard(10, 20);
        var gameBlockFactory = new GameBlockFactory();
        _gameService = new GameService(gameBoard, gameBlockFactory);

        _gameService.ScoreUpdate += (points, lines) =>
        {
            Dispatcher.Invoke(() =>
            {
                ScoreText.Text = points.ToString();
                LinesText.Text = lines.ToString();
            });
        };

        _gameTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(500)
        };

        _gameTimer.Tick += GameTick;
        _gameService.SpawnBlock();
        _gameTimer.Start();
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