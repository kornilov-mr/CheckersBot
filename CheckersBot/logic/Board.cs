using CheckersBot.logic.pieces;

namespace CheckersBot.logic;

/// <summary>
/// Class with all information about the board
/// </summary>
public class Board
{
    /// <summary>
    /// Holding all previous move that got executed
    /// </summary>
    public Stack<Move> MovesStack { get; } = new Stack<Move>();

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

    public Board(BoardPositionSetting boardPositionSetting) :
        this(boardPositionSetting, PieceColor.Black)
    {
    }

    public Board(BoardPositionSetting boardPositionSetting, PieceColor startingColor) :
        this(boardPositionSetting.Pieces!, startingColor)
    {
    }

    public Board(Piece[,] pieces, PieceColor startingColor)
    {
        Pieces = pieces;
        Walker = new AttackEdgeWalker(this);
        ColorToMove = startingColor;
        UpdateMoves(startingColor);
    }

    /// <summary>
    /// Executed a move, throws exception if the move wasn't valid
    /// </summary>
    /// <param name="move"> Move to execute </param>
    /// <exception cref="ArgumentException"> throws exception if the move wasn't valid </exception>
    public void MakeAMove(Move move)
    {
        if (!IsMoveValid(move))
        {
            Console.WriteLine($"Invalid move: {move}");
        }
        if (!IsMoveValid(move)) throw new ArgumentException("Invalid move:" + move);
        move.WasMenBeforeMove = true;
        Piece pieceOnStart = Pieces[move.XStart, move.YStart]!;
        Piece pieceClone = pieceOnStart.CloneWithNewPosition(move.XEnd, move.YEnd);
        Pieces[move.XEnd, move.YEnd] = pieceClone;
        Pieces[move.XStart, move.YStart] = null;


        if (Pieces[move.XEnd, move.YEnd] is KingPiece)
            move.WasMenBeforeMove = false;
        
        if (Pieces[move.XEnd, move.YEnd] is ManPiece manPiece && manPiece.IsAtTheEndOfTheBoard())
        {
            KingPiece newPiece = new KingPiece(move.XEnd, move.YEnd,
                Pieces[move.XEnd, move.YEnd]!.Color);
            Pieces[move.XEnd, move.YEnd] = newPiece;
        }

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
        MovesStack.Push(move);
    }

