using CheckersBot.logic;
using CheckersBot.logic.pieces;
using NUnit.Framework;

namespace CheckersBot.tests.logic;

[TestFixture]
public class MoveTests
{
    
    private readonly Board _defaultBoard = new Board(new BoardPositionSetting(
        PathResolver.ResolvePathFromSolutionRoot("/tests/startingPositions/defaultPosition.txt")));
    private readonly Board _boardWithKings = new Board(new BoardPositionSetting(
        PathResolver.ResolvePathFromSolutionRoot("/tests/startingPositions/boardWithKings.txt")));
    [Test]
    public void CheckManMoveOfOnePiece1()
    {
        string expected = "Move{xStart=2, yStart=1, xEnd=3, yEnd=2}\nMove{xStart=2, yStart=1, xEnd=3, yEnd=0}\n";
        Piece manPiece = new ManPiece(2, 1, PieceColor.White);
        string actual = Utils.CollectionToString(manPiece.GetAllMovesInBounds(_defaultBoard));
        Assert.That(actual, Is.EqualTo(expected));        
    }
    [Test]
    public void CheckManMoveOfOnePiece2()
    {
        string expected = "Move{xStart=5, yStart=2, xEnd=4, yEnd=3}\nMove{xStart=5, yStart=2, xEnd=4, yEnd=1}\n";
        Piece manPiece = new ManPiece(5, 2, PieceColor.Black);
        string actual = Utils.CollectionToString(manPiece.GetAllMovesInBounds(_defaultBoard));
        Assert.That(actual, Is.EqualTo(expected));        
    }
    [Test]
    public void CheckManMoveOfOnePiece3()
    {
        string expected = "";
        Piece manPiece = new ManPiece(6, 3, PieceColor.Black);
        string actual = Utils.CollectionToString(manPiece.GetAllMovesInBounds(_defaultBoard));
        Assert.That(actual, Is.EqualTo(expected));        
    }
    [Test]
    public void CheckKingMoveOfOnePiece1()
    {
        string expected = "Move{xStart=2, yStart=1, xEnd=3, yEnd=2}\nMove{xStart=2, yStart=1, xEnd=3, yEnd=0}\n" +
                          "Move{xStart=2, yStart=1, xEnd=1, yEnd=0}\n";
        Piece kingPiece = new KingPiece(2,1 ,PieceColor.White);
        string actual = Utils.CollectionToString(kingPiece.GetAllMovesInBounds(_boardWithKings));
        Assert.That(actual, Is.EqualTo(expected));
    }
    [Test]
    public void CheckKingMoveOfOnePiece2()
    {
        string expected = "Move{xStart=4, yStart=5, xEnd=5, yEnd=6}\nMove{xStart=4, yStart=5, xEnd=5, yEnd=4}\n" +
                          "Move{xStart=4, yStart=5, xEnd=3, yEnd=6}\nMove{xStart=4, yStart=5, xEnd=3, yEnd=4}\n";
        Piece kingPiece = new KingPiece(4,5 ,PieceColor.Black);
        string actual = Utils.CollectionToString(kingPiece.GetAllMovesInBounds(_boardWithKings));
        Assert.That(actual, Is.EqualTo(expected));
    }
}