using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Direction2D
{
     public static List<Vector2Int> cardDirList = new List<Vector2Int>
     {
          //all 8 directions
          new Vector2Int(0, 1),
          new Vector2Int(1, 1),
          new Vector2Int(1, 0),
          new Vector2Int(1, -1),
          new Vector2Int(0, -1),
          new Vector2Int(-1, -1),
          new Vector2Int(-1, 0),
          new Vector2Int(-1, 1),
     };
}
