
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
}
