using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
///     Manages loading and saving HighScores
/// </summary>
public static class HighScoreManager
{
    #region Constatnts
    /// <summary>
    ///     Save file name
    /// </summary>
    private const string SaveFileName = "/highscores.json";

    #endregion


    #region Public Methods

    /// <summary>
    ///     Save the current HighScores to file.
    /// </summary>
    public static void SaveScores()
    {
        if (GameManager.Scores == null)
            return;
        File.WriteAllText(Application.persistentDataPath + SaveFileName, JsonUtility.ToJson(GameManager.Scores));
        Debug.Log(JsonUtility.ToJson(GameManager.Scores));

        Debug.Log("Game Saved");
    }

    /// <summary>
    ///     Load HighScores from file - if exists, or create new HighScores object.
    /// </summary>
    public static void LoadScores()
    {
        if (File.Exists(Application.persistentDataPath + SaveFileName))
        {
            var reader = new StreamReader(Application.persistentDataPath + SaveFileName);
            var json = reader.ReadToEnd();

            var scores = JsonUtility.FromJson<HighScores>(json);
            GameManager.Scores = scores;
            scores.entries.Sort(HighScoreEntry.HighScoreCompare);

            Debug.Log("Scores Loaded");
            Debug.Log(scores);
        }
        else
        {
            GameManager.Scores = new HighScores();
            GameManager.Scores.entries.Add(new HighScoreEntry(0, -1));
            Debug.Log("Scores Created");
        }
    }

    #endregion
}

/// <summary>
///     Serializable object that hold the high scores.
/// </summary>
[Serializable]
public class HighScores
{
    /// <summary>
    ///     Number of levels in last save
    /// </summary>
    public int levelCount = SceneManager.sceneCountInBuildSettings;
    /// <summary>
    ///     List of the scores themselves.
    /// </summary>
    public List<HighScoreEntry> entries = new List<HighScoreEntry>(SceneManager.sceneCountInBuildSettings);

    public override string ToString()
    {
        return $"{levelCount} levels saved:\n{string.Join("\n", entries)}";
    }
}

/// <summary>
///     Hold high score for a single level
/// </summary>
[Serializable]
public class HighScoreEntry
{
    /// <summary>
    ///     Level the score is for
    /// </summary>
    public int level;
    /// <summary>
    ///     Name of the high-scorer
    /// </summary>
    public string name;
    /// <summary>
    ///     Moves in the high score
    /// </summary>
    public int moves;

    /// <summary>
    ///     Constructor
    /// </summary>
    public HighScoreEntry(int level, int moves, string name = "")
    {
        this.level = level;
        this.moves = moves;
        this.name = name;
    }

    public override string ToString()
    {
        return $"# Best in level {level}: {moves} moves by {name}";
    }

    /// testing sorting the list
    public static int HighScoreCompare(HighScoreEntry lhs, HighScoreEntry rhs)
    {
        return lhs == null ? rhs == null ? 0 : -1 : rhs == null ? 1 : lhs.level.CompareTo(rhs.level);
    }
}