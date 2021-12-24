using UnityEngine;

public class AvatarsControl : MonoBehaviour
{
    #region Properties

    /**
     * Is movement Paused?
     */
    public bool Pause { get; set; }

    #endregion


    #region Inspector

    [SerializeField]
    private IndicatorControl statePanel;

    #endregion


    #region Private Fields

    public int CurrentPlayer { get; private set; }
    private bool _moving;
    private Vector2 _targetDirection;

    #endregion


    #region Monobehaviour

    private void Start()
    {
        GameManager.AvatarController = this;

        statePanel.CreateAvatars();
        statePanel.Indicate(CurrentPlayer);
        
        GameManager.PlayerList[CurrentPlayer].SetActiveAnimation(true);
    }


    // Update is called once per frame
    private void Update()
    {
        if (Pause || _moving) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameManager.PlayerList[CurrentPlayer].SetActiveAnimation(false);
            CurrentPlayer = (CurrentPlayer + 1) % GameManager.PlayerList.Count;
            statePanel.Indicate(CurrentPlayer);
            GameManager.PlayerList[CurrentPlayer].SetActiveAnimation(true);
        }

        // Choose movement direction based on input, i not already moving.
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            GameManager.PlayerList[CurrentPlayer].SetMovement(PlayerControl.MovementDirection.Left);

        if (Input.GetKeyDown(KeyCode.UpArrow))
            GameManager.PlayerList[CurrentPlayer].SetMovement(PlayerControl.MovementDirection.Up);

        if (Input.GetKeyDown(KeyCode.RightArrow))
            GameManager.PlayerList[CurrentPlayer].SetMovement(PlayerControl.MovementDirection.Right);

        if (Input.GetKeyDown(KeyCode.DownArrow))
            GameManager.PlayerList[CurrentPlayer].SetMovement(PlayerControl.MovementDirection.Down);
    }

    #endregion
}