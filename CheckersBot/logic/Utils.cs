
namespace CheckersBot.logic;

public static class Utils
{
    public static string CollectionToString<T>(List<T> collection)
    {
        string s = "";
        foreach (var temp in collection)
        {
            s += temp + "\n";
        }

        return s;
    }

    public static bool CollectionEquals<T>(List<T> collection1, List<T> collection2)
    {
        if (collection1.Count != collection2.Count) return false;
        for (int i = 0; i < collection1.Count; i++)
        {
            if (!collection1[i]!.Equals(collection2[i])) return false;
        }
        return true;
    }
    public static bool CollectionContains<T>(HashSet<T> collection, T target)
    {
        foreach (var temp in collection)
        {
            if(temp!.Equals(target)) return true;
        }
        return false;
    }
}