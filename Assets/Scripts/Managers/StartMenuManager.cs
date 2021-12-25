using System.IO;
using UnityEngine;

/**
 * Main menu runner.
 */
public class StartMenuManager : MonoBehaviour
{
    private void Start()
    {
        GameManager.SetLevel(0);
        LoadScores();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            GameManager.SwitchToTargetScene();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Application.Quit();
            GameManager.SaveScores();
        }
    }
    
    private void LoadScores()
    { 
        if (File.Exists(Application.persistentDataPath + "/highscores.json"))
        {
            StreamReader reader = new StreamReader(Application.persistentDataPath + "/highscores.json");
            string json = reader.ReadToEnd();

            var scores = JsonUtility.FromJson<HighScores>(json);
            GameManager.Scores = scores;
            scores.entries.Sort(HighScoreEntry.HighScoreCompare);
            
            Debug.Log("Scores Loaded");
            foreach (var highScoreEntry in scores.entries)
            {
                print(highScoreEntry);
            }
        }
        else
        {
            GameManager.Scores = new HighScores();
            GameManager.Scores.entries[0] = new HighScoreEntry(0, -1);
            Debug.Log("Scores Created");

        }
    }
}