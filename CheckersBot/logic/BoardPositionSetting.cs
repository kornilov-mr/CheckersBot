using System.IO;
using CheckersBot.logic.pieces;

namespace CheckersBot.logic;
/// <summary>
/// Class, which is responsible for parsing setting file
/// </summary>
public class BoardPositionSetting
{
    public HashSet<Piece> WhitePieces { get; } = new HashSet<Piece>();
    public HashSet<Piece> BlackPieces { get; } = new HashSet<Piece>();
    public Piece?[,] Pieces { get; } = new Piece?[8, 8];

    public BoardPositionSetting(string settingFile)
    {
        ParseSettingFile(settingFile);
    }

    private void ParseSettingFile(string settingFile)
    {
        string content = File.ReadAllText(settingFile);
        content = content.Replace("\r\n", "\n");
        string[] strings = content.Split('\n');
        for (int i = 0; i < strings.Length; i++)
        {
            string[] piecesString = strings[i].Split(' ');
            for (int j = 0; j < piecesString.Length; j++)
            {
                Piece? piece = PieceFactory.CreatePiece(piecesString[j], i, j);
                Pieces[i, j] = piece;
                if(piece == null) continue;
                if(piece.Color == PieceColor.White) WhitePieces.Add(piece);
                if(piece.Color == PieceColor.Black) BlackPieces.Add(piece);
            }
        }
    }
}