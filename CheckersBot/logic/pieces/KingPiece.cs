namespace CheckersBot.logic.pieces;
/// <summary>
/// Piece, which can move back
/// </summary>
public class KingPiece : Piece
{
    public KingPiece(int xPosition, int yPosition, PieceColor color) : base(xPosition, yPosition, color)
    {
    }

    protected override List<Move> GetAllMoves(Board board)
    {
        List<Move> moves = new List<Move>();
        moves.Add(new Move(this, 1, 1));
        moves.Add(new Move(this, 1, -1));
        moves.Add(new Move(this, -1, 1));
        moves.Add(new Move(this, -1, -1));
        return MoveUtils.FilterAllBlockedMoves(moves, board);
    }

    protected override List<Move> GetAllAttackingMoves(Board board)
    {
        AttackingMove startMove = new AttackingMove(XPosition, YPosition, XPosition, YPosition);
        return board.Walker.GetAllAttackingMovesFromSquare(XPosition, YPosition,
            MoveUtils.SwitchColor(Color), startMove).Cast<Move>().ToList();
    }

    public override string ToString()
    {
        return "KingPiece" + base.ToString();
    }

    public override bool Equals(object? obj)
    {
        return base.Equals(obj) && obj is KingPiece;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}