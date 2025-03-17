using CheckersBot.logic;

namespace CheckersBot.engine;

public class MoveSequence
{
    public List<Move> MoveStack { get; } = new();

    public MoveSequence PushMoveToBack(Move move)
    {
        MoveStack.Add(move);
        return this;
    }

    public MoveSequence JoinSequence(MoveSequence moveSequence)
    {
        foreach (var move in moveSequence.MoveStack)
        {
            MoveStack.Add(move);
        }

        return this;
    }

    public override string ToString()
    {
        string s = "";
        foreach (Move move in MoveStack)
        {
            s += move + Environment.NewLine;
        }

        return s;
    }

#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public override bool Equals(object? obj)
    {
        if (obj == null) return false;
        if (obj == this) return true;
        if (!(obj is MoveSequence moveSequence)) return false;
        if (moveSequence.MoveStack.Count != MoveStack.Count) return false;
        for (int i = 0; i < MoveStack.Count; i++)
        {
            if (!MoveStack[i].Equals(moveSequence.MoveStack[i])) return false;
        }

        return true;
    }
}