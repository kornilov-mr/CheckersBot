using System.Windows.Controls;
using CheckersBot.logic;
using CheckersBot.logic.pieces;
using CheckersBot.UI.resources;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Resources;

namespace CheckersBot.UI.components.board;

public class ImagePieceIconFactory
{

    public static Image CreateIconImage(Piece piece)
    {
        if (piece is ManPiece)
        {
            if(piece.Color == PieceColor.White)
                return Resource.GetIcon("WhiteManPiece");
            if(piece.Color == PieceColor.Black)
                return Resource.GetIcon("BlackManPiece");
        }

        if (piece is KingPiece)
        {
            if(piece.Color == PieceColor.White)
                return Resource.GetIcon("WhiteKing");
            if(piece.Color == PieceColor.Black)
                return Resource.GetIcon("BlackKing");
        }
        throw new Exception("Unknown piece type or color");
    }
}