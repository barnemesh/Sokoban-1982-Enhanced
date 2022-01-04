using UnityEngine;

namespace Scriptable_Objects
{
    /// <summary>
    ///     Hold texts and formats for levels.
    /// </summary>
    [CreateAssetMenu]
    public class UITexts : ScriptableObject
    {
        [TextArea]
        [Tooltip("Text to display when the level is won.")]
        public string winText;

        [TextArea]
        [Tooltip("Text to display when the level is lost.")]
        public string loseText;

        [TextArea]
        [Tooltip("Text to display when trying to reset level.")]
        public string resetText;

        [TextArea]
        [Tooltip("Text to display when a box is stuck.")]
        public string boxStuckText;

        [TextArea]
        [Tooltip("Format to use to display moves and resets. must have format space for moves and for resets.")]
        public string scoreFormat;
    }
}