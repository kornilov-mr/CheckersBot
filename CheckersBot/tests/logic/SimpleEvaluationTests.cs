using CheckersBot.logic;
using NUnit.Framework;

namespace CheckersBot.tests.logic;

[TestFixture]
public class SimpleEvaluationTests
{
    private readonly Board _defaultBoard = new Board(new BoardPositionSetting(
        PathResolver.ResolvePathFromSolutionRoot("/tests/startingPositions/defaultPosition.txt")));
    private readonly Board _boardWithPromotion = new Board(new BoardPositionSetting(
        PathResolver.ResolvePathFromSolutionRoot("/tests/startingPositions/boardWithPromotion.txt")));

    [Test]
    public void SimpleEvaluationTest1()
    {
        Assert.That(0, Is.EqualTo(_defaultBoard.EvaluateBoard()));
    }
    [Test]
    public void SimpleEvaluationTest2()
    {
        Assert.That(7.5, Is.EqualTo(_boardWithPromotion.EvaluateBoard()));
    }
}