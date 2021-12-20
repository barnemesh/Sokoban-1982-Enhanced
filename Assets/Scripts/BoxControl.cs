using UnityEngine;

/**
 * Box movement controls.
 * If pushed - try and move in the direction pushed.
 * If there is nothing there - move.
 * If there is anything in the way - dont move.
 */
public class BoxControl : MonoBehaviour
{
    #region Public Methods

    /// <summary>
    ///     Check if the box can be moved in specified direction, and if it can, move it.
    /// </summary>
    /// <param name="direction">direction to move</param>
    /// <returns>true if moved, false o.w</returns>
    public bool TryToMoveInDirection (Vector2 direction)
    {
        if ( _moving ) return false;

        var hit = Physics2D.Raycast(myRigidbody.position, direction, 1.0f);
        if ( hit.collider == null )
        {
            _targetDirection = direction;
            _moving = true;
            return true;
        }

        return false;
    }

    #endregion


    #region Inspector

    [SerializeField]
    [Tooltip("How many updates should a single movement take")]
    private float updatesCountInMovement = 4.0f;

    [SerializeField]
    private Rigidbody2D myRigidbody;

    #endregion


    #region Private Fields

    private Vector2 _targetDirection;
    private bool _moving;
    private Vector2 _lastPosition;
    private float _distancePercentage;

    #endregion


    #region Monobehaviour

    private void Start ()
    {
        _lastPosition = myRigidbody.position;
        GameManager.TargetCounter++;
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
        if ( other.CompareTag("Target") ) GameManager.TargetCounter--;
    }

    private void OnTriggerExit2D (Collider2D other)
    {
        // When a box leaves a target - mark that target as not complete
        if ( other.CompareTag("Target") ) GameManager.TargetCounter++;
    }

    #endregion
}