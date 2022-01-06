using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///     UNUSED IN FINAL SUBMIT.
///     Control a UI element that indicates current chosen avatar.
/// </summary>
public class IndicatorControl : MonoBehaviour
{
    #region Static Constants
    /// <summary>
    ///     Hash to activate the avatar images animator.
    /// </summary>
    private static readonly int Empty = Animator.StringToHash("Empty");

    #endregion

    #region Inspector

    [SerializeField]
    [Tooltip("Prefab of Avatar image for UI")]
    private GameObject avatarMarker;

    #endregion

    #region Private Fields

    /// <summary>
    ///     List of the avatar indicators in this level.
    /// </summary>
    private readonly List<GameObject> _indicators = new List<GameObject>();

    /// <summary>
    ///     Currently active avatar index.
    /// </summary>
    private int _index;

    #endregion

    #region Public Methods

    /// <summary>
    ///     Initialize avatars images
    /// </summary>
    public void CreateAvatars()
    {
        if (avatarMarker != null)
            for (var i = 0; i < GameManager.PlayerList.Count; i++)
            {
                var item = Instantiate(avatarMarker, transform);
                item.GetComponentInChildren<Animator>().SetBool(Empty, false);
                _indicators.Add(item);
            }

        _index = 0;
        if (_indicators.Count == 0)
            return;

        _indicators[0].GetComponent<Image>().enabled = true;
        _indicators[0].GetComponentInChildren<Animator>().SetBool(Empty, true);
    }

    /// <summary>
    ///     Switch active image to image #i
    /// </summary>
    /// <param name="i">index of the player to indicate</param>
    public void Indicate(int i)
    {
        if (_indicators.Count == 0)
            return;

        _indicators[_index].GetComponent<Image>().enabled = false;
        _indicators[_index].GetComponentInChildren<Animator>().SetBool(Empty, false);
        _index = i % _indicators.Count;
        _indicators[_index].GetComponent<Image>().enabled = true;
        _indicators[_index].GetComponentInChildren<Animator>().SetBool(Empty, true);
    }

    #endregion
}