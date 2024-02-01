using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitManager : MonoBehaviour
{
    [SerializeField] private Sprite ATTENTION;
    [SerializeField] private Sprite ATTENTIONROUUUGE;
    
private void Update()
    {
        if (!Hero.Instance) return;
        for (int i = 0; i < MapManager.Instance.width; i++)
        {
            for (int j = 0; j < MapManager.Instance.height; j++)
            {
                if (!MapManager.Instance.mapArray[i, j].isExit) continue;
                if (MapManager.Instance.mapArray[i, j].hasDoorDown && j > 0 && !MapManager.Instance.mapArray[i, j - 1].isConnectedToPath)
                {
                    if (Hero.Instance.GetIndexHeroPos().x == i && Hero.Instance.GetIndexHeroPos().y == j)
                    {
                        MapManager.Instance.mapArray[i, j - 1].img.sprite = ATTENTIONROUUUGE;
                    }
                    else
                    {
                        MapManager.Instance.mapArray[i, j - 1].img.sprite = ATTENTION;
                    }
                }
                if (MapManager.Instance.mapArray[i, j].hasDoorUp && j < MapManager.Instance.height - 1 && !MapManager.Instance.mapArray[i, j + 1].isConnectedToPath)
                {
                    if (Hero.Instance.GetIndexHeroPos().x == i && Hero.Instance.GetIndexHeroPos().y == j)
                    {
                        MapManager.Instance.mapArray[i, j + 1].img.sprite = ATTENTIONROUUUGE;
                    }
                    else
                    {
                        MapManager.Instance.mapArray[i, j + 1].img.sprite = ATTENTION;
                    }
                }
                if (MapManager.Instance.mapArray[i, j].hasDoorLeft && i > 0 && !MapManager.Instance.mapArray[i - 1, j].isConnectedToPath)
                {
                    if (Hero.Instance.GetIndexHeroPos().x == i && Hero.Instance.GetIndexHeroPos().y == j)
                    {
                        MapManager.Instance.mapArray[i - 1, j].img.sprite = ATTENTIONROUUUGE;
                    }
                    else
                    {
                        MapManager.Instance.mapArray[i - 1, j].img.sprite = ATTENTION;
                    }
                }
                if (MapManager.Instance.mapArray[i, j].hasDoorRight && i < MapManager.Instance.width - 1 && !MapManager.Instance.mapArray[i + 1, j].isConnectedToPath)
                {
                    if (Hero.Instance.GetIndexHeroPos().x == i && Hero.Instance.GetIndexHeroPos().y == j)
                    {
                        MapManager.Instance.mapArray[i + 1, j].img.sprite = ATTENTIONROUUUGE;
                    }
                    else
                    {
                        MapManager.Instance.mapArray[i + 1, j].img.sprite = ATTENTION;
                    }
                }
            }
        }
    }
}
