using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapManagerTools
{
    private MapManager _mapManager;

    public MapManagerTools(MapManager mapManager)
    {
        _mapManager = mapManager;
    }

    public void CheckAllTilesTypeAndRotation()
    {
        for (int i = 0; i < _mapManager.width - 2; i++)
        {
            for (int j = 0; j < _mapManager.height - 2; j++)
            {
                CheckTileTypeAndRotation(_mapManager.mapArray[i, j]);
            }
        }

        SetConnectedToPath();
        SetExits();
    }

    private void CheckTileTypeAndRotation(TileData tileData)
    {
        if (tileData.img.sprite.name == "EnterDungeon") return;
        if (isLTile(tileData))
        {
            tileData.img.sprite = !tileData.isRoom
                ? _mapManager._sprites.First(x => x.name == "LWay")
                : _mapManager._sprites.First(x => x.name == "LRoom");
            tileData.transform.rotation = Quaternion.Euler(90, GetRotationFromLTile(tileData), 0);
        }
        else if (isStraightTile(tileData))
        {
            if (tileData.isRoom)
            {
                tileData.img.sprite = _mapManager._sprites.First(x => x.name == "SimpleRoom");
            }
            else
            {
                tileData.img.sprite = _mapManager._sprites.First(x => x.name == "SimpleWay");
            }
            tileData.transform.rotation = Quaternion.Euler(90, GetRotationFromStraightTile(tileData), 0);
        }
        else if (isTTile(tileData))
        {
            tileData.img.sprite = !tileData.isRoom
                ? _mapManager._sprites.First(x => x.name == "TWay")
                : _mapManager._sprites.First(x => x.name == "TRoom");
            tileData.transform.rotation = Quaternion.Euler(90, GetRotationFromTTile(tileData), 0);
        }
        else if (isCrossTile(tileData))
        {
            tileData.img.sprite = !tileData.isRoom
                ? _mapManager._sprites.First(x => x.name == "XWay")
                : _mapManager._sprites.First(x => x.name == "XRoom");
        }
        else if (isDeadEndTile(tileData))
        {
            tileData.img.sprite = _mapManager._sprites.First(x => x.name == "Sas");
            tileData.transform.rotation = Quaternion.Euler(90, GetRotationFromDeadEndTile(tileData), 0);
        }
    }

    private bool isLTile(TileData data)
    {
        return data.hasDoorDown && data.hasDoorLeft && !data.hasDoorRight && !data.hasDoorUp ||
               data.hasDoorDown && !data.hasDoorLeft && data.hasDoorRight && !data.hasDoorUp ||
               !data.hasDoorDown && data.hasDoorLeft && !data.hasDoorRight && data.hasDoorUp ||
               !data.hasDoorDown && !data.hasDoorLeft && data.hasDoorRight && data.hasDoorUp;
    }

    private float GetRotationFromLTile(TileData data)
    {
        return data.hasDoorDown switch
        {
            true when data.hasDoorLeft && !data.hasDoorRight && !data.hasDoorUp => 0,
            true when !data.hasDoorLeft && data.hasDoorRight && !data.hasDoorUp => 270,
            false when !data.hasDoorLeft && data.hasDoorRight && data.hasDoorUp => 180,
            false when data.hasDoorLeft && !data.hasDoorRight && data.hasDoorUp => 90,
            _ => 0
        };
    }

    private bool isStraightTile(TileData data)
    {
        return data.hasDoorDown && !data.hasDoorLeft && !data.hasDoorRight && data.hasDoorUp ||
               !data.hasDoorDown && !data.hasDoorLeft && data.hasDoorRight && !data.hasDoorUp;
    }

    private float GetRotationFromStraightTile(TileData data)
    {
        return data.hasDoorDown switch
        {
            true when !data.hasDoorLeft && !data.hasDoorRight && data.hasDoorUp => 90,
            false when !data.hasDoorLeft && data.hasDoorRight && !data.hasDoorUp => 0,
            _ => 0
        };
    }

    private bool isTTile(TileData data)
    {
        return data.hasDoorDown && data.hasDoorLeft && data.hasDoorRight && !data.hasDoorUp ||
               !data.hasDoorDown && data.hasDoorLeft && data.hasDoorRight && data.hasDoorUp ||
               data.hasDoorDown && !data.hasDoorLeft && data.hasDoorRight && data.hasDoorUp ||
               data.hasDoorDown && data.hasDoorLeft && !data.hasDoorRight && data.hasDoorUp;
    }

    private float GetRotationFromTTile(TileData data)
    {
        return data.hasDoorDown switch
        {
            true when data.hasDoorLeft && data.hasDoorRight && !data.hasDoorUp => 270,
            false when data.hasDoorLeft && data.hasDoorRight && data.hasDoorUp => 90,
            true when !data.hasDoorLeft && data.hasDoorRight && data.hasDoorUp => 180,
            true when data.hasDoorLeft && !data.hasDoorRight && data.hasDoorUp => 0,
            _ => 0
        };
    }

    private bool isCrossTile(TileData data)
    {
        return data.hasDoorDown && data.hasDoorLeft && data.hasDoorRight && data.hasDoorUp;
    }

    private bool isDeadEndTile(TileData data)
    {
        return data.hasDoorDown && !data.hasDoorLeft && !data.hasDoorRight && !data.hasDoorUp ||
               !data.hasDoorDown && data.hasDoorLeft && !data.hasDoorRight && !data.hasDoorUp ||
               !data.hasDoorDown && !data.hasDoorLeft && data.hasDoorRight && !data.hasDoorUp ||
               !data.hasDoorDown && !data.hasDoorLeft && !data.hasDoorRight && data.hasDoorUp;
    }

    private float GetRotationFromDeadEndTile(TileData data)
    {
        return data.hasDoorDown switch
        {
            true when !data.hasDoorLeft && !data.hasDoorRight && !data.hasDoorUp => 270,
            false when data.hasDoorLeft && !data.hasDoorRight && !data.hasDoorUp => 0,
            false when !data.hasDoorLeft && data.hasDoorRight && !data.hasDoorUp => 180,
            false when !data.hasDoorLeft && !data.hasDoorRight && data.hasDoorUp => 90,
            _ => 0
        };
    }

    public void SetExits()
    {
        for (int i = 0; i < _mapManager.width - 2; i++)
        {
            for (int j = 0; j < _mapManager.height - 2; j++)
            {
                var currentCell = _mapManager.mapArray[i, j];
            
                if (currentCell.isConnectedToPath)
                {
                    var hasDoorDown = currentCell.hasDoorDown;
                    var hasDoorUp = currentCell.hasDoorUp;
                    var hasDoorLeft = currentCell.hasDoorLeft;
                    var hasDoorRight = currentCell.hasDoorRight;

                    var isExitDown = (j == 0 && hasDoorDown) || (hasDoorDown && !_mapManager.mapArray[i, j - 1].PiecePlaced);
                    var isExitUp = (j == _mapManager.height - 3 && hasDoorUp) || (hasDoorUp && !_mapManager.mapArray[i, j + 1].PiecePlaced);
                    var isExitLeft = (i == 0 && hasDoorLeft) || (hasDoorLeft && !_mapManager.mapArray[i - 1, j].PiecePlaced);
                    var isExitRight = (i == _mapManager.width - 3 && hasDoorRight) || (hasDoorRight && !_mapManager.mapArray[i + 1, j].PiecePlaced);

                    currentCell.isExit = isExitDown || isExitUp || isExitLeft || isExitRight;
                }
            }
        }
    }

    public void SetConnectedToPath()
    {
        for (int i = 0; i < _mapManager.width - 2; i++)
        {
            for (int j = 0; j < _mapManager.height - 2; j++)
            {
                var currentCell = _mapManager.mapArray[i, j];

                if (currentCell.isConnectedToPath)
                    continue;

                var isConnectedLeft = i > 0 && _mapManager.mapArray[i - 1, j].isConnectedToPath && currentCell.hasDoorLeft;
                var isConnectedRight = i < _mapManager.width - 3 && _mapManager.mapArray[i + 1, j].isConnectedToPath && currentCell.hasDoorRight;
                var isConnectedDown = j > 0 && _mapManager.mapArray[i, j - 1].isConnectedToPath && currentCell.hasDoorDown;
                var isConnectedUp = j < _mapManager.height - 3 && _mapManager.mapArray[i, j + 1].isConnectedToPath && currentCell.hasDoorUp;

                if (isConnectedLeft || isConnectedRight || isConnectedDown || isConnectedUp)
                {
                    currentCell.isConnectedToPath = true;
                }
            }
        }
    }

    public Vector2Int[] GetTilesInLineOfSight(Vector2Int startPos)
    {
        List<Vector2Int> tiles = new List<Vector2Int>();
        for (int i = 0; i < 4; i++)
        {
            int x = startPos.x;
            int y = startPos.y;
            int xDir = 0;
            int yDir = 0;
            switch (i)
            {
                case 0:
                    xDir = 1;
                    break;
                case 1:
                    xDir = -1;
                    break;
                case 2:
                    yDir = 1;
                    break;
                case 3:
                    yDir = -1;
                    break;
            }
            while (x >= 0 && x < _mapManager.width - 2 && y >= 0 && y < _mapManager.height - 2)
            {
                if (_mapManager.mapArray[x, y].isRoom)
                {
                    _mapManager.mapArray[x, y].isVisited = true;
                    tiles.Add(new Vector2Int(x, y));
                }
                else
                {
                    break;
                }
                x += xDir;
                y += yDir;
            }
        }
        return tiles.ToArray();
    }
}