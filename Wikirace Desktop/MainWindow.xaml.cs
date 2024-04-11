using System.Windows;
using System.Windows.Controls;

namespace Wikirace_Desktop;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

    }

    private void Start_OnClick(object sender, RoutedEventArgs e)
    {
        //Remove all elements from the grid
        MainMenu.Children.Clear();
        //Add the game window to the grid
        Game gameWindow = new Game();
        
        this.Hide();
        
        gameWindow.Show();
        
    }

    private void Exit_OnClick(object sender, RoutedEventArgs e)
    {
        //Close the application
        Application.Current.Shutdown();
    }
}