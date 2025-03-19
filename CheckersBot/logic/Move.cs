using CheckersBot.logic.pieces;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace CheckersBot.logic;

/// <summary>
/// Class, Which contains Start and End position of a Piece
/// </summary>
public class Move
{
    public int XStart { get; }
    public int YStart { get; }
    public int XEnd { get; set; }
    public int YEnd { get; set; }
    public bool WasMenBeforeMove { get; set; }

    public Move(int xStart, int yStart, int xEnd, int yEnd)
    {
        XStart = xStart;
        YStart = yStart;
        XEnd = xEnd;
        YEnd = yEnd;
    }

    public Move(SquareIndex start, SquareIndex end)
    {
        XStart = start.X;
        YStart = start.Y;
        XEnd = end.X;
        YEnd = end.Y;
    }

    public Move(Piece piece, int xRelativeShift, int yRelativeShift)
    {
        XStart = piece.XPosition;
        YStart = piece.YPosition;
        XEnd = XStart + xRelativeShift;
        YEnd = YStart + yRelativeShift;
    }

    public Object Clone()
    {
        Move newMove = new Move(XStart, YStart, XEnd, YEnd);
        newMove.WasMenBeforeMove = WasMenBeforeMove;
        return newMove;
    }
    /// <summary>
    /// return true if the end is the same as the start
    /// </summary>
    /// <returns></returns>
    public bool IsInPlace()
    {
        return XStart == XEnd && YStart == YEnd;
    }
    /// <summary>
    /// Clones the move but swapping start and end
    /// </summary>
    /// <returns></returns>
    public virtual Move ReverseMove()
    {
        Move newMove = new Move(XEnd, YEnd, XStart, YStart);
        newMove.WasMenBeforeMove = WasMenBeforeMove;
        return newMove;
    }
    /// <summary>
    /// Returns true if the move can be considered a part of an attacking move
    /// (Used for better ui move visualization)
    /// </summary>
    /// <param name="board"> current board </param>
    /// <param name="squaresToMoveThrough"> List of squares used has clicked on </param>
    /// <returns></returns>
    public bool CouldBeAttackingMove(Board board, List<SquareIndex> squaresToMoveThrough)
    {
        if (!(Math.Abs(XEnd - XStart) == 2 && Math.Abs(YStart - YEnd) == 2))
            return false;
        
        Piece? pieceInBetween = board.GetPieceInBetweenMove(this);
        Piece? pieceOnTheEnd = board.GetPieceOnMoveEnd(this);
        if (pieceInBetween == null) return false;
        if (pieceOnTheEnd != null) return false;
        if (squaresToMoveThrough.Count == 0)
        {
            Piece? pieceAtTheStart = board.GetPieceOnMoveStart(this);
            if(pieceAtTheStart == null) return false;
            if (pieceInBetween.Color != MoveUtils.SwitchColor(pieceAtTheStart.Color)) return false;
            return true;
        }
        else
        {
            Piece? pieceAtTheStart =
                board.Pieces[squaresToMoveThrough.ElementAt(0).X, squaresToMoveThrough.ElementAt(0).Y];
            if(pieceAtTheStart == null) return false;
            if (pieceInBetween.Color != MoveUtils.SwitchColor(pieceAtTheStart.Color)) return false;
        }
        return true;
    }

    public override string ToString()
    {
        return "Move{" +
               "xStart=" + XStart +
               ", yStart=" + YStart +
               ", xEnd=" + XEnd +
               ", yEnd=" + YEnd +
               '}';
    }

    public override bool Equals(object? obj)
    {
        if (obj == null) return false;
        if (obj == this) return true;
        if (!(obj is Move move)) return false;
        return XStart == move.XStart && YStart == move.YStart &&
               XEnd == move.XEnd && YEnd == move.YEnd;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(XStart, YStart, XEnd, YEnd);
    }
}