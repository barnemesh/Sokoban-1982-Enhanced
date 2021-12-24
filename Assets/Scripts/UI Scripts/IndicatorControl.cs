using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IndicatorControl : MonoBehaviour
{
    
    #region Inspector

    [SerializeField]
    private GameObject avatarMarker;
    
    #endregion

    private readonly List<GameObject> _indicators = new List<GameObject>();
    private int _index;
    private int _empty;

    #region MonoBehaviour

    void Awake()
    {
        _empty = Animator.StringToHash("Empty");
    }
    

    #endregion

    public void CreateAvatars()
    {
        if (avatarMarker != null)
        {
            for (var i = 0; i < GameManager.PlayerList.Count; i++)
            {
                GameObject item = Instantiate(avatarMarker, transform);
                item.GetComponentInChildren<Animator>().SetBool(_empty, false);
                _indicators.Add(item);
            }
        }

        _index = 0;
        if (_indicators.Count > 0)
        {
            _indicators[0].GetComponent<Image>().enabled = true;
            _indicators[0].GetComponentInChildren<Animator>().SetBool(_empty, true);
        }
    }

    public void Indicate(int i)
    {
        if (_indicators.Count <= 0) 
            return;
        _indicators[_index].GetComponent<Image>().enabled = false;
        _indicators[_index].GetComponentInChildren<Animator>().SetBool(_empty, false);
        _index = i % _indicators.Count;
        _indicators[_index].GetComponent<Image>().enabled = true;
        _indicators[_index].GetComponentInChildren<Animator>().SetBool(_empty, true);
    }
    
    
}
