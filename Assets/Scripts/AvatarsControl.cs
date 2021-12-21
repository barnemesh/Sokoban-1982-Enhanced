using System;
using System.Collections;
using System.Collections.Generic;
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

    
    #endregion


    #region Private Fields

    private int _currentPlayer;
    private bool _moving;
    private Vector2 _targetDirection;

    #endregion


    #region Monobehaviour

    private void Start ()
    {
        GameManager.AvatarController = this;
        GameManager.PlayerList[_currentPlayer].ToggleIdle();
    }


    // Update is called once per frame
    private void Update ()
    {
        if ( Pause || _moving ) return;

        if ( Input.GetKeyDown(KeyCode.Space) )
        {
            GameManager.PlayerList[_currentPlayer].ToggleIdle();
            _currentPlayer = (_currentPlayer + 1) % GameManager.PlayerList.Count;
            GameManager.PlayerList[_currentPlayer].ToggleIdle();
        }
        
        // Choose movement direction based on input, i not already moving.
        if ( Input.GetKeyDown(KeyCode.LeftArrow) )
        {
            GameManager.PlayerList[_currentPlayer].SetMovement(PlayerControl.MovementDirection.Left);
        }

        if ( Input.GetKeyDown(KeyCode.UpArrow) )
        {
            GameManager.PlayerList[_currentPlayer].SetMovement(PlayerControl.MovementDirection.Up);
        }

        if ( Input.GetKeyDown(KeyCode.RightArrow) )
        {
            GameManager.PlayerList[_currentPlayer].SetMovement(PlayerControl.MovementDirection.Right);
        }

        if ( Input.GetKeyDown(KeyCode.DownArrow) )
        {
            GameManager.PlayerList[_currentPlayer].SetMovement(PlayerControl.MovementDirection.Down);
        }
    }
    
    
    #endregion
}
