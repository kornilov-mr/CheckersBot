using CheckersBot.logic;
using CheckersBot.logic.pieces;
using NUnit.Framework;

namespace CheckersBot.tests.logic;

[TestFixture]
public class PromotionalMoveTest
{
   private readonly Board _boardWithPromotion = new Board(new BoardPositionSetting(
      PathResolver.ResolvePathFromSolutionRoot("/tests/startingPositions/boardWithPromotion.txt")));
   [Test]
   public void TestPromotionalMove1()
   {
      Move move = new Move(1,1 , 0 ,0);
      _boardWithPromotion.MakeAMove(move);
      Piece pieceAfter = _boardWithPromotion.Pieces[0, 0]!;
      if (pieceAfter is KingPiece)
      {
         Assert.Pass();
         return;
      }
      Assert.Fail();
   }
}