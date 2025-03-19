using CheckersBot.logic;

namespace CheckersBot.gameControl.gameController;
/// <summary>
/// class to play without bot (mainly used for manual UI testing)
/// </summary>
/// <param name="board"></param>
public class OnePersonGameController(Board board) : AbstractGameController(board)
{
    protected override void StartGameInternal()
    {
        
    }
}