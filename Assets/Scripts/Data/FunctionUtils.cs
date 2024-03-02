
using UnityEngine;

public static class FunctionUtils
{
  public static DirectionToMove GetDirectionToMoveWithTilePos(Vector2Int targetPos, Vector2Int heroPos)
  {
    if (targetPos.x > heroPos.x)
    {
      return DirectionToMove.Right;
    }
    else if (targetPos.x < heroPos.x)
    {
      return DirectionToMove.Left;
    }
    else if (targetPos.y > heroPos.y)
    {
      return DirectionToMove.Up;
    }
    else if (targetPos.y < heroPos.y)
    {
      return DirectionToMove.Down;
    }
    return DirectionToMove.None;
  }
  
  public static float GetDistanceBetweenTwoPos(Vector2Int pos1, Vector2Int pos2)
  {
    return Mathf.Abs(pos1.x - pos2.x) + Mathf.Abs(pos1.y - pos2.y);
  }
}
