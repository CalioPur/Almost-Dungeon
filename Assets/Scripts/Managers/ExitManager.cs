using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitManager : MonoBehaviour
{
    [SerializeField] private Sprite ATTENTION;
    [SerializeField] private Sprite ATTENTIONROUUUGE;
    [SerializeField] private MapManager mapManager;
    
private void Update()
    {
        if (!Hero.Instance) return;
        for (int i = 0; i < Instance.width; i++)
        {
            for (int j = 0; j < Instance.height; j++)
            {
                if (!mapArray[i, j].isExit) continue;
                if (mapArray[i, j].hasDoorDown && j > 0 && !mapArray[i, j - 1].isConnectedToPath)
                {
                    if (Hero.Instance.GetIndexHeroPos().x == i && Hero.Instance.GetIndexHeroPos().y == j)
                    {
                        mapArray[i, j - 1].img.sprite = ATTENTIONROUUUGE;
                    }
                    else
                    {
                        mapArray[i, j - 1].img.sprite = ATTENTION;
                    }
                }
                if (mapArray[i, j].hasDoorUp && j < height - 1 && !mapArray[i, j + 1].isConnectedToPath)
                {
                    if (Hero.Instance.GetIndexHeroPos().x == i && Hero.Instance.GetIndexHeroPos().y == j)
                    {
                        mapArray[i, j + 1].img.sprite = ATTENTIONROUUUGE;
                    }
                    else
                    {
                        mapArray[i, j + 1].img.sprite = ATTENTION;
                    }
                }
                if (mapArray[i, j].hasDoorLeft && i > 0 && !mapArray[i - 1, j].isConnectedToPath)
                {
                    if (Hero.Instance.GetIndexHeroPos().x == i && Hero.Instance.GetIndexHeroPos().y == j)
                    {
                        mapArray[i - 1, j].img.sprite = ATTENTIONROUUUGE;
                    }
                    else
                    {
                        mapArray[i - 1, j].img.sprite = ATTENTION;
                    }
                }
                if (mapArray[i, j].hasDoorRight && i < width - 1 && !mapArray[i + 1, j].isConnectedToPath)
                {
                    if (Hero.Instance.GetIndexHeroPos().x == i && Hero.Instance.GetIndexHeroPos().y == j)
                    {
                        mapArray[i + 1, j].img.sprite = ATTENTIONROUUUGE;
                    }
                    else
                    {
                        mapArray[i + 1, j].img.sprite = ATTENTION;
                    }
                }
            }
        }
    }
}
