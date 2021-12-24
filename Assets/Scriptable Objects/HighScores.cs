using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


[Serializable]
public class HighScores
{
    // todo: list size? cant use dict for some reason...
    public int levelCount = SceneManager.sceneCountInBuildSettings;
    public List<int> moves = new List<int>(new int[SceneManager.sceneCountInBuildSettings]);
}

