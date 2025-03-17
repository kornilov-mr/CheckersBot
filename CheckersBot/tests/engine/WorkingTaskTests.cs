using CheckersBot.engine;
using CheckersBot.engine.threads;
using CheckersBot.logic;
using CheckersBot.logic.pieces;
using NUnit.Framework;

namespace CheckersBot.tests.engine;
[TestFixture]
public class WorkingTaskTests : AbstractTestWithEnvironment
{
    private readonly Board _defaultBoard = new Board(new BoardPositionSetting(
        PathResolver.ResolvePathFromSolutionRoot("/tests/startingPositions/defaultPosition.txt")));
    private readonly Board _boardCloseToWinning = new Board(new BoardPositionSetting(
        PathResolver.ResolvePathFromSolutionRoot("/tests/startingPositions/boardCloseToWinning.txt")));
    [Test]
    public async Task SimpleOneThreadDFSTest1()
    {
        WorkingTask workingTask = new WorkingTask(_defaultBoard,PieceColor.Black,1);
        Task<(MoveSequence, double)> eventTask = TestingUtils.WaitForResultReport(workingTask);
        Thread thread = new Thread(workingTask.Run);
        thread.Start();
        (MoveSequence, double) result = await eventTask;
        (MoveSequence, double) expected = (new MoveSequence().PushMoveToBack(new Move(5, 6, 4, 5)),
            0.1);
        Assert.That(TestingUtils.CompareMoveSequenceWithEval(result,expected), Is.True);
    }
    [Test]
    public async Task SimpleOneThreadDFSTest2()
    {
        WorkingTask workingTask = new WorkingTask(_defaultBoard,PieceColor.Black,2);
        Task<(MoveSequence, double)> eventTask = TestingUtils.WaitForResultReport(workingTask);
        Thread thread = new Thread(workingTask.Run);
        thread.Start();
        (MoveSequence, double) result = await eventTask;
        (MoveSequence, double) expected = (new MoveSequence()
                .PushMoveToBack(new Move(5, 6, 4, 7))
                .PushMoveToBack(new Move(2, 7,3,6)),
            0.0);
        Assert.That(TestingUtils.CompareMoveSequenceWithEval(result,expected), Is.True);
    }
    [Test]
    public async Task SimpleOneThreadDFSTest3()
    {
        WorkingTask workingTask = new WorkingTask(_defaultBoard,PieceColor.Black,3);
        Task<(MoveSequence, double)> eventTask = TestingUtils.WaitForResultReport(workingTask);
        Thread thread = new Thread(workingTask.Run);
        thread.Start();
        (MoveSequence, double) result = await eventTask;
        (MoveSequence, double) expected = (new MoveSequence().PushMoveToBack(new Move(5, 6, 4, 7))
                .PushMoveToBack(new Move(2, 1,3,0))
                .PushMoveToBack(new Move(5, 4,4,5)),
            0.2);
        Assert.That(TestingUtils.CompareMoveSequenceWithEval(result,expected), Is.True);
    }
    [Test]
    public async Task SimpleOneThreadDFSTest4()
    {
        WorkingTask workingTask = new WorkingTask(_defaultBoard,PieceColor.Black,4);
        Task<(MoveSequence, double)> eventTask = TestingUtils.WaitForResultReport(workingTask);
        Thread thread = new Thread(workingTask.Run);
        thread.Start();
        (MoveSequence, double) result = await eventTask;
        
        AttackingMove move3 = new AttackingMove(4, 5, 2, 3);
        move3.AddKilledPiece(new ManPiece(3,4, PieceColor.White));
        move3.AddVisitedSquare(new SquareIndex(2,3));
        
        AttackingMove move4 = new AttackingMove(1, 4, 3, 2);
        move4.AddKilledPiece(new ManPiece(2,3, PieceColor.Black));
        move4.AddVisitedSquare(new SquareIndex(3,2));
        
        (MoveSequence, double) expected = (new MoveSequence()
                .PushMoveToBack(new Move(5, 6,4,5))
                .PushMoveToBack(new Move(2, 3,3,4))
                .PushMoveToBack(move3)
                .PushMoveToBack(move4),
            -0.2);
        Console.WriteLine(result);
        Assert.That(TestingUtils.CompareMoveSequenceWithEval(result,expected), Is.True);
    }

    [Test]
    public async Task CloseToWinningTest()
    {
        WorkingTask workingTask = new WorkingTask(_boardCloseToWinning, PieceColor.Black, 3);
        Task<(MoveSequence, double)> eventTask = TestingUtils.WaitForResultReport(workingTask);
        Thread thread = new Thread(workingTask.Run);
        thread.Start();
        (MoveSequence, double) result = await eventTask;
        AttackingMove move2 = new AttackingMove(0, 5, 2, 7);
        move2.AddKilledPiece(new KingPiece(1, 6, PieceColor.Black));
        move2.AddVisitedSquare(new SquareIndex(2, 7));
        (MoveSequence, double) expected = (new MoveSequence()
                .PushMoveToBack(new Move(0, 7, 1, 6))
                .PushMoveToBack(move2),
            double.MinValue);
        Console.WriteLine(result);
        Assert.That(TestingUtils.CompareMoveSequenceWithEval(result, expected), Is.True);
    }
}