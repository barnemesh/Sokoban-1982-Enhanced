using TMPro;
using UnityEngine;

namespace Scriptable_Objects
{
    [CreateAssetMenu]
    public class UITexts : ScriptableObject
    {
        [TextArea]
        public string winText;
        [TextArea]
        public string loseText;
        [TextArea]
        public string resetText;
        [TextArea]
        public string scoreFormat;
    }
}
