using UnityEngine;

/// <summary>
///     Controller for all avatars in the level.
/// </summary>
public class AvatarsControl : MonoBehaviour
{
    #region Inspector

    [SerializeField]
    [Tooltip("Panel that hold the indicators for active avatars.")]
    private IndicatorControl statePanel;

    #endregion

    #region Properties

    /**
     * Is movement Paused?
     */
    public bool Pause { get; set; }

    #endregion


    #region Private Fields

    /// <summary>
    ///     Current player index in the GameManager list.
    /// </summary>
    private int _currentPlayer;

    /// <summary>
    ///     Is the active avatar moving?
    /// </summary>
    private bool _moving;

    /// <summary>
    ///     Target direction to move to.
    /// </summary>
    private Vector2 _targetDirection;

    #endregion


    #region Monobehaviour

    private void Start()
    {
        GameManager.AvatarController = this;
        if (statePanel.isActiveAndEnabled)
        {
            statePanel.CreateAvatars();
            statePanel.Indicate(_currentPlayer);
        }

        GameManager.PlayerList[_currentPlayer].SetActiveAnimation(true);
    }


    // Update is called once per frame
    private void Update()
    {
        if (Pause || _moving)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameManager.PlayerList[_currentPlayer].SetActiveAnimation(false);
            _currentPlayer = (_currentPlayer + 1) % GameManager.PlayerList.Count;
            statePanel.Indicate(_currentPlayer);
            GameManager.PlayerList[_currentPlayer].SetActiveAnimation(true);
        }

        // Choose movement direction based on input, i not already moving.
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            GameManager.PlayerList[_currentPlayer].SetMovement(PlayerControl.MovementDirection.Left);

        if (Input.GetKeyDown(KeyCode.UpArrow))
            GameManager.PlayerList[_currentPlayer].SetMovement(PlayerControl.MovementDirection.Up);

        if (Input.GetKeyDown(KeyCode.RightArrow))
            GameManager.PlayerList[_currentPlayer].SetMovement(PlayerControl.MovementDirection.Right);

        if (Input.GetKeyDown(KeyCode.DownArrow))
            GameManager.PlayerList[_currentPlayer].SetMovement(PlayerControl.MovementDirection.Down);
    }

    #endregion
}