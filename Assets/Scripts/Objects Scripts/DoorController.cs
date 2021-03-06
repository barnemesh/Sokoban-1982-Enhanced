using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
///     Manage opening and closing doors in a Tilemap
/// </summary>
public class DoorController : MonoBehaviour
{
    #region Private Method

    /// <summary>
    ///     Change count of players on the doors when toggling the Trigger of the collider.
    /// </summary>
    private void HandleExitWhenDisablingTrigger()
    {
        GameManager.DoorCounter += _tilemapCollider.isTrigger ? _playerOnSelf : -_playerOnSelf;
    }

    #endregion

    #region Inspector

    [SerializeField]
    [Tooltip("Tile to be at when the target condition is met.")]
    private TileBase tileWhenActive;

    [SerializeField]
    [Tooltip("Tile to be at when the target condition isn't met.")]
    private TileBase tileWhenInactive;

    [SerializeField]
    [Tooltip("Should the tiles be trigger when target condition was met?")]
    private bool triggerStateWhenActive;

    #endregion


    #region Private Fields

    /// <summary>
    ///     Are the doors in this tilemap open?
    /// </summary>
    private bool _conditionMet;

    /// <summary>
    ///     This objects Tilemap component.
    /// </summary>
    private Tilemap _doorsTilemap;

    /// <summary>
    ///     Collider of this objects Tilemap.
    /// </summary>
    private TilemapCollider2D _tilemapCollider;

    /// <summary>
    /// </summary>
    private int _playerOnSelf;

    #endregion


    #region MonoBehaviour

    // Start is called before the first frame update
    private void Start()
    {
        _doorsTilemap = GetComponent<Tilemap>();
        _tilemapCollider = _doorsTilemap.GetComponent<TilemapCollider2D>();
        _conditionMet = _doorsTilemap.ContainsTile(tileWhenActive);
        _tilemapCollider.isTrigger = _conditionMet ? triggerStateWhenActive : !triggerStateWhenActive;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!_conditionMet && GameManager.TargetCounter == 0)
        {
            _conditionMet = true;
            _doorsTilemap.SwapTile(tileWhenInactive, tileWhenActive);
            _tilemapCollider.isTrigger = triggerStateWhenActive;
            _tilemapCollider.ProcessTilemapChanges();
            HandleExitWhenDisablingTrigger();
        }

        if (_conditionMet && GameManager.TargetCounter > 0)
        {
            _conditionMet = false;
            _doorsTilemap.SwapTile(tileWhenActive, tileWhenInactive);
            _tilemapCollider.isTrigger = !triggerStateWhenActive;
            _tilemapCollider.ProcessTilemapChanges();
            HandleExitWhenDisablingTrigger();
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.collider.CompareTag("Player")) _playerOnSelf--;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Player")) _playerOnSelf++;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // When an avatar reaches a door - mark that door as entered
        if (!other.CompareTag("Player"))
            return;
        GameManager.DoorCounter++;
        _playerOnSelf++;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // When an avatar leaves a door - mark that door as empty
        if (!other.CompareTag("Player"))
            return;
        GameManager.DoorCounter--;
        _playerOnSelf--;
    }

    #endregion
}