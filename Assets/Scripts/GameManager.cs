using System;
using System.Collections.Generic;
using System.Linq;
using Scriptable_Objects;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Tilemaps;

/// <summary>
/// Static Game Manager - data and changes UI and scenes as required
/// </summary>
public static class GameManager
{

    #region Private Variables
    
    private const string ScoreFormat = "Moves: {0,7}  Resets: {1,1}";

    private static int _resets;
    private static int _currentLevel;
    private static int _movesInLevel;
    private static int _inDoor;

    private static GameObject _winText;
    private static GameObject _lostText;
    private static GameObject _resetText;

    private static GameObject _activeText;
    private static int _currentPlayer;
    private static TextMeshProUGUI _scoreText;
    private static TextMeshProUGUI _messagesText;
    private static UITexts _uiTexts;

    #endregion


    #region Properties

    public static bool LevelWon { get; private set; }
    

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
            if(LevelWon) return;
            _movesInLevel = value;
            UpdateScore();
        } 
    }
    
    public static int DoorCounter
    {
        get => _inDoor;
        set
        {
            _inDoor = value;
            LevelWon = _inDoor == PlayerList.Count;
        } 
    }

    /// <summary>
    /// Counter for the number of targets that have no boxes on them in the current level.
    /// </summary>
    public static int TargetCounter { get; set; }

    /// <summary>
    /// The AvatarController in the current level.
    /// </summary>
    public static AvatarsControl AvatarController { get; set; }

    public static List<PlayerControl> PlayerList { get; } = new List<PlayerControl>();

    #endregion

    #region Setter Methods

    /// <summary>
    /// Set the UI texts used in the current level.
    /// </summary>
    /// <param name="messagesText"></param>
    /// <param name="scoreText"></param>
    public static void SetTexts (TextMeshProUGUI messagesText, TextMeshProUGUI scoreText, UITexts uiTexts)
    {
        DeactivateText();
        _uiTexts = uiTexts;
        _messagesText = messagesText;
        _scoreText = scoreText;
    }
    
    /// <summary>
    /// Toggle the current AvatarController ability to move, while UI is displayed.
    /// </summary>
    public static void TogglePlayerMovement ()
    {
        if ( AvatarController != null )
            AvatarController.Pause = !AvatarController.Pause;
    }

    /// <summary>
    /// Update the current level the game manager manages, and number of resets in this level.
    /// </summary>
    /// <param name="levelNumber"> Number of new level.</param>
    public static void SetLevel (int levelNumber)
    {
        _resets = levelNumber == _currentLevel ? _resets + 1 : 0;
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
        int targetScene = GetTargetScene();
        PlayerList.Clear();
        _inDoor = 0;
        TargetCounter = 0;
        _movesInLevel = 0;
        SceneManager.LoadScene(targetScene);
    }

    /// <summary>
    /// Display UI text based on current level status.
    /// </summary>
    public static void ActivateText ()
    {
        // todo: use GetTargetScene
        DeactivateText();
        if ( DoorCounter == PlayerList.Count )
            _messagesText.text = _uiTexts.winText;
        else
            _messagesText.text = _resets >= MaxResets ? _uiTexts.loseText : _uiTexts.resetText;
    }

    /// <summary>
    /// Remove the Text currently displayed.
    /// </summary>
    public static void DeactivateText ()
    {
        _messagesText.text = "";
    }

    #endregion


    #region Private Helper Methods

    /// <summary>
    /// Get the number of the next scene to load. If level was won, get next level, else reload current scene.
    /// </summary>
    /// <returns> NUmber of scene to load</returns>
    private static int GetTargetScene ()
    {
        Debug.Log(DoorCounter);
        Debug.Log(PlayerList.Count);
        if ( DoorCounter == PlayerList.Count )
            return (_currentLevel + 1) % SceneManager.sceneCountInBuildSettings;
        return _resets >= MaxResets ? 0 : _currentLevel;
    }

    /// <summary>
    /// If score text exists, update it with current values.
    /// In the original game this did not exist.
    /// </summary>
    private static void UpdateScore ()
    {
        if ( _scoreText == null ) return;
        _scoreText.text = string.Format(_uiTexts.scoreFormat, _movesInLevel, MaxResets - _resets);
    }
    #endregion
}