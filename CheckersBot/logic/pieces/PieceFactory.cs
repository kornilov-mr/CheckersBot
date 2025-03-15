namespace CheckersBot.logic.pieces;

public static class PieceFactory
{
    /// <summary>
    /// Creates a piece from line, while parsing setting file
    /// </summary>
    /// <param name="pieceString"> String in setting file</param>
    /// <param name="x"> X-Position of the Piece</param>
    /// <param name="y"> Y-Position of the Piece</param>
    /// <returns> A piece created from setting </returns>
    public static Piece? CreatePiece(string pieceString, int x, int y)
    {
        Piece? piece = null;

        switch (pieceString)
        {
            case "MWh":
                piece = new ManPiece(x, y, PieceColor.White);
                break;
            case "MBl":
                piece = new ManPiece(x, y, PieceColor.Black);
                break;
            case "KBl":
                piece = new KingPiece(x, y, PieceColor.Black);
                break;
            case "KWh":
                piece = new KingPiece(x, y, PieceColor.White);
                break;
            case "---":
                break;
        }

        return piece;
    }

    /// <summary>
    /// Creates string representation of a piece for console game 
    /// </summary>
    /// <param name="piece"> Piece to convert </param>
    /// <returns> string representation </returns>
    public static string CreateStringFromPiece(Piece? piece)
    {
        if (piece is null)
            return "---";
        if (piece is ManPiece && piece.Color == PieceColor.White)
            return "MWh";
        if (piece is ManPiece && piece.Color == PieceColor.Black)
            return "MBl";
        if (piece is KingPiece && piece.Color == PieceColor.White)
            return "KWh";
        if (piece is KingPiece && piece.Color == PieceColor.Black)
            return "KBl";
        return "";
    }
}