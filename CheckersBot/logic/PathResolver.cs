namespace CheckersBot.logic;

using Path = System.IO.Path;
/// <summary>
/// Class, which responsible for Path in the System
/// </summary>
public static class PathResolver
{
    /// <summary>
    /// Returns Path from solution root
    /// </summary>
    /// <param name="relativePath"> relative path from solution root </param>
    /// <returns> Absolute path from solution root</returns>
    public static string ResolvePathFromSolutionRoot(string? relativePath)
    {
        if (String.IsNullOrWhiteSpace(relativePath))
            return Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\"));
        return Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\" + relativePath));
    }
}