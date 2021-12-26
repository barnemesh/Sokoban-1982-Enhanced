using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Manage opening and closing doors in a Tilemap
/// </summary>
public class DoorController : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Tile of open door")]
    private TileBase openDoor;

    [SerializeField]
    [Tooltip("Tile of closed door")]
    private TileBase closedDoor;

    /// <summary>
    /// Are the doors in this tilemap open?
    /// </summary>
    private bool _doorsOpen;
    /// <summary>
    /// This objects Tilemap component.
    /// </summary>
    private Tilemap _doorsTilemap;

    // Start is called before the first frame update
    private void Start()
    {
        _doorsTilemap = GetComponent<Tilemap>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (!_doorsOpen && GameManager.TargetCounter == 0)
        {
            _doorsOpen = true;
            _doorsTilemap.SwapTile(closedDoor, openDoor);
            _doorsTilemap.GetComponent<TilemapCollider2D>().isTrigger = true;
        }

        if (_doorsOpen && GameManager.TargetCounter > 0)
        {
            _doorsOpen = false;
            _doorsTilemap.SwapTile(openDoor, closedDoor);
            _doorsTilemap.GetComponent<TilemapCollider2D>().isTrigger = false;
        }
    }
}