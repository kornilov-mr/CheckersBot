using System.Security.Policy;
using System.Windows;
using CheckersBot.engine;
using CheckersBot.engine.threads;
using CheckersBot.logic;
using CheckersBot.logic.pieces;
using CheckersBot.tests;
using CheckersBot.utils;
using DotNetEnv;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace CheckersBot;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly ILogger _consoleLogger = LoggerSingleton.CreateLogger<WorkingTask>();
    private readonly Board _defaultBoard = new Board(new BoardPositionSetting(
        PathResolver.ResolvePathFromSolutionRoot("/tests/startingPositions/defaultPosition.txt")));
    public MainWindow()
    {
        Env.Load(PathResolver.ResolvePathFromSolutionRoot(".env"));
        // MasterThread masterThread = new MasterThread(_defaultBoard,2,5*1000);
        // masterThread.ReportMove += Report;
        // Thread thread = new Thread(masterThread.StartCalculation);
        // thread.Start();
        // _consoleLogger.LogInformation("Starting CheckersBot");
        // InitializeComponent();
        HashSet<Piece> pieces = new HashSet<Piece>();
        Piece piece1 = new ManPiece(1,1,PieceColor.White);
        Piece piece2 = new ManPiece(1,1,PieceColor.White);
        pieces.Add(piece1);
        pieces.Remove(piece2);
    }

    public void Report(Move move, double eval)
    {
        Console.WriteLine("real");
        Console.WriteLine(move);
        Console.WriteLine(eval);
    }
}