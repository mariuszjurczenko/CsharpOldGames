using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using Tetris.Application.Services;
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
        _gameService = new GameService(gameBoard);

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
            var rect = new Rectangle
            {
                Width = BlockSize,
                Height = BlockSize,
                Fill = new SolidColorBrush(GetColor(block.Color)),
                Stroke = new SolidColorBrush(Colors.Black),
                StrokeThickness = 1
            };

            // Ustawienie pozycji bloku na Canvas
            Canvas.SetLeft(rect, block.X * BlockSize);
            Canvas.SetTop(rect, block.Y * BlockSize);

            GameCanvas.Children.Add(rect);
        }
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
}