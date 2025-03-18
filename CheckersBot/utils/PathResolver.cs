namespace CheckersBot.utils;
using Path = System.IO.Path;

public abstract class PathResolver
{
    public static string ResolvePathFromSolutionRoot(string? relativePath)
    {
        if(String.IsNullOrWhiteSpace(relativePath))
            return Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\"));
        return Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\"+relativePath));
    }
}