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
    protected CameraController cameraController;
    [SerializeField] protected GameObject bulletTrail;
    [SerializeField] protected float bulletTrailSpeed = 300f;
    public bool automatic;
    [SerializeField] protected int magazineSize = 30;
    [SerializeField] protected int currentAmmo;
    [SerializeField] protected int reserveAmmo = 90;
    [SerializeField] protected float reloadTime = 1.5f;
    protected bool isReloading = false;
    protected Coroutine reloadRoutine;
    [SerializeField] protected Vector3 kickbackPosition = new Vector3(0f, 0f, -0.1f);
    [SerializeField] protected Vector3 kickbackRotation = new Vector3(-5f, 0f, 0f);
    [SerializeField] protected float kickbackReturnSpeed = 6f;
    [SerializeField] protected AudioSource audioSource;
    [SerializeField] protected AudioClip shootSound;

    [SerializeField] protected Vector3 aimPositionOffset = new Vector3(0f, -0.05f, 0.1f);
    [SerializeField] protected float aimSpeed = 10f;
    [SerializeField] protected float aimFOV = 40f;
    [SerializeField] protected float aimSensitivityMultiplier = 0.5f;
    protected bool isAiming = false;
    protected Vector3 aimTargetPos;

    protected Vector3 originalWeaponPos;
    protected Quaternion originalWeaponRot;
    protected Vector3 targetKickPos;
    protected Quaternion targetKickRot;

    protected virtual void Awake()
    {
        originalWeaponPos = transform.localPosition;
        originalWeaponRot = transform.localRotation;
        targetKickPos = originalWeaponPos;
        targetKickRot = originalWeaponRot;
        aimTargetPos = originalWeaponPos;
    }

    protected virtual void Start()
    {
        currentCooldown = fireCooldown;
        currentAmmo = magazineSize;
    }

    public virtual void Initialize(Transform ownerCamera)
    {
        playerCamera = ownerCamera;
        cameraController = ownerCamera.GetComponent<CameraController>();
    }

    protected virtual void Update()
    {
        if (isReloading)
        {
            currentCooldown -= Time.deltaTime;
            ApplyWeaponRecoilVisual();
            return;
        }

        if (InputController.Instance.GetButtonDown(InputController.InputAction.Reload))
        {
            TryReload();
        }
        HandleAimInput();
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

        ApplyWeaponRecoilVisual();
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
        TriggerKickback();
        PlayShootSound();
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

    protected virtual void TriggerKickback()
    {
        targetKickPos = aimTargetPos + kickbackPosition;
        targetKickRot = originalWeaponRot * Quaternion.Euler(kickbackRotation);
    }

    protected virtual void ApplyWeaponRecoilVisual()
    {
        targetKickPos = Vector3.Lerp(targetKickPos, aimTargetPos, kickbackReturnSpeed * Time.deltaTime);
        targetKickRot = Quaternion.Lerp(targetKickRot, originalWeaponRot, kickbackReturnSpeed * Time.deltaTime);

        transform.localPosition = Vector3.Lerp(transform.localPosition, targetKickPos, aimSpeed * Time.deltaTime);
        transform.localRotation = targetKickRot;
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

    protected virtual void PlayShootSound()
    {
        if (audioSource != null && shootSound != null)
        {
            audioSource.Stop();
            audioSource.clip = shootSound;
            audioSource.Play();
        }
    }

    protected virtual void HandleAimInput()
    {
        bool aimButtonHeld = InputController.Instance.GetButton(InputController.InputAction.Aim);

        if (aimButtonHeld != isAiming)
        {
            isAiming = aimButtonHeld;
            if (isAiming)
            {
                aimTargetPos = originalWeaponPos + aimPositionOffset;
            }
            else
            {
                aimTargetPos = originalWeaponPos;
            }
            if (cameraController != null)
            {
                cameraController.SetAiming(isAiming, aimFOV, aimSensitivityMultiplier);
            }
        }
    }
}