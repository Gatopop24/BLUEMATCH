using System;
using System.Collections;
using UnityEngine;

public class BaseGun : MonoBehaviour
{
    [SerializeField] protected ObjectPooler pooler;
    [SerializeField] protected int damage = 10;
    [SerializeField] protected float fireCooldown;
    [SerializeField] protected float currentCooldown;
    [SerializeField] protected float bulletRange;
    [SerializeField] protected Transform muzzle;
    protected Transform playerCamera;
    [SerializeField] protected GameObject bulletTrail;
    [SerializeField] protected float bulletTrailSpeed = 300f;
    public bool automatic;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        currentCooldown = fireCooldown;
    }

    public virtual void Initialize(Transform ownerCamera)
    {
        playerCamera = ownerCamera;
    }

    protected virtual void Update()
    {
        if(automatic)
        {
            if(InputController.Instance.GetButton(InputController.InputAction.Fire))
            {
                if(currentCooldown <= 0f)
                {
                    Shoot();
                    currentCooldown = fireCooldown;
                }
            }
        }
        else
        {
            if(InputController.Instance.GetButtonDown(InputController.InputAction.Fire))
            {
                if(currentCooldown <= 0f)
                {
                    Shoot();
                    currentCooldown = fireCooldown;
                }
            }
        }
        currentCooldown -= Time.deltaTime;
    }

    protected virtual void Shoot()
    {
        Ray gunRay = new Ray(playerCamera.position, playerCamera.forward);
        Vector3 hitPoint;

        if(Physics.Raycast(gunRay, out RaycastHit hit, bulletRange))
        {
            hitPoint = hit.point;
            Health health = hit.collider.GetComponent<Health>();
            if(health != null)
            {
                health.TakeDamage(damage);
            }
        }
        else
        {
            hitPoint = gunRay.origin + gunRay.direction * bulletRange;
        }
        SpawnBulletTrail(hitPoint);
    }

    protected virtual void SpawnBulletTrail(Vector3 hitPoint)
    {
        GameObject trailObj = pooler.GetPooledObject();
        trailObj.transform.position = muzzle.position;
        trailObj.transform.rotation = Quaternion.identity;
        trailObj.SetActive(true); 
        LineRenderer line = trailObj.GetComponent<LineRenderer>();
        StartCoroutine(AnimateTrail(line, muzzle.position, hitPoint));
    }

    protected virtual IEnumerator AnimateTrail(LineRenderer line, Vector3 start, Vector3 end)
    {
        line.SetPosition(0, start);
        line.SetPosition(1, start);

        float distance = Vector3.Distance(start, end);
        float duration = distance / bulletTrailSpeed;
        float elapsed = 0f; //elapsed time

        while(elapsed < duration)
        {
            Vector3 currentEnd = Vector3.Lerp(start, end, elapsed/duration);
            line.SetPosition(1, currentEnd);
            elapsed += Time.deltaTime;
            yield return null;
        }

        line.SetPosition(1, end);
        yield return new WaitForSeconds(0.05f);
        line.gameObject.SetActive(false); //modify for object pooling
    }
}
