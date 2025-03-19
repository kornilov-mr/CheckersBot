using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using CheckersBot.gameControl.gameController;
using CheckersBot.logic;
using CheckersBot.logic.pieces;
using CheckersBot.UI.resources;

namespace CheckersBot.UI.components.board;

public class PiecesLayer : UniformGrid
{
    private DockPanel[,] Squares { get; } = new DockPanel[8, 8];

    private int _squareSize = int.Parse(
        Environment.GetEnvironmentVariable("PLAYING_SQUARE_SIZE")!);

    private SquareIndex? LastPressedSquare { get; set; }
    public AbstractGameController? GameController { get; private set; }

    private List<SquareIndex> LastSquares { get; } = new();

    public void SetUpBoard(AbstractGameController gameController)
    {
        GameController = gameController;
        GameController.OnMovePlayed += RenderMove;
        RedisplayAllPieces();
    }

    private void RedisplayAllPieces()
    {
        Board board = GameController!.Board;
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                Squares[i, j].Children.Clear();
                if (board.Pieces[i, j] != null)
                {
                    Squares[i, j].Children.Add(ImagePieceIconFactory.CreateIconImage(board.Pieces[i, j]!));
                }
                else
                {
                    Squares[i, j].Children.Add(CreateEmptyRectangle());
                }
            }
        }
    }

    private void RenderMove(Move move)
    {
        Dispatcher.BeginInvoke(new Action(() =>
        {
            Board board = GameController!.Board;
            Squares[move.XStart, move.YStart].Children.Clear();
            Squares[move.XStart, move.YStart].Children.Add(CreateEmptyRectangle());
            Squares[move.XEnd, move.YEnd].Children.Clear();
            Squares[move.XEnd, move.YEnd].Children
                .Add(ImagePieceIconFactory.CreateIconImage(board.Pieces[move.XEnd, move.YEnd]!));
            if (move is AttackingMove attackingMove)
            {
                foreach (var piece in attackingMove.KilledPieces)
                {
                    Squares[piece.XPosition, piece.YPosition].Children.Clear();
                    Squares[piece.XPosition, piece.YPosition].Children.Add(CreateEmptyRectangle());

                }
            }
        }));
    }


    protected override void OnInitialized(EventArgs e)
    {
        Height = _squareSize * 8;
        Width = _squareSize * 8;
        for (int row = 0; row < 8; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                DockPanel panel = new DockPanel();
                Squares[row, col] = panel;
                Children.Add(panel);
            }
        }
    }

    protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
    {
        Point position = e.GetPosition(this);

        double cellWidth = ActualWidth / 8;
        double cellHeight = ActualHeight / 8;
        int column = (int)(position.X / cellWidth);
        int row = (int)(position.Y / cellHeight);
        LastPressedSquare = new SquareIndex(row, column);
    }

    protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
    {
        if (LastPressedSquare == null) return;
        Point position = e.GetPosition(this);

        double cellWidth = ActualWidth / 8;
        double cellHeight = ActualHeight / 8;
        int column = (int)(position.X / cellWidth);
        int row = (int)(position.Y / cellHeight);

        Move moveToPlay = new Move(LastPressedSquare.X, LastPressedSquare.Y,
            row, column);
        Board board = GameController!.Board;
        if (moveToPlay.CouldBeAttackingMove(board, LastSquares))
        {
            AddNewSquarePair(LastPressedSquare, new SquareIndex(row, column));
        }
        if (LastSquares.Count != 0)
        {
            AttackingMove? attackingMove = MakeAttackingMoveFromSquares();
            if (attackingMove != null)
                moveToPlay = attackingMove;
        }
        if (GameController!.Board.IsMoveValid(moveToPlay))
        {
            GameController!.MakeAMove(moveToPlay);
            ClearLastSquares();
        }
        LastPressedSquare = null;
    }

    protected override void OnMouseRightButtonDown(MouseButtonEventArgs e)
    {
        ClearLastSquares();
    }

    private void ClearLastSquares()
    {
        for (int i = 0; i < LastSquares.Count-2; i += 2)
        {
            Squares[LastSquares[i+1].X, LastSquares[i+1].Y].Children.Clear();
            Squares[LastSquares[i+1].X, LastSquares[i+1].Y].Children.Add(CreateEmptyRectangle());
        }        
        LastSquares.Clear();
    }
    private void AddNewSquarePair(SquareIndex square1, SquareIndex square2)
    {
        LastSquares.Add(square1);
        LastSquares.Add(square2);
        Image ghostPiece = Resource.GetIcon("WhiteManPiece");
        ghostPiece.Opacity = 0.3;
        Squares[square2.X,square2.Y].Children.Clear();
        Squares[square2.X,square2.Y].Children.Add(ghostPiece);
    }
    private AttackingMove? MakeAttackingMoveFromSquares()
    {
        Board board = GameController!.Board;
        AttackingMove attackingMove = new AttackingMove(LastSquares.First().X, LastSquares.First().Y,
            LastSquares.Last().X, LastSquares.Last().Y);
        if (board.GetPieceOnMoveStart(attackingMove) == null) return null;
        for (int i = 0; i < LastSquares.Count; i += 2)
        {
            Move move = new Move(LastSquares.ElementAt(i), LastSquares.ElementAt(i + 1));
            Piece? pieceInBetween = board.GetPieceInBetweenMove(move);
            if (pieceInBetween == null) return null;
            if (pieceInBetween.Color != MoveUtils.SwitchColor(board.GetPieceOnMoveStart(attackingMove)!.Color))
                return null;
            if (board.GetPieceOnMoveEnd(move) != null) return null;
            attackingMove.AddKilledPiece(pieceInBetween);
            attackingMove.AddVisitedSquare(LastSquares.ElementAt(i + 1));
        }
        return attackingMove;
    }

    private Rectangle CreateEmptyRectangle()
    {
        return new Rectangle
        {
            Width = _squareSize,
            Height = _squareSize,
            Fill = Brushes.Blue,
            Opacity = 0
        };
    }
}