using CheckersBot.logic;
using DotNetEnv;

namespace CheckersBot.tests;

public abstract class AbstractTestWithEnvironment
{
    protected AbstractTestWithEnvironment()
    {
        Env.Load(PathResolver.ResolvePathFromSolutionRoot(".env"));
    }
}