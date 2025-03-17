namespace CheckersBot.logic.pieces;
/// <summary>
/// Piece, which can't move back
/// </summary>
public class ManPiece : Piece
{
    public ManPiece(int xPosition, int yPosition, PieceColor color) : base(xPosition, yPosition, color,1)
    {
    }

    protected override List<Move> GetAllMoves(Board board)
    {
        List<Move> moves = new List<Move>();
        if (Color == PieceColor.White)
        {
            moves.Add(new Move(this, 1, 1));
            moves.Add(new Move(this, 1, -1));
        }
        else
        {
            moves.Add(new Move(this, -1, 1));
            moves.Add(new Move(this, -1, -1));
        }

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
        return new ManPiece(XPosition, YPosition, Color);
    }

    public override Piece CloneWithNewPosition(int x, int y)
    {
        return new ManPiece(x, y, Color);
    }

    public bool IsAtTheEndOfTheBoard()
    {
        if (Color == PieceColor.White)
        {
            return XPosition == 7;
        }
        else
        {
            return XPosition == 0;
        }
    }

    public override string ToString()
    {
        return "ManPiece" + base.ToString();
    }

    public override bool Equals(object? obj)
    {
        return base.Equals(obj) && obj is ManPiece;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}