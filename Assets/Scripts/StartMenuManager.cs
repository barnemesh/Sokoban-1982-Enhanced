using UnityEngine;

/**
 * Main menu runner.
 */
public class StartMenuManager : MonoBehaviour
{
    private void Start ()
    {
        GameManager.SetLevel(0);
    }

    // Update is called once per frame
    private void Update ()
    {
        if ( Input.GetKeyDown(KeyCode.Space) ) GameManager.SwitchToTargetScene();

        if ( Input.GetKeyDown(KeyCode.Q) ) Application.Quit();
    }
}