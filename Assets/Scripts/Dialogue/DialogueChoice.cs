using LogicUI.FancyTextRendering;
using UnityEngine;
using UnityEngine.UI;

namespace Dialogue
{
    public class DialogueChoice : MonoBehaviour
    {
        [field:SerializeField] public MarkdownRenderer markdownManager { get; private set; }
        [field:SerializeField] public Button Btn { get; private set; }
    }
}