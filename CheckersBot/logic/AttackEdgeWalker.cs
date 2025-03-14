namespace CheckersBot.logic;

/// <summary>
/// Class, which is responsible for calculating all attacks using recursion
/// </summary>
/// <param name="board"> Main playing board </param>
public class AttackEdgeWalker(Board board)
{
    private Board Board { get; } = board;

    /// <summary>
    /// Pieces that are already killed on current branch of recursion, help escape loops
    /// </summary>
    private bool[,] UsedPieces { get; } = new bool[8, 8];

    /// <summary>
    /// Returns all Attacks from a square on the board for certain color
    /// </summary>
    /// <param name="x"> X-Position of the Piece </param>
    /// <param name="y"> Y-Position of the Piece </param>
    /// <param name="colorToAttack"> Color of the Piece </param>
    /// <param name="move"> Previous recursion Move </param> 
    /// <returns> Collection with all possible attacking moves </returns>
    public List<AttackingMove> GetAllAttackingMovesFromSquare(int x, int y, PieceColor colorToAttack,
        AttackingMove move)
    {
        List<AttackingMove> resultMoves = new List<AttackingMove>();
        resultMoves.AddRange(CalculateBranch(x, y, 1, 1, colorToAttack, move));
        resultMoves.AddRange(CalculateBranch(x, y, -1, 1, colorToAttack, move));
        resultMoves.AddRange(CalculateBranch(x, y, 1, -1, colorToAttack, move));
        resultMoves.AddRange(CalculateBranch(x, y, -1, -1, colorToAttack, move));
        if (resultMoves.Count == 0 && !move.IsInPlace())
        {
            return new List<AttackingMove>() { move };
        }

        return resultMoves;
    }
    /// <summary>
    /// A part of recursion on certain direction
    /// </summary>
    /// <param name="x"> X-Position of the Piece </param>
    /// <param name="y"> Y-Position of the Piece </param>
    /// <param name="xDirection"> X-Direction where the next move is going to go </param>
    /// <param name="yDirection"> Y-Direction where the next move is going to go </param>
    /// <param name="colorToAttack"> Color of the Piece </param>
    /// <param name="move"> Previous recursion Move </param>
    /// <returns> Collection with all possible attacking moves at this brach</returns>
    private List<AttackingMove> CalculateBranch(int x, int y, int xDirection, int yDirection,
        PieceColor colorToAttack, AttackingMove move)
    {
        List<AttackingMove> resultMoves = new List<AttackingMove>();
        if (!Board.IsInBounds(x + xDirection, y + yDirection) ||
            !Board.IsInBounds(x + xDirection * 2, y + yDirection * 2))
            return resultMoves;
        if (Board.Pieces[x + xDirection, y + yDirection] != null &&
            UsedPieces[x + xDirection, y + yDirection] == false &&
            Board.Pieces[x + xDirection, y + yDirection]!.Color.Equals(colorToAttack) &&
            Board.Pieces[x + xDirection * 2, y + yDirection * 2] == null)
        {
            AttackingMove moveCopy = (AttackingMove)move.Clone();
            moveCopy.AddKilledPiece(Board.Pieces[x + xDirection, y + yDirection]!);
            moveCopy.AddVisitedSquare(new SquareIndex(x + xDirection * 2, y + yDirection * 2));
            moveCopy.XEnd = x + xDirection * 2;
            moveCopy.YEnd = y + yDirection * 2;
            UsedPieces[x + xDirection, y + yDirection] = true;
            resultMoves.AddRange(GetAllAttackingMovesFromSquare(x + xDirection * 2, y + yDirection * 2,
                colorToAttack, moveCopy));
            UsedPieces[x + xDirection, y + yDirection] = false;
        }

        return resultMoves;
    }
}