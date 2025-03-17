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

    public bool IsInPlace()
    {
        return XStart == XEnd && YStart == YEnd;
    }

    public virtual Move ReverseMove()
    {
        Move newMove = new Move(XEnd, YEnd, XStart, YStart);
        newMove.WasMenBeforeMove = WasMenBeforeMove;
        return newMove;
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