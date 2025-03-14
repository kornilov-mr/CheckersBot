namespace CheckersBot.logic;

public static class MoveUtils
{
    /// <summary>
    /// Filter all move that are blocked by a piece of all colors
    /// </summary>
    /// <param name="moves"> moves to filter </param>
    /// <param name="board"> main playing field </param>
    /// <returns> Collection of all possible moves </returns>
    public static List<Move> FilterAllBlockedMoves(List<Move> moves, Board board)
    {
        List<Move> filteredMoves = new List<Move>();
        foreach (var currMove in moves)
        {
            if (board.GetPieceOnMoveEnd(currMove) == null)
            {
                filteredMoves.Add(currMove);
            }
        }

        return filteredMoves;
    }
    /// <summary>
    /// Filter all move that are out of bounds of the board
    /// </summary>
    /// <param name="moves"> moves to filter </param>
    /// <param name="board"> main playing field </param>
    /// <returns> Collection of all possible moves </returns>
    public static List<Move> FilterMoveOutOfBounds(List<Move> moves, Board board)
    {
        List<Move> filteredMoves = new List<Move>();
        foreach (var currMove in moves)
        {
            if (board.IsMoveEndInBounds(currMove))
            {
                filteredMoves.Add(currMove);
            }
        }

        return filteredMoves;
    }
    /// <summary>
    /// Switches color to the opposite
    /// </summary>
    /// <param name="pieceColor"> Color to change </param>
    /// <returns> opposite color </returns>
    public static PieceColor SwitchColor(PieceColor pieceColor)
    {
        return pieceColor.Equals(PieceColor.Black) ? PieceColor.White : PieceColor.Black;
    }
}