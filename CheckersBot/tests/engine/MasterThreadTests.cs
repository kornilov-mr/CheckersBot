using CheckersBot.engine;
using CheckersBot.logic;
using CheckersBot.logic.pieces;
using NUnit.Framework;

namespace CheckersBot.tests.engine;

[TestFixture]
public class MasterThreadTests : AbstractTestWithEnvironment
{
    private readonly Board _defaultBoard = new Board(new BoardPositionSetting(
        PathResolver.ResolvePathFromSolutionRoot("/tests/startingPositions/defaultPosition.txt")));

    private readonly Board _boardCloseToWinning = new Board(new BoardPositionSetting(
        PathResolver.ResolvePathFromSolutionRoot("/tests/startingPositions/boardCloseToWinning.txt")));

    [Test]
    public async Task MasterThreadTest1()
    {
        MasterThread masterThread = new MasterThread(_defaultBoard, 3, 5 * 1000);
        Task<(MoveSequence, double)> eventTask = TestingUtils.WaitForResultReport(masterThread);
        Thread thread = new Thread(masterThread.StartCalculation);
        thread.Start();
        (MoveSequence, double) result = await eventTask;
        (MoveSequence, double) expected = (new MoveSequence().PushMoveToBack(new Move(5, 6, 4, 7))
                .PushMoveToBack(new Move(2, 1, 3, 0))
                .PushMoveToBack(new Move(5, 4, 4, 5)),
            0.2);
        Console.WriteLine(result);
        Assert.That(TestingUtils.CompareMoveSequenceWithEval(result, expected), Is.True);
    }

    [Test]
    public async Task MasterThreadTest2()
    {
        MasterThread masterThread = new MasterThread(_defaultBoard, 2, 5 * 1000);
        Task<(MoveSequence, double)> eventTask = TestingUtils.WaitForResultReport(masterThread);
        Thread thread = new Thread(masterThread.StartCalculation);
        thread.Start();
        (MoveSequence, double) result = await eventTask;
        (MoveSequence, double) expected = (new MoveSequence()
                .PushMoveToBack(new Move(5, 6, 4, 7))
                .PushMoveToBack(new Move(2, 7, 3, 6)),
            0.0);
        Assert.That(TestingUtils.CompareMoveSequenceWithEval(result, expected), Is.True);
    }

    [Test]
    public async Task CloseToWinningTest()
    {
        MasterThread masterThread = new MasterThread(_boardCloseToWinning, 2, 5 * 1000);
        Task<(MoveSequence, double)> eventTask = TestingUtils.WaitForResultReport(masterThread);
        Thread thread = new Thread(masterThread.StartCalculation);
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