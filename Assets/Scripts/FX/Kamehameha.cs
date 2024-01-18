using System;
using System.Collections;
using UnityEngine;

public class Kamehameha : AttackFX
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private ParticleSystem particleSystem;

    private Vector3 targetPos;
    float timer = 0;
    private DirectionToMove _direction;

    IEnumerator IncreaseWidth()
    {
        float i = 0;
        transform.LookAt(targetPos);
        switch (_direction)
        {
            case DirectionToMove.Left:
                while (i < timer)
                {
                    float dt = Mathf.Lerp(transform.position.x, targetPos.x, i / timer);
                    lineRenderer.SetPosition(1, new Vector3(dt, transform.position.y, transform.position.z));
                    float distance = Mathf.Abs(transform.position.x - dt) / particleSystem.startSpeed;
                    particleSystem.startLifetime = distance;
                    i += Time.deltaTime;
                    yield return null;
                }
                break;
            case DirectionToMove.Right:
                while (i < timer)
                {
                    float dt = Mathf.Lerp(transform.position.x, targetPos.x, i / timer);
                    lineRenderer.SetPosition(1, new Vector3(dt, transform.position.y, transform.position.z));
                    float distance = Mathf.Abs(transform.position.x - dt) / particleSystem.startSpeed;
                    particleSystem.startLifetime = distance;
                    i += Time.deltaTime;
                    yield return null;
                }
                break;
            case DirectionToMove.Up:
                while (i < timer)
                {
                    float dt = Mathf.Lerp(transform.position.z, targetPos.z, i / timer);
                    lineRenderer.SetPosition(1, new Vector3(transform.position.x, transform.position.y, dt));
                    float distance = Mathf.Abs(transform.position.z - dt) / particleSystem.startSpeed;
                    particleSystem.startLifetime = distance;
                    i += Time.deltaTime;
                    yield return null;
                }
                break;
            case DirectionToMove.Down:
                while (i < timer)
                {
                    float dt = Mathf.Lerp(transform.position.z, targetPos.z, i / timer);
                    lineRenderer.SetPosition(1, new Vector3(transform.position.x, transform.position.y, dt));
                    float distance = Mathf.Abs(transform.position.z - dt) / particleSystem.startSpeed;
                    particleSystem.startLifetime = distance;
                    i += Time.deltaTime;
                    yield return null;
                }
                break;
            case DirectionToMove.None:
                yield break;
            case DirectionToMove.Error:
                yield break;
            default:
                yield break;
        }
        Destroy(gameObject, 0.5f);
    }

    public override void Launch()
    {
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.position);
        StartCoroutine(IncreaseWidth());
    }

    public override void Init(Transform target, Transform owner, float time, DirectionToMove direction)
    {
        transform.position = owner.position;
        targetPos = target.position;
        timer = time;
        _direction = direction;
    }
}