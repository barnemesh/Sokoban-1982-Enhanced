using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


[Serializable]
public class HighScores
{
    // todo: list size? cant use dict for some reason...
    public int levelCount = SceneManager.sceneCountInBuildSettings;
    public List<HighScoreEntry> entries = new List<HighScoreEntry>(SceneManager.sceneCountInBuildSettings);
}

[Serializable]
public class HighScoreEntry
{
    public int level;
    public string name;
    public int moves;

    public HighScoreEntry(int level, int moves)
    {
        this.level = level;
        this.moves = moves;
    }
    public override string ToString()
    {
        return $"#{level} : {moves} moves by {name}";
    }

    public static int HighScoreCompare(HighScoreEntry lhs, HighScoreEntry rhs)
    {
        return lhs == null ? rhs == null ? 0 : -1 : rhs == null ? 1 : lhs.level.CompareTo(rhs.level);
    }
}


