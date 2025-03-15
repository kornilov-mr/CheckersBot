using CheckersBot.logic;

namespace CheckersBot.gameControl.gameController;

public class ConsoleGameController(Board board) : AbstractGameController(board)
{
    protected override void StartGameInternal()
    {
        Console.WriteLine(Board.ToString());
        while (true)
        {
            string moveString = Console.ReadLine()!;
            if (moveString.Equals("exit")) return;
            string[] moveArgs = moveString.Split(" ");
            Move move = new Move(int.Parse(moveArgs[0]), int.Parse(moveArgs[1]),
                int.Parse(moveArgs[2]), int.Parse(moveArgs[3]));
            MakeAMove(move);
            Console.WriteLine(Board.ToString());
        }
    }
}