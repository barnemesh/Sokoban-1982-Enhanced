using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelIndicator : MonoBehaviour
{
    #region Inspector

    [SerializeField]
    [Tooltip("")]
    private GameObject markerPrefab;

    [SerializeField]
    [Tooltip("")]
    private string mainMenu = "Main Menu";
    [SerializeField]
    [Tooltip("")]
    private string normalLevel = "Level {0}";


    #endregion

    #region Private Fields

    private readonly List<MarkerControl> _indicators = new List<MarkerControl>();
    
    private int _index;

    #endregion

    #region MonoBehaviour

    private void Start()
    {
        GameManager.SetLevel(SceneManager.sceneCountInBuildSettings - 1);

        _index = 0;
        var item = Instantiate(markerPrefab, transform);
        var ind = item.GetComponent<MarkerControl>();
        ind.Marked = true;
        ind.Text = mainMenu;
        _indicators.Add(ind);
        
        for (var i = 1; i < SceneManager.sceneCountInBuildSettings - 1; i++)
        {
            item = Instantiate(markerPrefab, transform);
            ind = item.GetComponent<MarkerControl>();
            ind.Marked = false;
            ind.Text = string.Format(normalLevel, i);
            _indicators.Add(ind);
        }
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Application.Quit();
            GameManager.SaveScores();
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameManager.SwitchToSceneByNumber(_index);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
            Indicate(_index - 1);
        if (Input.GetKeyDown(KeyCode.DownArrow))
            Indicate(_index + 1);
        
    }

    #endregion

    #region Private Methods

    private void Indicate(int i)
    {
        if (_indicators.Count == 0)
            return;

        _indicators[_index].Marked = false;
        _index = i % _indicators.Count;
        _indicators[_index].Marked = true;
    }

    #endregion
}