using CheckersBot.logic;
using NUnit.Framework;

namespace CheckersBot.tests.logic;

[TestFixture]
public class WinningGameTests
{
    private readonly Board _defaultBoard = new Board(new BoardPositionSetting(
        PathResolver.ResolvePathFromSolutionRoot("/tests/startingPositions/defaultPosition.txt")));
    private readonly Board _boardWithWin = new Board(new BoardPositionSetting(
        PathResolver.ResolvePathFromSolutionRoot("/tests/startingPositions/boardWithWin.txt")));
    [Test]
    public void WinningGameTest1()
    {
        Assert.That(_boardWithWin.WhoWonTheGame(), Is.Not.Null);
        Assert.That(_boardWithWin.WhoWonTheGame(), Is.EqualTo(PieceColor.White));
    }
    [Test]
    public void WinningGameTest2()
    {
        Assert.That(_defaultBoard.WhoWonTheGame(), Is.Null);
    }
}