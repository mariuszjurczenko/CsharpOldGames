using System.Windows;
using System.Windows.Controls;

namespace Tetris.Presentation;

public class InputDialog : Window
{
    private TextBox nameTextBox;

    public string PlayerName => nameTextBox.Text;

    public InputDialog(string title, string prompt)
    {
        Title = title;
        Width = 300;
        Height = 150;
        WindowStartupLocation = WindowStartupLocation.CenterScreen;
        ResizeMode = ResizeMode.NoResize;

        var grid = new Grid
        {
            Margin = new Thickness(10)
        };

        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

        var promptTaxt = new TextBox
        {
            Text = prompt,
            Margin = new Thickness(0, 0, 0, 5)
        };
        Grid.SetRow(promptTaxt, 0);

        nameTextBox = new TextBox
        {
            Margin = new Thickness(0, 0, 0, 10)
        };
        Grid.SetRow(nameTextBox, 1);

        var buttonPanel = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            HorizontalAlignment = HorizontalAlignment.Right,
        };
        Grid.SetRow(buttonPanel, 2);

        var okButton = new Button
        {
            Content = "OK",
            Width = 75,
            Margin = new Thickness(0, 0, 5, 0),
            IsDefault = true,
        };
        okButton.Click += (s, e) =>
        {
            DialogResult = true;
            Close();
        };

        var cancelButton = new Button
        {
            Content = "Cancel",
            Width = 75,
            IsCancel = true,
        };

        buttonPanel.Children.Add(okButton);
        buttonPanel.Children.Add(cancelButton);

        grid.Children.Add(promptTaxt);
        grid.Children.Add(nameTextBox);
        grid.Children.Add(buttonPanel);

        Content = grid;
    }
}
