using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * Main menu runner.
 */
public class StartMenuManager : MonoBehaviour
{
    private void Start()
    {
        GameManager.SetLevel(0);
        GameManager.LoadScores();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            GameManager.SwitchToSceneByNumber(SceneManager.sceneCountInBuildSettings - 1);

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Application.Quit();
            GameManager.SaveScores();
        }
    }

    public void GetName(string test)
    {
        GameManager.UserName = test.ToUpper();
        print(GameManager.UserName);
    }
    
}