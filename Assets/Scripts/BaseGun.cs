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
    [SerializeField] protected int magazineSize = 30;
    [SerializeField] protected int currentAmmo;
    [SerializeField] protected int reserveAmmo = 90;
    [SerializeField] protected float reloadTime = 1.5f;
    protected bool isReloading = false;
    protected Coroutine reloadRoutine;

    protected virtual void Start()
    {
        currentCooldown = fireCooldown;
        currentAmmo = magazineSize;
    }

    public virtual void Initialize(Transform ownerCamera)
    {
        playerCamera = ownerCamera;
    }

    protected virtual void Update()
    {
        if (isReloading)
        {
            currentCooldown -= Time.deltaTime;
            return;
        }

        if (InputController.Instance.GetButtonDown(InputController.InputAction.Reload))
        {
            TryReload();
        }

        if (automatic)
        {
            if (InputController.Instance.GetButton(InputController.InputAction.Fire))
            {
                if (currentCooldown <= 0f)
                {
                    TryShoot();
                }
            }
        }
        else
        {
            if (InputController.Instance.GetButtonDown(InputController.InputAction.Fire))
            {
                if (currentCooldown <= 0f)
                {
                    TryShoot();
                }
            }
        }
        currentCooldown -= Time.deltaTime;
    }

    protected virtual void TryShoot()
    {
        if (currentAmmo <= 0)
        {
            TryReload();
            return;
        }

        Shoot();
        currentAmmo--;
        currentCooldown = fireCooldown;
    }

    protected virtual void TryReload()
    {
        if (isReloading)
        {
            return;
        } 
        if (currentAmmo >= magazineSize)
        {
            return;
        }
        if (reserveAmmo <= 0)
        {
            return;
        } 

        reloadRoutine = StartCoroutine(Reload());
    }

    protected virtual IEnumerator Reload()
    {
        isReloading = true;

        yield return new WaitForSeconds(reloadTime);

        int ammoNeeded = magazineSize - currentAmmo;
        int ammoToLoad = Mathf.Min(ammoNeeded, reserveAmmo);

        currentAmmo += ammoToLoad;
        reserveAmmo -= ammoToLoad;

        isReloading = false;
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
        float elapsed = 0f;

        while(elapsed < duration)
        {
            Vector3 currentEnd = Vector3.Lerp(start, end, elapsed/duration);
            line.SetPosition(1, currentEnd);
            elapsed += Time.deltaTime;
            yield return null;
        }

        line.SetPosition(1, end);
        yield return new WaitForSeconds(0.05f);
        line.gameObject.SetActive(false);
    }
}