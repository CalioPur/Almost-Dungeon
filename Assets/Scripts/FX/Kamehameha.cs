using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class Kamehameha : AttackFX
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private ParticleSystem particle;

    private Vector3 targetPos;
    float timer = 0;
    private DirectionToMove _direction;

    IEnumerator IncreaseWidth()
    {
        float i = 0;
        transform.LookAt(targetPos);
        var mainModule = particle.main;
        switch (_direction)
        {
            case DirectionToMove.Left:
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                while (i < timer)
                {
                    float dt = Mathf.Lerp(transform.position.x, targetPos.x, i / timer);
                    lineRenderer.SetPosition(0, transform.position);
                    lineRenderer.SetPosition(1, new Vector3(dt, transform.position.y, transform.position.z));
                    float distance = Mathf.Abs(transform.position.x - dt) / particle.startSpeed;
                    mainModule.startLifetime = distance;
                    i += Time.deltaTime;
                    yield return null;
                }
                break;
            case DirectionToMove.Right:
                while (i < timer)
                {
                    float dt = Mathf.Lerp(transform.position.x, targetPos.x, i / timer);
                    lineRenderer.SetPosition(0, transform.position);
                    lineRenderer.SetPosition(1, new Vector3(dt, transform.position.y, transform.position.z));
                    float distance = Mathf.Abs(transform.position.x - dt) / particle.startSpeed;
                    mainModule.startLifetime = distance;
                    i += Time.deltaTime;
                    yield return null;
                }
                break;
            case DirectionToMove.Up:
                transform.rotation = Quaternion.Euler(0, 270, 0);
                while (i < timer)
                {
                    float dt = Mathf.Lerp(transform.position.z, targetPos.z, i / timer);
                    lineRenderer.SetPosition(0, transform.position);
                    lineRenderer.SetPosition(1, new Vector3(transform.position.x, transform.position.y, dt));
                    float distance = Mathf.Abs(transform.position.z - dt) / particle.startSpeed;
                    mainModule.startLifetime = distance;
                    i += Time.deltaTime;
                    yield return null;
                }
                break;
            case DirectionToMove.Down:
                transform.rotation = Quaternion.Euler(0, 90, 0);
                while (i < timer)
                {
                    float dt = Mathf.Lerp(transform.position.z, targetPos.z, i / timer);
                    lineRenderer.SetPosition(0, transform.position);
                    lineRenderer.SetPosition(1, new Vector3(transform.position.x, transform.position.y, dt));
                    float distance = Mathf.Abs(transform.position.z - dt) / particle.startSpeed;
                    mainModule.startLifetime = distance;
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
        transform.position = owner.position + new Vector3(0, 0.4f, 0);
        targetPos = target.position;
        timer = time;
        _direction = direction;
    }
}