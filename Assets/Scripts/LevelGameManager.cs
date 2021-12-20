using Scriptable_Objects.Level_Datas;
using TMPro;
using UnityEngine;

public class LevelGameManager : MonoBehaviour
{
    #region Inspector

    /// <summary>
    /// Text to show score. Did not exist in the original game.
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI scoreText;
    /// <summary>
    /// Text to display when the level is won.
    /// </summary>
    [SerializeField]
    private GameObject winText;
    /// <summary>
    /// Text to display when the level is ongoing.
    /// </summary>
    [SerializeField]
    private GameObject resetText;
    /// <summary>
    /// Text to display when last reset was reached.
    /// </summary>
    [SerializeField]
    private GameObject lostText;
    /// <summary>
    /// This level number.
    /// </summary>
    [SerializeField]
    private int levelNumber;
    /// <summary>
    /// Level Data for this level.
    /// </summary>
    [SerializeField]
    private LevelData levelData;
    /// <summary>
    /// Box prefab for the player to push.
    /// </summary>
    [SerializeField]
    private GameObject boxPrefab;
    /// <summary>
    /// Player prefab, the controlled avatar.
    /// </summary>
    [SerializeField]
    private GameObject playerPrefab;

    #endregion


    #region Private Fields

    private static int _resets;
    private bool _waitingForInput;

    #endregion


    #region Monobehaviour

    private void Start ()
    {
        // Update GameManager with current level data
        resetText.SetActive(false);
        winText.SetActive(false);
        lostText.SetActive(false);
        GameManager.SetTexts(winText, lostText, resetText);
        GameManager.ScoreText = scoreText; // todo refactor to texts
        GameManager.SetLevel(levelNumber);

        // Create player and Boxes
        // Instantiate(playerPrefab, levelData.player, Quaternion.identity);
        //
        // var box = new GameObject("Boxes");
        // foreach ( var boxPosition in levelData.boxes )
        //     Instantiate(boxPrefab, boxPosition, Quaternion.identity, box.transform);
    }

    private void Update ()
    {
        // Use f1 to do stuff.
        if ( !_waitingForInput && Input.GetKeyDown(KeyCode.F1) )
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

        if ( Input.GetKeyDown(KeyCode.N) )
        {
            _waitingForInput = false;
            GameManager.TogglePlayerMovement();
            GameManager.DeactivateText();
        }
    }

    #endregion
}