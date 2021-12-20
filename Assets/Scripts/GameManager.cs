using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// Static Game Manager - data and changes UI and scenes as required
/// </summary>
public static class GameManager
{
    #region Private Variables

    private const string ScoreFormat = "Moves: {0}  \t \t Resets: {1}";
    private static bool _levelWon;

    private static int _resets;
    private static int _currentLevel;
    private static int _targetCounter;
    private static int _movesInLevel;

    private static GameObject _winText;
    private static GameObject _lostText;
    private static GameObject _resetText;

    private static GameObject _activeText;
    private static PlayerControl _playerControl;
    
    #endregion


    #region Properties
    
    /// <summary>
    /// If Exists - Text to show move count and reset count.
    /// Does not exist in the original game - but is here to show possibilities.
    /// </summary>
    public static TextMeshProUGUI ScoreText { get; set; }

    /// <summary>
    /// Maximum number of resets per level. Public to allow future improvements for levels with more or less resets,
    /// or UI control.
    /// </summary>
    public static int MaxResets { get; } = 4;

    /// <summary>
    /// Number of moves since level start. Did not exist in the original game. If there is a score text, updates it.
    /// </summary>
    public static int MoveCounter
    {
        get => _movesInLevel;
        set
        {
            if (_levelWon) return;
            _movesInLevel = value;
            UpdateScore();
        } 
    }

    /// <summary>
    /// Counter for the number of targets that have no boxes on them in the current level.
    /// </summary>
    public static int TargetCounter
    {
        get => _targetCounter;
        set
        {
            _targetCounter = value;
            _levelWon = _targetCounter == 0;
        }
    }

    /// <summary>
    /// The Player in the current level.
    /// </summary>
    public static PlayerControl Player
    {
        get => _playerControl;
        set
        {
            _playerControl = value;
            _levelWon = false;
            MoveCounter = 0;
        }
    }
    
    #endregion


    #region Setter Methods

    /// <summary>
    /// Set the UI texts used in the current level.
    /// </summary>
    /// <param name="winText"> Text to display after level was won.</param>
    /// <param name="lostText"> Text to display when reached max resets for the level.</param>
    /// <param name="resetText"> Text to display when the level isn't won yet, and max resets wasn't reached.</param>
    public static void SetTexts (GameObject winText, GameObject lostText, GameObject resetText)
    {
        DeactivateText();
        _winText = winText;
        _lostText = lostText;
        _resetText = resetText;
        // todo: set score texts
    }
    
    /// <summary>
    /// Toggle the current Player ability to move, while UI is displayed.
    /// </summary>
    public static void TogglePlayerMovement ()
    {
        if ( Player != null )
            Player.Pause = !Player.Pause;
    }

    /// <summary>
    /// Update the current level the game manager manages, and number of resets in this level.
    /// </summary>
    /// <param name="levelNumber"> Number of new level.</param>
    public static void SetLevel (int levelNumber)
    {
        _resets = levelNumber == _currentLevel ? _resets + 1 : 0;
        TargetCounter = 0;
        _levelWon = false;
        _currentLevel = levelNumber;
        UpdateScore(); // todo: refactor player resets and this?
    }

    #endregion


    #region Manager Methods

    /// <summary>
    /// Load the next scene based on current level status.
    /// </summary>
    public static void SwitchToTargetScene ()
    {
        DeactivateText();
        SceneManager.LoadScene(GetTargetScene());
    }

    /// <summary>
    /// Display UI text based on current level status.
    /// </summary>
    public static void ActivateText ()
    {
        DeactivateText();
        _activeText = TargetCounter switch
        {
            0 => _winText,
            _ => _resets >= MaxResets ? _lostText : _resetText
        };

        _activeText.SetActive(true);
    }

    /// <summary>
    /// Remove the Text currently displayed.
    /// </summary>
    public static void DeactivateText ()
    {
        if ( _activeText == null )
            return;

        _activeText.SetActive(false);
        _activeText = null;
    }

    #endregion


    #region Private Helper Methods

    /// <summary>
    /// Get the number of the next scene to load. If level was won, get next level, else reload current scene.
    /// </summary>
    /// <returns> NUmber of scene to load</returns>
    private static int GetTargetScene ()
    {
        return TargetCounter switch
        {
            0 => (_currentLevel + 1) % SceneManager.sceneCountInBuildSettings,
            _ => _resets >= MaxResets ? 0 : _currentLevel
        };
    }

    /// <summary>
    /// If score text exists, update it with current values.
    /// In the original game this did not exist.
    /// </summary>
    private static void UpdateScore ()
    {
        if ( ScoreText == null ) return;
        ScoreText.text = string.Format(ScoreFormat, _movesInLevel, MaxResets - _resets);
    }
    #endregion
}