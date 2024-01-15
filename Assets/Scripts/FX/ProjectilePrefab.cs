using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[Serializable]
public struct ProjToQueue
{
    public Transform target;
    public float timer;
}

public class ProjectilePrefab : AttackFX
{
    [SerializeField] List<ProjToQueue> projectiles;
    private float timer;
    private Vector3 dest;

    public override void Init(Transform target, Transform owner, float time, DirectionToMove direction)
    {
        transform.LookAt(target);
        float directionToMove = 0;
        
        dest = new Vector3(target.position.x, transform.position.y, target.position.z);
        float offset = 0.5f;
        
        switch (direction)
        {
            case DirectionToMove.Up:
                directionToMove = 270;
                dest -= new Vector3(0, 0, offset);
                break;
            case DirectionToMove.Down:
                directionToMove = 90;
                dest += new Vector3(0, 0, offset);
                break;
            case DirectionToMove.Left:
                directionToMove = 0;
                dest += new Vector3(offset, 0, 0);
                break;
            case DirectionToMove.Right:
                directionToMove = 180;
                dest -= new Vector3(offset, 0, 0);
                break;
        }
        
        transform.rotation = Quaternion.Euler(90, 0, directionToMove);
        //il faut rotate en fonction de la direction
        timer = time;
    }
    
    IEnumerator LaunchProjectile()
    {
        yield return new WaitForSeconds(timer);
        while (projectiles.Count > 0)
        {
            ProjToQueue projectile = projectiles[0];
            projectile.target.gameObject.SetActive(true);
            yield return new WaitForSeconds(projectile.timer);
            projectile.target.gameObject.SetActive(false);
            projectiles.RemoveAt(0);
        }

    }

    public override void Launch()
    {
        float totalTimer = timer;
        foreach (var projectile in projectiles)
        {
            totalTimer += projectile.timer;
        }
        transform.DOMove(dest, timer).SetEase(Ease.InBack);
        StartCoroutine(LaunchProjectile());
        
        Destroy(gameObject, totalTimer);
    }
}
