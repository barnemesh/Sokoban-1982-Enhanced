using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
///     Manager for the level chooser scene
/// </summary>
public class LevelIndicator : MonoBehaviour
{
    #region Private Methods

    /// <summary>
    ///     Indicate new item
    /// </summary>
    /// <param name="i"> index of new item to indicate</param>
    private void Indicate(int i)
    {
        if (_indicators.Count == 0)
            return;

        _indicators[_index].Marked = false;
        _index = i % _indicators.Count;
        _indicators[_index].Marked = true;
    }

    #endregion

    #region Inspector

    [SerializeField]
    [Tooltip("Prefab for the items in the list")]
    private GameObject markerPrefab;

    [SerializeField]
    [Tooltip("Text in the item for the Main Menu")]
    private string mainMenu = "Main Menu";

    [SerializeField]
    [Tooltip("Format for the text in the item for each level")]
    private string normalLevel = "Level {0}";

    #endregion

    #region Private Fields

    /// <summary>
    ///     List of all indicators
    /// </summary>
    private readonly List<MarkerControl> _indicators = new List<MarkerControl>();

    /// <summary>
    ///     Currently chosen item
    /// </summary>
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
            GameManager.SaveScores();
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.Space)) GameManager.SwitchToSceneByNumber(_index);

        if (Input.GetKeyDown(KeyCode.UpArrow))
            Indicate(_index - 1);
        if (Input.GetKeyDown(KeyCode.DownArrow))
            Indicate(_index + 1);
    }

    #endregion
}