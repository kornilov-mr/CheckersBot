using System.Windows;
using System.Windows.Controls;
using DotNetEnv;
using PathResolver = CheckersBot.logic.PathResolver;

namespace CheckersBot;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        Env.Load(PathResolver.ResolvePathFromSolutionRoot(".env"));
        InitializeComponent();
    }
}