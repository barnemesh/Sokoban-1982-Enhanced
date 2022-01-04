using UnityEngine;

/**
 * Box movement controls.
 * If pushed - try and move in the direction pushed.
 * If there is nothing there - move.
 * If there is anything in the way - dont move.
 */
public class BoxControl : MonoBehaviour
{
    #region Private Methods

    private bool IsStuck()
    {
        if (_onTarget)
            return false;
        
        var hitUp = Physics2D.Raycast(myRigidbody.position, Vector2.up, 1.0f);
        var hitLeft = Physics2D.Raycast(myRigidbody.position, Vector2.left, 1.0f);
        var hitRight = Physics2D.Raycast(myRigidbody.position, Vector2.right, 1.0f);
        var hitDown = Physics2D.Raycast(myRigidbody.position, Vector2.down, 1.0f);

        var up = hitUp.collider != null && hitUp.collider.CompareTag("Wall");
        var left = hitLeft.collider != null && hitLeft.collider.CompareTag("Wall");
        var right = hitRight.collider != null && hitRight.collider.CompareTag("Wall");
        var down = hitDown.collider != null && hitDown.collider.CompareTag("Wall");

        return up && left || up && right || right && down || down && left;
    } 

    #endregion
    
    #region Public Methods

    /// <summary>
    ///     Check if the box can be moved in specified direction, and if it can, move it.
    /// </summary>
    /// <param name="direction">direction to move</param>
    /// <returns>true if moved, false o.w</returns>
    public bool TryToMoveInDirection(Vector2 direction)
    {
        if (_moving) 
            return false;

        var hit = Physics2D.Raycast(myRigidbody.position, direction, 1.0f);
        if (hit.collider != null) 
            return false;
        
        _targetDirection = direction;
        _moving = true;
        return true;
    }

    #endregion
    
    #region Inspector

    [SerializeField]
    [Tooltip("How many updates should a single movement take")]
    private float updatesCountInMovement = 4.0f;

    [SerializeField]
    [Tooltip("This objects rigidbody.")]
    private Rigidbody2D myRigidbody;

    #endregion
    
    #region Private Fields

    /// <summary>
    /// Target direction to move to.
    /// </summary>
    private Vector2 _targetDirection;

    /// <summary>
    /// Position before movement started
    /// </summary>
    private Vector2 _lastPosition;

    /// <summary>
    /// Is the avatar moving currently?
    /// </summary>
    private bool _moving;

    /// <summary>
    /// Percentage of movement complete.
    /// </summary>
    private float _distancePercentage;

    private bool _onTarget;

    #endregion

    #region Monobehaviour

    private void Start()
    {
        _lastPosition = myRigidbody.position;
        GameManager.TargetCounter++;
    }

    private void FixedUpdate()
    {
        if (!_moving)
            return;

        // If we need to move, use exactly updatesCountInMovement to finish the entire movement.
        _distancePercentage += 1 / updatesCountInMovement;
        _distancePercentage = _distancePercentage >= 1 ? 1 : _distancePercentage;

        myRigidbody.MovePosition(_lastPosition + _distancePercentage * _targetDirection);

        if (!(_distancePercentage >= 1))
            return;

        _distancePercentage = 0;
        _lastPosition += _targetDirection;
        _moving = false;
        if (IsStuck())
        {
            print($"Box stuck at position{myRigidbody.position}");
            GameManager.BoxIsStuck = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        _onTarget = true;
        // When a box reaches a target - mark that target as complete
        if (other.CompareTag("Target")) 
            GameManager.TargetCounter--;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        _onTarget = false;
        // When a box leaves a target - mark that target as not complete
        if (other.CompareTag("Target")) 
            GameManager.TargetCounter++;
    }

    #endregion
}