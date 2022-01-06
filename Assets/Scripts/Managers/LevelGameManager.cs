using Scriptable_Objects;
using TMPro;
using UnityEngine;

/// <summary>
///     Manager of a single level in the game.
/// </summary>
public class LevelGameManager : MonoBehaviour
{
    #region Private Fields

    /// <summary>
    ///     Is the game waiting for input?
    /// </summary>
    private bool _waitingForInput;

    #endregion

    #region Inspector

    [SerializeField]
    [Tooltip("UITexts objects that hold the texts and formats for this level.")]
    private UITexts texts;

    [SerializeField]
    [Tooltip("Text to show score")]
    private TextMeshProUGUI scoreText;


    [SerializeField]
    [Tooltip("Text to show any message to user.")]
    private TextMeshProUGUI messagesText;

    // TODO: Use scene number instead?
    [SerializeField]
    [Tooltip("This level number.")]
    private int levelNumber;

    #endregion

    #region Monobehaviour

    private void Start()
    {
        // Update GameManager with current level data
        GameManager.SetTexts(messagesText, scoreText, texts);
        GameManager.SetLevel(levelNumber);
    }

    private void Update()
    {
        // Use f1 to do stuff.
        if (!_waitingForInput && (Input.GetKeyDown(KeyCode.F1) || GameManager.LevelWon || GameManager.BoxIsStuck))
        {
            _waitingForInput = true;
            GameManager.TogglePlayerMovement();
            GameManager.ActivateText();
        }

        // if already waiting for input, check if there is input.
        if (!_waitingForInput)
            return;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            GameManager.SaveScores();
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.Y)) GameManager.SwitchToSceneByNumber(0);

        if (Input.GetKeyDown(KeyCode.Space))
            GameManager.SwitchToTargetScene();

        if (Input.GetKeyDown(KeyCode.N) && !(GameManager.LevelWon || GameManager.BoxIsStuck))
        {
            _waitingForInput = false;
            GameManager.TogglePlayerMovement();
            GameManager.DeactivateText();
        }
    }

    #endregion
}