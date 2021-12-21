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
        Up, Down, Left, Right
    }

    #endregion
    
    #region Properties

    /**
     * Is movement Paused?
     */
    public bool Pause { get; set; }

    #endregion


    #region Inspector
    
    [SerializeField]
    [Tooltip("How many updates should a single movement take")]
    private float updatesCountInMovement = 4.0f;

    // todo: Idle sprite to mark current active.
    /// <summary>
    ///     Struct to keep the sprites - just to be able to create a list with named elements
    /// </summary>
    [Serializable]
    public struct Sprites
    {
        public Sprite up;
        public Sprite down;
        public Sprite left;
        public Sprite right;
    }

    [SerializeField]
    private Sprites sprites;

    [SerializeField]
    private SpriteRenderer mySpriteRenderer;

    [SerializeField]
    private Rigidbody2D myRigidbody;

    #endregion


    #region Private Fields

    private Vector2 _targetDirection;
    private Vector2 _lastPosition;
    private bool _moving;
    private float _distancePercentage;
    public Animator _animator;

    #endregion


    #region Public Methods

    public void ToggleIdle ()
    {
        _animator.SetTrigger("Idle");
    }

    public void SetMovement (MovementDirection direction)
    {
        if ( _moving )
            return;

        switch (direction)
        {
            case MovementDirection.Left: 
                mySpriteRenderer.sprite = sprites.left;
                _targetDirection = Vector2.left;
                break;
            case MovementDirection.Up:
                mySpriteRenderer.sprite = sprites.up;
                _targetDirection = Vector2.up;
                break;
            case MovementDirection.Down:
                mySpriteRenderer.sprite = sprites.down;
                _targetDirection = Vector2.down;
                break;
            case MovementDirection.Right:
                mySpriteRenderer.sprite = sprites.right;
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

    private void Start ()
    {
        _animator = GetComponent<Animator>();
        print(_animator.gameObject.transform.position);
        _lastPosition = myRigidbody.position;
        GameManager.PlayerList.Add(this);  // register this player as active
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

    #endregion
}