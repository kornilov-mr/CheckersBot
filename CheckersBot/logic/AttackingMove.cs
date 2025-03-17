using CheckersBot.logic.pieces;

namespace CheckersBot.logic;
/// <summary>
/// Class, Which contains move and pieces that was killed before reaching end square
/// </summary>
/// <param name="xStart"> X-Position at the start </param>
/// <param name="yStart"> Y-Position at the start </param>
/// <param name="xEnd"> X-Position at the end </param>
/// <param name="yEnd"> Y-Position at the end </param>
public class AttackingMove(int xStart, int yStart, int xEnd, int yEnd)
    : Move(xStart, yStart, xEnd, yEnd)
{
    public List<Piece> KilledPieces { get; } = new List<Piece>();
    public List<SquareIndex> VisitedSquares { get; } = new List<SquareIndex>();

    public AttackingMove(AttackingMove move) : this(move.XStart, move.YStart, move.XEnd, move.YEnd)
    {
        AddKilledPieces(move.KilledPieces);
    }

    public void AddKilledPiece(Piece piece)
    {
        KilledPieces.Add(piece);
    }

    public void AddKilledPieces(List<Piece> pieces)
    {
        KilledPieces.AddRange(pieces);
    }

    public void AddVisitedSquare(SquareIndex square)
    {
        VisitedSquares.Add(square);
    }

    public void AddVisitedSquares(List<SquareIndex> squares)
    {
        VisitedSquares.AddRange(squares);
    }

    public new Object Clone()
    {
        AttackingMove move = new AttackingMove(XStart, YStart, XEnd, YEnd);
        move.AddKilledPieces(KilledPieces);
        move.AddVisitedSquares(VisitedSquares);
        return move;
    }

    public override Move ReverseMove()
    {
        AttackingMove newMove = new AttackingMove(XEnd, YEnd, XStart, YStart);
        newMove.AddKilledPieces(KilledPieces);
        newMove.AddVisitedSquares(VisitedSquares);
        return newMove;
    }

    public override string ToString()
    {
        return base.ToString() + " KilledPiece:" + Utils.CollectionToString(KilledPieces) +
               " VisitedSquares:" + Utils.CollectionToString(VisitedSquares);
    }

    public override bool Equals(object? obj)
    {
        if (obj == null) return false;
        if (obj == this) return true;
        if (!(obj is AttackingMove move)) return false;
        return base.Equals(move) &&
               Utils.CollectionEquals(KilledPieces, move.KilledPieces) &&
               Utils.CollectionEquals(VisitedSquares, move.VisitedSquares);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), KilledPieces, VisitedSquares);
    }
}