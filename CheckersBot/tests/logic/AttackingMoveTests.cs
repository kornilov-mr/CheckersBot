using CheckersBot.logic;
using CheckersBot.logic.pieces;
using NUnit.Framework;

namespace CheckersBot.tests.logic;

[TestFixture]
public class AttackingMoveTests
{
    private readonly Board _boardSimpleChainAttack = new Board(new BoardPositionSetting(
        PathResolver.ResolvePathFromSolutionRoot("/tests/startingPositions/boardWithOneChainAttack.txt")));
    private readonly Board _boardWithBigChainAttack = new Board(new BoardPositionSetting(
        PathResolver.ResolvePathFromSolutionRoot("/tests/startingPositions/boardWithBigChainAttack.txt")));

    [Test]
    public void AttackingMoveTest1()
    {
        string expected = "Move{xStart=3, yStart=2, xEnd=1, yEnd=4} KilledPiece:ManPiecePiece{xPosition=2, yPosition=3," +
                          " pieceColor=White}\n VisitedSquares:Move{X=1, Y=4}\n\n";
        Piece manPiece = new ManPiece(3, 2, PieceColor.Black);
        string actual = Utils.CollectionToString(manPiece.GetAllAttackingMovesInBounds(_boardSimpleChainAttack));
        Assert.That(actual, Is.EqualTo(expected));
    }
    [Test]
    public void AttackingMoveTest2()
    {
        string expected = "Move{xStart=4, yStart=7, xEnd=6, yEnd=5} KilledPiece:ManPiecePiece{xPosition=5, yPosition=6," +
                          " pieceColor=Black}\n VisitedSquares:Move{X=6, Y=5}\n\n";
        Piece manPiece = new ManPiece(4, 7, PieceColor.White);
        string actual = Utils.CollectionToString(manPiece.GetAllAttackingMovesInBounds(_boardSimpleChainAttack));
        Assert.That(actual, Is.EqualTo(expected));
    }
    [Test]
    public void AttackingMoveTest3()
    {
        string expected = "Move{xStart=2, yStart=0, xEnd=0, yEnd=6} KilledPiece:ManPiecePiece{xPosition=1, yPosition=1," +
                          " pieceColor=White}\nManPiecePiece{xPosition=1, yPosition=3, pieceColor=White}" +
                          "\nManPiecePiece{xPosition=1, yPosition=5, pieceColor=White}" +
                          "\n VisitedSquares:Move{X=0, Y=2}\nMove{X=2, Y=4}\nMove{X=0, Y=6}" +
                          "\n\nMove{xStart=2, yStart=0, xEnd=4, yEnd=2}" +
                          " KilledPiece:ManPiecePiece{xPosition=1, yPosition=1, pieceColor=White}" +
                          "\nManPiecePiece{xPosition=1, yPosition=3, pieceColor=White}" +
                          "\nManPiecePiece{xPosition=3, yPosition=3, pieceColor=White}" +
                          "\n VisitedSquares:Move{X=0, Y=2}\nMove{X=2, Y=4}\nMove{X=4, Y=2}\n\n";
        Piece manPiece = new ManPiece(2, 0, PieceColor.Black);
        string actual = Utils.CollectionToString(manPiece.GetAllAttackingMovesInBounds(_boardWithBigChainAttack));
        Assert.That(actual, Is.EqualTo(expected));
    }
}