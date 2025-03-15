using System.Windows;
using CheckersBot.logic;
using DotNetEnv;

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