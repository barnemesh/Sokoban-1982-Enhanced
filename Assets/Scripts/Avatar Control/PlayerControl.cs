using UnityEngine;

/**
 * AvatarController avatar controller.
 */
public class PlayerControl : MonoBehaviour
{
    #region Static Constants

    /// <summary>
    /// Hash of the animators integer condition for movement directions.
    /// </summary>
    private static readonly int DirectionHash = Animator.StringToHash("Direction");

    #endregion
    
    #region Enums

    /// <summary>
    /// enum to indicate movement direction.
    /// </summary>
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

    [SerializeField]
    [Tooltip("This objects animator")]
    private Animator animator;

    [SerializeField]
    [Tooltip("This objects rigidbody")]
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

    #endregion


    #region Public Methods

    /// <summary>
    /// Set the Idle animation, indicating this avatar is active.
    /// </summary>
    /// <param name="state"> the desired state</param>
    public void SetActiveAnimation(bool state)
    {
        animator.SetInteger(DirectionHash, state ? -4 : 4);
    }

    /// <summary>
    /// Start moving in the given direction, if possible.
    /// </summary>
    /// <param name="direction"></param>
    public bool SetMovement(MovementDirection direction)
    {
        if (_moving)
            return false;

        switch (direction)
        {
            case MovementDirection.Left:
                animator.SetInteger(DirectionHash, 1);
                _targetDirection = Vector2.left;
                break;
            case MovementDirection.Up:
                animator.SetInteger(DirectionHash, 2);
                _targetDirection = Vector2.up;
                break;
            case MovementDirection.Down:
                animator.SetInteger(DirectionHash, 3);
                _targetDirection = Vector2.down;
                break;
            case MovementDirection.Right:
                animator.SetInteger(DirectionHash, 4);
                _targetDirection = Vector2.right;
                break;
            default:
                return false;
        }


        // if we need to move - check if we can move in the desired direction.

        var hit = Physics2D.Raycast(myRigidbody.position, _targetDirection, 1.0f);

        // if there is a box, check if it can be moved before moving.
        if (hit.collider != null && hit.collider.CompareTag("Box"))
        {
            var boxControl = hit.collider.GetComponent<BoxControl>();
            if (!boxControl.TryToMoveInDirection(_targetDirection))
                return false;

            _moving = true;
        }

        if (hit.collider == null)
            _moving = true;

        // Count movements
        if (_moving)
            GameManager.MoveCounter++;
        return _moving;
    }

    #endregion


    #region Monobehaviour

    private void Awake()
    {
        _lastPosition = myRigidbody.position;
        GameManager.PlayerList.Add(this); // register this player as active
        SetActiveAnimation(false);
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
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // When an avatar reaches a door - mark that door as entered
        if (other.CompareTag("Door"))
            GameManager.DoorCounter++;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // When an avatar leaves a door - mark that door as empty
        if (other.CompareTag("Door"))
            GameManager.DoorCounter--;
    }

    #endregion
}