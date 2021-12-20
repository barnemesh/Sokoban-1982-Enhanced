using System.Collections.Generic;
using UnityEngine;

namespace Scriptable_Objects.Level_Datas
{
    /// <summary>
    /// Hold the data for the player and boxes position for a given level
    /// </summary>
    [CreateAssetMenu]
    public class LevelData : ScriptableObject
    {
        public Vector2 player;
        public List<Vector2> boxes;
    }
}