    /// <summary>
    /// Executes last move in reverse (used for the engine to path tree in reverse order) 
    /// </summary>
    public void UndoLastMove()
    {
        Move move = MovesStack.Pop();
        Move reversedMove = move.ReverseMove();
        Piece pieceOnStart = Pieces[reversedMove.XStart, reversedMove.YStart]!;
        Piece pieceClone = pieceOnStart.CloneWithNewPosition(reversedMove.XEnd, reversedMove.YEnd);

        Pieces[reversedMove.XEnd, reversedMove.YEnd] = pieceClone;
        Pieces[reversedMove.XStart, reversedMove.YStart] = null;

        if (Pieces[reversedMove.XEnd, reversedMove.YEnd] is KingPiece && reversedMove.WasMenBeforeMove)
        {
            ManPiece newPiece = new ManPiece(reversedMove.XEnd, reversedMove.YEnd,
                Pieces[reversedMove.XEnd, reversedMove.YEnd]!.Color);
            Pieces[reversedMove.XEnd, reversedMove.YEnd] = newPiece;
        }

        Pieces[reversedMove.XStart, reversedMove.YStart] = null;

        if (reversedMove is AttackingMove attackingMove)
        {
            foreach (var piece in attackingMove.KilledPieces)
            {
                Pieces[piece.XPosition, piece.YPosition] = piece;
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
        if (!IsMoveEndInBounds(move)) return false;
        if (Pieces[move.XStart, move.YStart] == null) return false;
        if (Pieces[move.XEnd, move.YEnd] != null) return false;
        if (move is AttackingMove attackingMove)
            return Utils.CollectionContains(AllAvailableAttackingMoves, attackingMove);

        if (AllAvailableAttackingMoves.Count != 0)
            return false;
        return Utils.CollectionContains(AllAvailableNormalMoves, move);
    }

    public HashSet<Move> GetActualValidMoves()
    {
        if (AllAvailableAttackingMoves.Count == 0)
            return AllAvailableNormalMoves;
        return AllAvailableAttackingMoves.Cast<Move>().ToHashSet();
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
                if (Pieces[j, i] != null && Pieces[j, i]!.Color == color)
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
                if (Pieces[j, i] != null && Pieces[j, i]!.Color == color)
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

    /// <summary>
    /// return the color of who won the game, if the game is still running will be null
    /// </summary>
    /// <returns></returns>
    public PieceColor? WhoWonTheGame()
    {
        if (AllAvailableNormalMoves.Count == 0 && AllAvailableAttackingMoves.Count == 0)
            return MoveUtils.SwitchColor(ColorToMove);
        return null;
    }

    public double EvaluateBoard()
    {
        PieceColor? whoWonTheGame = WhoWonTheGame();
        if (whoWonTheGame != null)
        {
            if (whoWonTheGame.Equals(PieceColor.Black))
            {
                return double.MaxValue;
            }
            else
            {
                return double.MinValue;
            }
        }

        double pieceCount = 0;

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (Pieces[j, i] != null)
                {
                    if (Pieces[j, i]!.Color == PieceColor.Black)
                    {
                        pieceCount += Pieces[j, i]!.PieceValue;
                    }
                    else
                    {
                        pieceCount -= Pieces[j, i]!.PieceValue;
                    }
                }
            }
        }

        double controlCount = 0;
        if (ColorToMove.Equals(PieceColor.Black))
        {
            controlCount += (double)AllAvailableNormalMoves.Count / 10 +
                            (double)AllAvailableAttackingMoves.Count / 10;
            controlCount -= (double)GetAllMovesForColor(MoveUtils.SwitchColor(ColorToMove)).Count / 10 +
                            (double)GetAllAttackingMovesForColor(MoveUtils.SwitchColor(ColorToMove)).Count / 10;
        }
        else
        {
            controlCount -= (double)AllAvailableNormalMoves.Count / 10 +
                            (double)AllAvailableAttackingMoves.Count / 10;
            controlCount += (double)GetAllMovesForColor(MoveUtils.SwitchColor(ColorToMove)).Count / 10 +
                            (double)GetAllAttackingMovesForColor(MoveUtils.SwitchColor(ColorToMove)).Count / 10;
        }

        return controlCount + pieceCount;
    }

    /// <summary>
    /// Clones board and all pieces
    /// </summary>
    /// <returns> New board with the same parameters </returns>
    public Board Clone()
    {
        Piece?[,] pieces = new Piece?[8, 8];
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (Pieces[i, j] == null)
                    continue;
                pieces[i, j] = Pieces[i, j]!.Clone();
            }
        }

        return new Board(pieces!, ColorToMove);
    }

    public bool IsInBounds(int x, int y)
    {
        return x >= 0 && y >= 0 && x < 8 && y < 8;
    }

    public Piece? GetPieceInBetweenMove(Move move)
    {
        if (!IsMoveStartInBounds(move)) return null;
        if (!IsMoveEndInBounds(move)) return null;
        if (!(Math.Abs(move.XEnd - move.XStart) == 2 && Math.Abs(move.YStart - move.YEnd) == 2)) return null;
        return Pieces[(move.XEnd + move.XStart) / 2, (move.YEnd + move.YStart) / 2];
        
    }
    public Piece? GetPieceOnMoveStart(Move move)
    {
        if (!IsMoveStartInBounds(move)) return null;
        return Pieces[move.XStart, move.YStart];
    }
    public Piece? GetPieceOnMoveEnd(Move move)
    {
        if (!IsMoveEndInBounds(move)) return null;
        return Pieces[move.XEnd, move.YEnd];
    }
    public bool IsMoveStartInBounds(Move move)
    {
        return IsInBounds(move.XStart, move.YStart);
    }
    public bool IsMoveEndInBounds(Move move)
    {
        return IsInBounds(move.XEnd, move.YEnd);
    }

    public override string ToString()
    {
        string s = "";
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                s += PieceFactory.CreateStringFromPiece(Pieces[i, j]) + " ";
            }

            s += "\n";
        }

        return s;
    }

    public override bool Equals(object? obj)
    {
        if (obj == null) return false;
        if (obj == this) return true;
        if (!(obj is Board board)) return false;
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (Pieces[i, j] == null && board.Pieces[i, j] == null)
                {
                    continue;
                }

                if (Pieces[i, j] == null && board.Pieces[i, j] != null)
                {
                    return false;
                }

                if (!Pieces[i, j]!.Equals(board.Pieces[i, j]))
                    return false;
            }
        }

        return true;
    }

    public override int GetHashCode()
    {
        return Pieces.GetHashCode();
    }
}