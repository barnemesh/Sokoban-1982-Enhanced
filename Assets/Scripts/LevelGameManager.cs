using Scriptable_Objects;
using Scriptable_Objects.Level_Datas;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelGameManager : MonoBehaviour
{
    #region Inspect

    [SerializeField]
    private UITexts texts;
    
    /// <summary>
    /// Text to show score. Did not exist in the original game.
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI scoreText;


    [SerializeField]
    private TextMeshProUGUI messagesText;
    /// <summary>
    /// This level number.
    /// </summary>
    [SerializeField]
    private int levelNumber;

    #endregion


    #region Private Fields

    private bool _waitingForInput;

    #endregion


    #region Monobehaviour

    private void Start ()
    {
        // Update GameManager with current level data
        GameManager.SetTexts(messagesText, scoreText, texts);
        GameManager.SetLevel(levelNumber);
        
    }

    private void Update ()
    {

        // Use f1 to do stuff.
        if ( !_waitingForInput && (Input.GetKeyDown(KeyCode.F1) || GameManager.LevelWon ))
        {
            _waitingForInput = true;
            GameManager.TogglePlayerMovement();
            GameManager.ActivateText();
        }

        // if already waiting for input, check if there is input.
        if ( !_waitingForInput ) return;
        if ( Input.GetKeyDown(KeyCode.Q) )
            Application.Quit();

        if ( Input.GetKeyDown(KeyCode.Y) )
            GameManager.SwitchToTargetScene();

        if ( Input.GetKeyDown(KeyCode.N) && !GameManager.LevelWon)
        {
            _waitingForInput = false;
            GameManager.TogglePlayerMovement();
            GameManager.DeactivateText();
        }
    }

    #endregion
}