using System.Collections.Generic;

public static class EncounterResultData
{
    public static Dictionary<int, bool> levelResults = new Dictionary<int, bool>();

    public static void SetResult(int level, bool isWin)
    {
        levelResults[level] = isWin;
    }

    public static bool HasResult(int level)
    {
        return levelResults.ContainsKey(level);
    }

    public static bool GetResult(int level)
    {
        return levelResults[level];
    }
}