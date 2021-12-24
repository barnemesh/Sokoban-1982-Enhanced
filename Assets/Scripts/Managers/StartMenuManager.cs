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
            
            Debug.Log("Scores Loaded");
            for (var index = 0; index < scores.moves.Count; index++)
            {
                print(index + " : " + scores.moves[index]);
            }
        }
        else
        {
            GameManager.Scores = new HighScores();
            GameManager.Scores.moves[0] = -1;
            Debug.Log("Scores Created");

        }
    }
}