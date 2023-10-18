using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugValuePath : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    private List<GameObject> textsValueForExploration = new();
    int sizeX;
    int sizeY;
    [SerializeField] private TMP_Text text;
    string textValue = "";
    // Start is called before the first frame update
    void Start()
    {
        //fill the text with the value of the valueForExploration
        sizeX = GameManager.Instance.SizeX;
        sizeY = GameManager.Instance.SizeY;
        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                textValue += 0 + " ";
            }
            textValue += "\n";
        }
        text.text = textValue;

    }

    // Update is called once per frame
    void Update()
    {
        textValue = "";
        for (int i = 0; i < sizeY; i++)
        {
            for (int j = 0; j <sizeX; j++)
            {
                textValue += GameManager.Instance.GetValueForExploration(j,i) + " ";
            }
            textValue += "\n";
        }
        
        text.text = textValue;
    }
}
