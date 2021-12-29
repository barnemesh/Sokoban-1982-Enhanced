using System.Collections.Generic;
using Scriptable_Objects;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
///     Static Game Manager - data and changes UI and scenes as required
/// </summary>
public static class GameManager
{

    #region Private Variables
    
    private static int _resets;
    private static int _currentLevel;
    private static int _movesInLevel;
    private static int _inDoor;
    
    private static TextMeshProUGUI _scoreText;
    private static TextMeshProUGUI _messagesText;
    private static UITexts _uiTexts;
    private static HighScores _scores;

    /// <summary>
    ///     Maximum number of resets per level. Public to allow future improvements for levels with more or less resets,
    ///     or UI control.
    /// </summary>
    private const int MaxResets = 4;

    #endregion
    
    #region Properties

    /// <summary>
    /// Name the current player set for high-scores.
    /// </summary>
    public static string UserName { get; set; }

    /// <summary>
    /// The HighScores loaded for this play.
    /// </summary>
    public static HighScores Scores
    {
        get => _scores;
        set => _scores ??= value;
    }

    /// <summary>
    /// Indicate if the current level is complete.
    /// </summary>
    public static bool LevelWon { get; private set; }

    /// <summary>
    ///     Number of moves since level start. Did not exist in the original game. If there is a score text, updates it.
    /// </summary>
    public static int MoveCounter
    {
        get => _movesInLevel;
        set
        {
            if (LevelWon) return;
            _movesInLevel = value;
            UpdateScore();
        }
    }

    /// <summary>
    /// Count the number of doors that have avatars in them. When all avatars are in doors, the level is won.
    /// </summary>
    public static int DoorCounter
    {
        get => _inDoor;
        set
        {
            _inDoor = value;
            LevelWon = _inDoor == PlayerList.Count;
            Debug.Log($"{LevelWon},{_inDoor},{PlayerList.Count}");
            if (LevelWon && Scores != null)
            {
                // todo: find
                var i = Scores.entries.FindIndex(x => x.level == _currentLevel);
                if (i == -1)
                {
                    Scores.entries.Add(new HighScoreEntry(_currentLevel, MoveCounter, UserName));
                }
                else if (MoveCounter <= Scores.entries[i].moves)
                {
                    Scores.entries[i].moves = MoveCounter;
                    Scores.entries[i].name = UserName;
                }
            }
        }
    }

    /// <summary>
    ///     Counter for the number of targets that have no boxes on them in the current level.
    /// </summary>
    public static int TargetCounter { get; set; }

    /// <summary>
    ///     The AvatarController in the current level.
    /// </summary>
    public static AvatarsControl AvatarController { get; set; }

    /// <summary>
    /// List of all active Avatars in the level.
    /// </summary>
    public static List<PlayerControl> PlayerList { get; } = new List<PlayerControl>();

    #endregion

    #region Setter Methods

    /// <summary>
    ///     Set the UI texts used in the current level.
    /// </summary>
    /// <param name="messagesText"> Center of screen message</param>
    /// <param name="scoreText"> Score and resets text</param>
    /// <param name="uiTexts"> UITexts holding the texts and formats to use in this level.</param>
    public static void SetTexts(TextMeshProUGUI messagesText, TextMeshProUGUI scoreText, UITexts uiTexts)
    {
        _uiTexts = uiTexts;
        _messagesText = messagesText;
        _scoreText = scoreText;
    }

    /// <summary>
    ///     Toggle the current AvatarController ability to move, while UI is displayed.
    /// </summary>
    public static void TogglePlayerMovement()
    {
        if (AvatarController != null)
            AvatarController.Pause = !AvatarController.Pause;
    }

    /// <summary>
    ///     Update the current level the game manager manages, and number of resets in this level.
    /// </summary>
    /// <param name="levelNumber"> Number of new level.</param>
    public static void SetLevel(int levelNumber)
    {
        _resets = levelNumber == _currentLevel ? _resets + 1 : 0;
        _currentLevel = levelNumber;
        LevelWon = false;
        UpdateScore();
        
        if (Scores == null) 
            return;
        Debug.Log(Scores.entries.Find(x => x.level == _currentLevel));
    }

    #endregion

    #region Manager Methods

    /// <summary>
    ///     Load the next scene based on current level status.
    /// </summary>
    public static void SwitchToTargetScene()
    {
        DeactivateText();
        var targetScene = GetTargetScene();
        PlayerList.Clear();
        _inDoor = 0;
        TargetCounter = 0;
        _movesInLevel = 0;
        SceneManager.LoadScene(targetScene);
    }

    /// <summary>
    ///     Display UI text based on current level status.
    /// </summary>
    public static void ActivateText()
    {
        DeactivateText();
        if (DoorCounter == PlayerList.Count)
            _messagesText.text = _uiTexts.winText;
        else
            _messagesText.text = _resets >= MaxResets ? _uiTexts.loseText : _uiTexts.resetText;
    }

    /// <summary>
    ///     Remove the Text currently displayed.
    /// </summary>
    public static void DeactivateText()
    {
        if (_messagesText != null) 
            _messagesText.text = "";
    }

    /// <summary>
    /// Load scores from file.
    /// </summary>
    public static void LoadScores ()
    {
        HighScoreManager.LoadScores();
    }
    
    /// <summary>
    /// Save scores to file.
    /// </summary>
    public static void SaveScores ()
    {
        HighScoreManager.SaveScores();
    }
    
    #endregion
    
    #region Private Helper Methods

    /// <summary>
    ///     Get the number of the next scene to load. If level was won, get next level, else reload current scene.
    /// </summary>
    /// <returns> NUmber of scene to load</returns>
    private static int GetTargetScene()
    {
        if (DoorCounter == PlayerList.Count)
            return (_currentLevel + 1) % SceneManager.sceneCountInBuildSettings;
        return _resets >= MaxResets ? 0 : _currentLevel;
    }

    /// <summary>
    ///     If score text exists, update it with current values.
    ///     In the original game this did not exist.
    /// </summary>
    private static void UpdateScore()
    {
        if (_scoreText == null) 
            return;
        _scoreText.text = string.Format(_uiTexts.scoreFormat, _movesInLevel, MaxResets - _resets);
        if (Scores != null)
        {
            // todo: null check?
            _scoreText.text += "\t" + Scores.entries.Find(x => x.level == _currentLevel);
        }
    }

    #endregion
}