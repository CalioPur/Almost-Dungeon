using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPrefab : MonoBehaviour
{
    [field:SerializeField] public Image Image { get; private set; }
    [field:SerializeField] public Button Btn { get; private set; }
    [field:SerializeField] public TMP_Text text { get; private set; }
}