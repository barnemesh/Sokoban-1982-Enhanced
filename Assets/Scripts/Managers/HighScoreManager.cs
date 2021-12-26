using System.IO;
using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages loading and saving HighScores
/// </summary>
public static class HighScoreManager
{
    #region Constatnts

    private const string SaveFileName = "/highscores.json";

    #endregion


    #region Public Methods

    /// <summary>
    /// Save the current HighScores to file.
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
    /// Load HighScores from file - if exists, or create new HighScores object.
    /// </summary>
    public static void LoadScores()
    { 
        if (File.Exists(Application.persistentDataPath + SaveFileName))
        {
            StreamReader reader = new StreamReader(Application.persistentDataPath + SaveFileName);
            string json = reader.ReadToEnd();

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

[Serializable]
public class HighScores
{
    // todo: list size? cant use dict for some reason...
    public int levelCount = SceneManager.sceneCountInBuildSettings;
    public List<HighScoreEntry> entries = new List<HighScoreEntry>(SceneManager.sceneCountInBuildSettings);

    public override string ToString ()
    {
        return $"{levelCount} levels saved:\n{string.Join("\n", entries)}";
    }
}

[Serializable]
public class HighScoreEntry
{
    public int level;
    public string name;
    public int moves;

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

    public static int HighScoreCompare(HighScoreEntry lhs, HighScoreEntry rhs)
    {
        return lhs == null ? rhs == null ? 0 : -1 : rhs == null ? 1 : lhs.level.CompareTo(rhs.level);
    }
}