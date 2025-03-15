using CheckersBot.logic;
using CheckersBot.logic.pieces;
using NUnit.Framework;

namespace CheckersBot.tests.logic;
[TestFixture]
public class UndoMoveTests
{
    private readonly Board _defaultBoard = new Board(new BoardPositionSetting(
        PathResolver.ResolvePathFromSolutionRoot("/tests/startingPositions/defaultPosition.txt")));
    private readonly Board _boardWithPromotion = new Board(new BoardPositionSetting(
        PathResolver.ResolvePathFromSolutionRoot("/tests/startingPositions/boardWithPromotion.txt")));
    private readonly Board _boardSimpleChainAttack = new Board(new BoardPositionSetting(
        PathResolver.ResolvePathFromSolutionRoot("/tests/startingPositions/boardWithOneChainAttack.txt")));

    [Test]
    public void UndoNormalMoveTest()
    {
        Board operationBoard = new Board(new BoardPositionSetting(
            PathResolver.ResolvePathFromSolutionRoot("/tests/startingPositions/defaultPosition.txt")));
        Move move = new Move(5,0,4,1);
        operationBoard.MakeAMove(move);
        operationBoard.UndoLastMove();
        Assert.That(operationBoard, Is.EqualTo(_defaultBoard));
    }

    [Test]
    public void UndoPromotionTest()
    {
        Board operationBoard = new Board(new BoardPositionSetting(
            PathResolver.ResolvePathFromSolutionRoot("/tests/startingPositions/boardWithPromotion.txt")));
        Move move = new Move(1,1,0,0);
        operationBoard.MakeAMove(move);
        operationBoard.UndoLastMove();
        Assert.That(operationBoard, Is.EqualTo(_boardWithPromotion));
    }

    [Test]
    public void UndoAttackingMoveTest()
    {
        Board operationBoard = new Board(new BoardPositionSetting(
            PathResolver.ResolvePathFromSolutionRoot("/tests/startingPositions/boardWithOneChainAttack.txt")));
        AttackingMove move = new AttackingMove(3,2,1,4);
        move.AddKilledPiece(operationBoard.Pieces[2,3]!);
        move.AddVisitedSquare(new SquareIndex(1,4));
        operationBoard.MakeAMove(move);
        operationBoard.UndoLastMove();
        Assert.That(operationBoard, Is.EqualTo(_boardSimpleChainAttack));
    }
}