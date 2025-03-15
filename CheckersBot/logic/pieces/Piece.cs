// ReSharper disable NonReadonlyMemberInGetHashCode
namespace CheckersBot.logic.pieces;
/// <summary>
/// Abstract class for all pieces
/// </summary>

public abstract class Piece
{
    public int PieceValue { get; }
    public int XPosition { get; private set; }
    public int YPosition { get; private set; }
    public PieceColor Color { get; }

    public Piece(int xPosition, int yPosition, PieceColor color, int pieceValue)
    {
        XPosition = xPosition;
        YPosition = yPosition;
        Color = color;
        PieceValue = pieceValue;
    }
    /// <summary>
    /// Changes Position to move end
    /// </summary>
    /// <param name="move">Move that is being executed</param>
    public void MoveToMoveEnd(Move move)
    {
        XPosition = move.XEnd;
        YPosition = move.YEnd;
    }
    /// <summary>
    /// Returns all Moves from abstract method, which are in bound of the Board
    /// </summary>
    /// <param name="board">Main playing board</param>
    /// <returns>Collection on move, that are possible</returns>
    public List<Move> GetAllMovesInBounds(Board board)
    {
        return MoveUtils.FilterMoveOutOfBounds(GetAllMoves(board),
            board);
    }
    /// <summary>
    /// Returns all attacking Moves from abstract method, which are in bound of the Board
    /// </summary>
    /// <param name="board">Main playing board</param>
    /// <returns>Collection on move, that are possible</returns>
    public List<Move> GetAllAttackingMovesInBounds(Board board)
    {
        return MoveUtils.FilterMoveOutOfBounds(GetAllAttackingMoves(board),
            board);
    }
    /// <summary>
    /// Returns all Moves that are possible
    /// </summary>
    /// <param name="board">Main playing board</param>
    /// <returns>Collection on move, that are possible</returns>
    protected abstract List<Move> GetAllMoves(Board board);
    /// <summary>
    /// Returns all attacking Moves that are possible
    /// </summary>
    /// <param name="board">Main playing board</param>
    /// <returns>Collection on move, that are possible</returns>
    protected abstract List<Move> GetAllAttackingMoves(Board board);

    public override string ToString()
    {
        return "Piece{" +
               "xPosition=" + XPosition +
               ", yPosition=" + YPosition +
               ", pieceColor=" + Color +
               '}';
    }

    public override bool Equals(object? obj)
    {
        if (obj == null) return false;
        if (obj == this) return true;
        if (!(obj is Piece piece)) return false;
        return XPosition == piece.XPosition && YPosition == piece.YPosition && Color == piece.Color;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(XPosition, YPosition, (int)Color);
    }
}