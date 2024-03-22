using System.Collections;
using System.Collections.Generic;
using BehaviourTree;
using DG.Tweening;
using UnityEngine;

public class AttackMinionsInLine : Node
{
    HeroBlackboard blackboard;

    public AttackMinionsInLine(HeroBlackboard _blackboard)
    {
        blackboard = _blackboard;
    }

    public override NodeState Evaluate(Node root)
    {
        Vector2Int indexes = blackboard.hero.GetIndexHeroPos();
        Vector2Int MapSize = blackboard.hero.mapManager.GetSizeDungeon();


        blackboard.Targets.Clear();
        switch (blackboard.directionToMove)
        {
            case DirectionToMove.Up:
                for (int y = indexes.y; y < MapSize.y; y++)
                {
                    TileData tileData = blackboard.hero.mapManager.GetTileDataAtPosition(indexes.x, y);
                    if (!tileData.PiecePlaced) break;
                    if (!tileData) break;

                    if (blackboard.hero.mapManager.GetNbMonstersOnPos(new Vector2Int(indexes.x, y)) > 0)
                    {
                        foreach (var enemy in tileData.enemies)
                        {
                            blackboard.Targets.Add(enemy);
                        }
                    }

                    if (!tileData.hasDoorUp) break;
                }

                break;
            case DirectionToMove.Right:
                for (int x = indexes.x; x < MapSize.x; x++)
                {
                    TileData tileData = blackboard.hero.mapManager.GetTileDataAtPosition(x, indexes.y);
                    if (!tileData.PiecePlaced) break;
                    if (blackboard.hero.mapManager.GetNbMonstersOnPos(new Vector2Int(x, indexes.y)) > 0)
                    {
                        foreach (var enemy in tileData.enemies)
                        {
                            blackboard.Targets.Add(enemy);
                        }
                    }

                    if (!tileData.hasDoorRight) break;
                }

                break;
            case DirectionToMove.Down:
                for (int y = indexes.y; y >= 0; y--)
                {
                    TileData tileData = blackboard.hero.mapManager.GetTileDataAtPosition(indexes.x, y);
                    if (!tileData.PiecePlaced) break;
                    if (!tileData) break;

                    if (blackboard.hero.mapManager.GetNbMonstersOnPos(new Vector2Int(indexes.x, y)) > 0)
                    {
                        foreach (var enemy in tileData.enemies)
                        {
                            blackboard.Targets.Add(enemy);
                        }
                    }

                    if (!tileData.hasDoorDown) break;
                }

                break;
            case DirectionToMove.Left:
                for (int x = indexes.x; x >= 0; x--)
                {
                    TileData tileData = blackboard.hero.mapManager.GetTileDataAtPosition(x, indexes.y);
                    if (!tileData.PiecePlaced) break;
                    if (blackboard.hero.mapManager.GetNbMonstersOnPos(new Vector2Int(x, indexes.y)) > 0)
                    {
                        foreach (var enemy in tileData.enemies)
                        {
                            blackboard.Targets.Add(enemy);
                        }
                    }

                    if (!tileData.hasDoorLeft) break;
                }

                break;
            case DirectionToMove.None:
                return NodeState.Failure;
            case DirectionToMove.Error:
                return NodeState.Failure;
        }

        if (blackboard.Targets.Count == 0)
        {
            return NodeState.Failure;
        }

        blackboard.hero.PlayAttackClip();
        blackboard.hero.PlayAttackFX(blackboard.Targets[^1].transform, 1.0f, blackboard.directionToMove);
        blackboard.hero.AddAnim(new AnimToQueue(blackboard.hero.transform, blackboard.Targets[^1].transform,
            Vector3.zero, true,
            1.0f, Ease.InBack, 2));
        foreach (var enemy in blackboard.Targets)
        {
            if (enemy)
                enemy.TakeDamage(blackboard.hero.info.So.AttackPoint, blackboard.hero.attackType);
        }

        return NodeState.Success;
    }
}