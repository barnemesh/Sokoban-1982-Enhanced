using System;
using UnityEngine;

/**
 * AvatarController avatar controller.
 */
public class PlayerControl : MonoBehaviour
{
    #region Enums

    public enum MovementDirection
    {
        Up,
        Down,
        Left,
        Right
    }

    #endregion


    #region Inspector

    [SerializeField]
    [Tooltip("How many updates should a single movement take")]
    private float updatesCountInMovement = 4.0f;

    // todo: Idle sprite to mark current active.
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private Rigidbody2D myRigidbody;

    #endregion


    #region Private Fields

    private Vector2 _targetDirection;
    private Vector2 _lastPosition;
    private bool _moving;
    private float _distancePercentage;
    private int _animatorHash;

    #endregion


    #region Public Methods

    public void ToggleIdle ()
    {
        animator.SetInteger(_animatorHash, -animator.GetInteger(_animatorHash));
    }

    public void SetMovement (MovementDirection direction)
    {
        if ( _moving )
            return;

        switch (direction)
        {
            case MovementDirection.Left:
                animator.SetInteger(_animatorHash, 1);
                _targetDirection = Vector2.left;
                break;
            case MovementDirection.Up:
                animator.SetInteger(_animatorHash, 2);
                _targetDirection = Vector2.up;
                break;
            case MovementDirection.Down:
                animator.SetInteger(_animatorHash, 3);
                _targetDirection = Vector2.down;
                break;
            case MovementDirection.Right:
                animator.SetInteger(_animatorHash, 4);
                _targetDirection = Vector2.right;
                break;
            default:
                return;
        }


        // if we need to move - check if we can move in the desired direction.

        var hit = Physics2D.Raycast(myRigidbody.position, _targetDirection, 1.0f);

        // if there is a box, check if it can be moved before moving.
        if ( hit.collider != null && hit.collider.CompareTag("Box") )
        {
            var boxControl = hit.collider.GetComponent<BoxControl>();
            if ( !boxControl.TryToMoveInDirection(_targetDirection) ) return;

            _moving = true;
        }

        if ( hit.collider == null ) _moving = true;

        // Count movements
        if ( _moving ) GameManager.MoveCounter++;
    }

    #endregion


    #region Monobehaviour

    private void Awake ()
    {
        _animatorHash = Animator.StringToHash("Direction");
        _lastPosition = myRigidbody.position;
        GameManager.PlayerList.Add(this); // register this player as active
    }

    private void FixedUpdate ()
    {
        if ( !_moving )
            return;

        // If we need to move, use exactly updatesCountInMovement to finish the entire movement.
        _distancePercentage += 1 / updatesCountInMovement;
        _distancePercentage = _distancePercentage >= 1 ? 1 : _distancePercentage;

        myRigidbody.MovePosition(_lastPosition + _distancePercentage * _targetDirection);

        if ( !(_distancePercentage >= 1) )
            return;

        _distancePercentage = 0;
        _lastPosition += _targetDirection;
        _moving = false;
    }

    private void OnTriggerEnter2D (Collider2D other)
    {
        // When a box reaches a target - mark that target as complete
        if ( other.CompareTag("Door") ) GameManager.DoorCounter++;
    }

    private void OnTriggerExit2D (Collider2D other)
    {
        // When a box leaves a target - mark that target as not complete
        if ( other.CompareTag("Door") ) GameManager.DoorCounter--;
    }

    #endregion
}