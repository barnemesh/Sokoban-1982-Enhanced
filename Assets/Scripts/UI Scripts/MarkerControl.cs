using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///     Controller for UI item that marks levels in the level choosing scene
/// </summary>
public class MarkerControl : MonoBehaviour
{
    #region MonoBehaviour

    // Start is called before the first frame update
    private void Awake()
    {
        _text = GetComponentInChildren<TextMeshProUGUI>();
        _image = GetComponentInChildren<Image>();
    }

    #endregion

    #region Inspector

    [SerializeField]
    [Tooltip("Sprite to mark indicated marker")]
    private Sprite markedSprite;

    [SerializeField]
    [Tooltip("Sprite to mark non-indicated markers")]
    private Sprite unMarkedSprite;

    #endregion

    #region Private Fields

    /// <summary>
    ///     Text this items presents
    /// </summary>
    private TextMeshProUGUI _text;

    /// <summary>
    ///     Image indicating the current item
    /// </summary>
    private Image _image;

    /// <summary>
    ///     is this item currently marked?
    /// </summary>
    private bool _marked;

    #endregion

    #region Properties

    /// <summary>
    ///     Is this item currently marked?
    /// </summary>
    public bool Marked
    {
        set
        {
            _marked = value;
            _image.sprite = _marked ? markedSprite : unMarkedSprite;
        }
    }

    /// <summary>
    ///     The text this items shows.
    /// </summary>
    public string Text
    {
        set => _text.text = value;
    }

    #endregion
}