using CheckersBot.logic;
using CheckersBot.logic.pieces;
using NUnit.Framework;

namespace CheckersBot.tests.logic;
[TestFixture]
public class InvalidMovesTests
{

    private readonly Board _boardSimpleChainAttack = new Board(new BoardPositionSetting(
        PathResolver.ResolvePathFromSolutionRoot("/tests/startingPositions/boardWithOneChainAttack.txt")));
    private readonly Board _defaultBoardBlackStart = new Board(new BoardPositionSetting(
        PathResolver.ResolvePathFromSolutionRoot("/tests/startingPositions/defaultPosition.txt")));
    private readonly Board _defaultBoardWhiteStarts = new Board(new BoardPositionSetting(
        PathResolver.ResolvePathFromSolutionRoot("/tests/startingPositions/defaultPosition.txt")), PieceColor.White);

    [Test]
    public void InvalidNormalMoveTest1()
    {
        Move move = new Move(2,1,3,0);
        Assert.That(_defaultBoardBlackStart.IsMoveValid(move), Is.False);
    }
    [Test]
    public void InvalidNormalMoveTest2()
    {
        Move move = new Move(5,0,4,1);
        Assert.That(_defaultBoardWhiteStarts.IsMoveValid(move), Is.False);
    }
    [Test]
    public void InvalidNormalMoveTest3()
    {
        Move move = new Move(1,0,2,1);
        Assert.That(_defaultBoardBlackStart.IsMoveValid(move), Is.False);
    }
    [Test]
    public void InvalidNormalMoveTest4()
    {
        Move move = new Move(1,0,2,0);
        Assert.That(_defaultBoardBlackStart.IsMoveValid(move), Is.False);
    }
    [Test]
    public void InvalidAttackingMoveTest1()
    {
        AttackingMove move = new AttackingMove(3,2,1,4);
        move.AddKilledPiece(new ManPiece(2,3, PieceColor.White));
        move.AddVisitedSquare(new SquareIndex(1,3));
        
        Assert.That(_boardSimpleChainAttack.IsMoveValid(move), Is.False);
    }
    [Test]
    public void InvalidAttackingMoveTest2()
    {
        AttackingMove move = new AttackingMove(3,2,1,4);
        move.AddKilledPiece(new ManPiece(2,2, PieceColor.White));
        move.AddVisitedSquare(new SquareIndex(1,4));
        
        Assert.That(_boardSimpleChainAttack.IsMoveValid(move), Is.False);
    }
    [Test]
    public void InvalidAttackingMoveTest3()
    {
        AttackingMove move = new AttackingMove(3,2,1,3);
        move.AddKilledPiece(new ManPiece(2,3, PieceColor.White));
        move.AddVisitedSquare(new SquareIndex(1,4));
        
        Assert.That(_boardSimpleChainAttack.IsMoveValid(move), Is.False);
    }
}