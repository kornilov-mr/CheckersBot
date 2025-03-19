namespace CheckersBot.logic.pieces;
/// <summary>
/// Piece, which can move back
/// </summary>
public class KingPiece : Piece
{
    public KingPiece(int xPosition, int yPosition, PieceColor color) : base(xPosition, yPosition, color,1)
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

    public override Piece Clone()
    {
        return new KingPiece(XPosition, YPosition, Color);
    }

    public override Piece CloneWithNewPosition(int x, int y)
    {
        return new KingPiece(x, y, Color);
    }

    public override string ToString()
    {
        return "KingPiece" + base.ToString();
    }

    

}