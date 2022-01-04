using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MarkerControl : MonoBehaviour
{
    [SerializeField]
    [Tooltip("")]
    private Sprite markedSprite;
    [SerializeField]
    [Tooltip("")]
    private Sprite unMarkedSprite;
    
    private TextMeshProUGUI _text;
    private Image _image;
    private bool _marked;

    // Start is called before the first frame update
    void Awake()
    {
        _text = GetComponentInChildren<TextMeshProUGUI>();
        _image = GetComponentInChildren<Image>();
    }

    public bool Marked
    {
        set
        {
            _marked = value;
            _image.sprite = _marked ? markedSprite : unMarkedSprite;
        }
    }

    public string Text
    {
        set => _text.text = value;
    }
}
