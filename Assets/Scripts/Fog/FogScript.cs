using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FogScript : MonoBehaviour
{
    [SerializeField] private Sprite fogExit1;
    [SerializeField] private Sprite fogExitOpposite;
    [SerializeField] private Sprite fogExitCorner;
    [SerializeField] private Sprite fogExitTShape;
    [SerializeField] private Sprite fogExit4Ways;
    [SerializeField] private Sprite fogFull;

    private GameObject fogParent;

    private TileData[,] mapArray;
    private int[,] fogArray;
    // Start is called before the first frame update
    void OnEnable()
    {
        PlayerCardController.OnTilePosedEvent += ActualizeFog;
    }

    private void OnDisable()
    {
        PlayerCardController.OnTilePosedEvent -= ActualizeFog;
    }

     private void ActualizeFog(TileData arg1, CardInfoInstance arg2)
    {
        ActualizeFog();
    }
    
    private void ActualizeFog()
    {
        CreateFogParentIfNone();
        mapArray = MapManager.Instance.mapArray;
        DeleteChildrenOfFogParent();
        FillFogArray();
        for (int i = 0; i < mapArray.GetLength(0); i++)
        {
            for (int j = 0; j < mapArray.GetLength(1); j++)
            {
                if (fogArray[i, j] == -1) continue;
                SetExitFog(i, j);
            }
        }
    }

    private void SetExitFog(int i, int j)
    {
        bool[] directionOfExit = FindDirectionOfExit(i, j);
        switch (fogArray[i, j])
        {
            case 0:
                CreateFogFull(i, j);
                break;
            case 1 when directionOfExit[0]:
                CreateFogExit1(i, j, 0);
                break;
            case 1 when directionOfExit[1]:
                CreateFogExit1(i, j, 1);
                break;
            case 1 when directionOfExit[2]:
                CreateFogExit1(i, j, 2);
                break;
            case 1 when directionOfExit[3]:
                CreateFogExit1(i, j, 3);
                break;
            case 2 when (directionOfExit[0] && directionOfExit[1]):
                CreateFogExitCorner(i, j, 0);
                break;
            case 2 when (directionOfExit[1] && directionOfExit[2]):
                CreateFogExitCorner(i, j, 1);
                break;
            case 2 when (directionOfExit[2] && directionOfExit[3]):
                CreateFogExitCorner(i, j, 2);
                break;
            case 2 when (directionOfExit[3] && directionOfExit[0]):
                CreateFogExitCorner(i, j, 3);
                break;
            case 2 when (directionOfExit[0] && directionOfExit[2]):
                CreateFogExitOpposite(i, j, 0);
                break;
            case 2:
            {
                if (directionOfExit[1] && directionOfExit[3]) CreateFogExitOpposite(i, j, 1);
                break;
            }
            case 3 when (directionOfExit[0] && directionOfExit[1] && directionOfExit[2]):
                CreateFogExitTShape(i, j, 1);
                break;
            case 3 when (directionOfExit[1] && directionOfExit[2] && directionOfExit[3]):
                CreateFogExitTShape(i, j, 2);
                break;
            case 3 when (directionOfExit[2] && directionOfExit[3] && directionOfExit[0]):
                CreateFogExitTShape(i, j, 3);
                break;
            case 3 when (directionOfExit[3] && directionOfExit[0] && directionOfExit[1]):
                CreateFogExitTShape(i, j, 0);
                break;
            case 4:
                CreateFogExit4Ways(i, j);
                break;
        }
    }

    private void CreateFogFull(int i, int i1)
    {
        GameObject fogExit = new GameObject("FogExit")
        {
            transform =
            {
                parent = fogParent.transform,
                position = new Vector3(i - MapManager.Instance.width/2 + 1, 0.1f, i1 - MapManager.Instance.height/2 + 1),
                rotation = Quaternion.Euler(90, 0, 0),
                localScale = new Vector3(0.2f, 0.2f, 0.2f)
            }
        };
        SpriteRenderer spriteRenderer = fogExit.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = fogFull;
    }

    private void CreateFogExit4Ways(int i, int j)
    {
        GameObject fogExit = new GameObject("FogExit")
        {
            transform =
            {
                parent = fogParent.transform,
                position = new Vector3(i - MapManager.Instance.width/2 + 1, 0.1f, j - MapManager.Instance.height/2 + 1),
                rotation = Quaternion.Euler(90, 0, 0),
                localScale = new Vector3(0.2f, 0.2f, 0.2f)
            }
        };
        SpriteRenderer spriteRenderer = fogExit.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = fogExit4Ways;
    }

    private void CreateFogExitTShape(int i, int j, int p2)
    {
        GameObject fogExit = new GameObject("FogExit")
        {
            transform =
            {
                parent = fogParent.transform,
                position = new Vector3(i - MapManager.Instance.width/2 + 1, 0.1f, j - MapManager.Instance.height/2 + 1),
                rotation = Quaternion.Euler(90, 0, 90 * p2),
                localScale = new Vector3(0.2f, 0.2f, 0.2f)
            }
        };
        SpriteRenderer spriteRenderer = fogExit.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = fogExitTShape;
    }

    private void CreateFogExitOpposite(int i, int j, int p2)
    {
        GameObject fogExit = new GameObject("FogExit")
        {
            transform =
            {
                parent = fogParent.transform,
                position = new Vector3(i - MapManager.Instance.width/2 + 1, 0.1f, j - MapManager.Instance.height/2 + 1),
                rotation = Quaternion.Euler(90, 0, 90 * p2),
                localScale = new Vector3(0.2f, 0.2f, 0.2f)
            }
        };
        SpriteRenderer spriteRenderer = fogExit.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = fogExitOpposite;
    }

    private void CreateFogExitCorner(int i, int j, int p2)
    {
        GameObject fogExit = new GameObject("FogExit")
        {
            transform =
            {
                parent = fogParent.transform,
                position = new Vector3(i - MapManager.Instance.width/2 + 1, 0.1f, j - MapManager.Instance.height/2 + 1),
                rotation = Quaternion.Euler(90, 0, 90 * p2),
                localScale = new Vector3(0.2f, 0.2f, 0.2f)
            }
        };
        SpriteRenderer spriteRenderer = fogExit.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = fogExitCorner;
    }

    private void CreateFogExit1(int i, int j, int p2)
    {
        GameObject fogExit = new GameObject("FogExit")
        {
            transform =
            {
                parent = fogParent.transform,
                position = new Vector3(i - MapManager.Instance.width/2 + 1, 0.1f, j - MapManager.Instance.height/2 + 1),
                rotation = Quaternion.Euler(90, 0, 90 * p2),
                localScale = new Vector3(0.2f, 0.2f, 0.2f)
            }
        };
        SpriteRenderer spriteRenderer = fogExit.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = fogExit1;
    }

    private void DeleteChildrenOfFogParent()
    {
        foreach (Transform child in fogParent.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void FillFogArray()
    {
        fogArray = new int[mapArray.GetLength(0), mapArray.GetLength(1)];
        for (int i = 0; i < mapArray.GetLength(0); i++)
        {
            for (int j = 0; j < mapArray.GetLength(1); j++)
            {
                if (MapManager.Instance.mapArray[i, j].PiecePlaced)
                {
                    fogArray[i, j] = -1;
                    continue;
                }
                fogArray[i, j] = CheckNumberOfDoorsAround(i, j);
            }
        }
    }

    private int CheckNumberOfDoorsAround(int i, int j)
    {
        bool[] directions = new bool[4];
        directions = FindDirectionOfExit(i, j);
        return directions.Count(t => t);
    }
    private bool[] FindDirectionOfExit(int i, int j)
    {
        bool[] directions = new bool[4];
        if (i != 0) if (mapArray[i - 1, j].hasDoorRight) directions[0] = true;
        if (j != 0) if (mapArray[i, j - 1].hasDoorUp) directions[1] = true;
        if (i != MapManager.Instance.width - 3) if (mapArray[i + 1, j].hasDoorLeft) directions[2] = true;
        if (j != MapManager.Instance.height - 3) if (mapArray[i, j + 1].hasDoorDown) directions[3] = true;
        return directions;
    }
    private void CreateFogParentIfNone()
    {
        if (fogParent == null)
        {
            fogParent = new GameObject("FogParent");
            fogParent.SetActive(false);
        }
    }

}
