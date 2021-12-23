using UnityEngine;
using UnityEngine.Tilemaps;

public class DoorController : MonoBehaviour
{
    [SerializeField]
    private TileBase openDoor;

    [SerializeField]
    private TileBase closedDoor;

    private bool _doorsOpen;
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
            print("here");
            _doorsOpen = false;
            _doorsTilemap.SwapTile(openDoor, closedDoor);
            _doorsTilemap.GetComponent<TilemapCollider2D>().isTrigger = false;
        }
    }
}