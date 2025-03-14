namespace CheckersBot.logic;
/// <summary>
/// Class, which contains index of a square
/// </summary>
/// <param name="x"> X-Position of a square </param>
/// <param name="y"> Y-Position of a square </param>
public class SquareIndex(int x, int y)
{
    public int X { get; } = x;
    public int Y { get; } = y;

    public override string ToString()
    {
        return "Move{" +
               "X=" + X +
               ", Y=" + Y +
               '}';
    }

    public override bool Equals(object? obj)
    {
        if (obj == null) return false;
        if (obj == this) return true;
        if (!(obj is SquareIndex square)) return false;
        return X == square.X && Y == square.Y;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }
}