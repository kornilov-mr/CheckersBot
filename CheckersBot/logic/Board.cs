using CheckersBot.logic.pieces;

namespace CheckersBot.logic;
/// <summary>
/// Class with all information about the board
/// </summary>
public class Board
{
    /// <summary>
    /// Object for finding all attacking moves
    /// </summary>
    public AttackEdgeWalker Walker { get; }
    /// <summary>
    /// All pieces in 8x8
    /// </summary>
    public Piece?[,] Pieces { get; }
    /// <summary>
    /// All moves, that are valid at this move
    /// </summary>
    public HashSet<Move> AllAvailableNormalMoves { get; } = new HashSet<Move>();
    /// <summary>
    /// All attacking moves, that are valid at this move
    /// </summary>
    public HashSet<AttackingMove> AllAvailableAttackingMoves { get; } = new HashSet<AttackingMove>();
    /// <summary>
    /// Next color to play
    /// </summary>
    public PieceColor ColorToMove { get; private set; }

    public Board(BoardPositionSetting boardPositionSetting) : this(boardPositionSetting, PieceColor.Black)
    {
    }

    public Board(BoardPositionSetting boardPositionSetting, PieceColor startingColor)
    {
        Pieces = boardPositionSetting.Pieces;
        Walker = new AttackEdgeWalker(this);
        ColorToMove = startingColor;
        UpdateMoves(startingColor);
    }

    public Piece? GetPieceOnMoveEnd(Move move)
    {
        if (!IsMoveEndInBounds(move)) return null;
        return Pieces[move.XEnd, move.YEnd];
    }

    public bool IsMoveEndInBounds(Move move)
    {
        return IsInBounds(move.XEnd, move.YEnd);
    }

    public bool IsInBounds(int x, int y)
    {
        return x >= 0 && y >= 0 && x < 8 && y < 8;
    }
    /// <summary>
    /// Executed a move, throws exception if the move wasn't valid
    /// </summary>
    /// <param name="move"> Move to execute </param>
    /// <exception cref="ArgumentException"> throws exception if the move wasn't valid </exception>
    public void MakeAMove(Move move)
    {
        if (!IsMoveEndInBounds(move)) throw new ArgumentException("Invalid move");
        Pieces[move.XEnd, move.YEnd] = Pieces[move.XStart, move.YStart];
        Pieces[move.XEnd, move.YEnd]!.MoveToMoveEnd(move);
        Pieces[move.XStart, move.YStart] = null;
        if (move is AttackingMove attackingMove)
        {
            foreach (var piece in attackingMove.KilledPieces)
            {
                Pieces[piece.XPosition, piece.YPosition] = null;
            }
        }

        ColorToMove = MoveUtils.SwitchColor(ColorToMove);
        UpdateMoves(ColorToMove);
    }
    /// <summary>
    /// Check if the move is possible
    /// </summary>
    /// <param name="move"> Move to check </param>
    /// <returns>bool, true if valid </returns>
    public bool IsMoveValid(Move move)
    {
        if (Pieces[move.XStart, move.YStart] == null) return false;
        if (Pieces[move.XEnd, move.YEnd] != null) return false;
        if (move is AttackingMove attackingMove)
            return Utils.CollectionContains(AllAvailableAttackingMoves, attackingMove);

        if (AllAvailableAttackingMoves.Count != 0)
            return false;
        return Utils.CollectionContains(AllAvailableNormalMoves, move);
    }
    /// <summary>
    /// Calculates all Moves based on the color
    /// </summary>
    /// <param name="color"> Color to play </param>
    /// <returns> Collection with all possible moves for color</returns>
    private List<Move> GetAllMovesForColor(PieceColor color)
    {
        List<Move> moves = new List<Move>();
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (Pieces[j, i] != null && Pieces[j, i]!.Color.Equals(color))
                {
                    moves.AddRange(Pieces[j, i]!.GetAllMovesInBounds(this));
                }
            }
        }

        return moves;
    }
    /// <summary>
    /// Calculates all attacking Moves based on the color
    /// </summary>
    /// <param name="color"> Color to play </param>
    /// <returns> Collection with all possible attacking moves for color</returns>
    private List<Move> GetAllAttackingMovesForColor(PieceColor color)
    {
        List<Move> moves = new List<Move>();
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (Pieces[j, i] != null && Pieces[j, i]!.Color.Equals(color))
                {
                    moves.AddRange(Pieces[j, i]!.GetAllAttackingMovesInBounds(this));
                }
            }
        }

        return moves;
    }
    /// <summary>
    /// Updates AllAvailableNormalMoves and AllAvailableAttackingMoves for next turn
    /// </summary>
    /// <param name="colorToMove"> Color to play </param>
    private void UpdateMoves(PieceColor colorToMove)
    {
        AllAvailableNormalMoves.Clear();
        foreach (var move in GetAllMovesForColor(colorToMove))
        {
            AllAvailableNormalMoves.Add(move);
        }

        AllAvailableAttackingMoves.Clear();
        foreach (var move in GetAllAttackingMovesForColor(colorToMove))
        {
            AllAvailableAttackingMoves.Add((AttackingMove)move);
        }
    }
}