using System;
using System.Collections;
using UnityEngine;

public class BaseGun : MonoBehaviour
{
    [SerializeField] protected int damage = 10;
    [SerializeField] protected float fireCooldown;
    [SerializeField] protected float currentCooldown;
    [SerializeField] protected float bulletRange;

    protected Transform playerCamera;
    protected CameraController cameraController;

    public bool automatic;

    protected GunAmmo gunAmmo;
    protected GunAiming gunAiming;
    protected GunRecoil gunRecoil;
    protected GunEffects gunEffects;
    public int CurrentAmmo
    {
        get { return gunAmmo.CurrentAmmo; }
    }

    public int ReserveAmmo
    {
        get { return gunAmmo.ReserveAmmo; }
    }

    protected virtual void Awake()
    {
        gunAmmo = GetComponent<GunAmmo>();
        gunAiming = GetComponent<GunAiming>();
        gunRecoil = GetComponent<GunRecoil>();
        gunEffects = GetComponent<GunEffects>();
    }

    protected virtual void Start()
    {
        currentCooldown = fireCooldown;
    }

    public virtual void Initialize(Transform ownerCamera)
    {
        playerCamera = ownerCamera;
        cameraController = ownerCamera.GetComponent<CameraController>();
        gunAiming.Initialize(ownerCamera);
    }

    protected virtual void Update()
    {
        if (gunAmmo.IsReloading)
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
        if (gunAmmo.CurrentAmmo <= 0)
        {
            TryReload();
            return;
        }

        Shoot();
        gunAmmo.UseAmmo();
        currentCooldown = fireCooldown;
    }

    protected virtual void TryReload()
    {
        gunAmmo.TryReload();
    }

    protected virtual void Shoot()
    {
        TriggerKickback();
        PlayShootSound();
        Ray gunRay = new Ray(playerCamera.position, playerCamera.forward);
        Vector3 hitPoint;

        if (Physics.Raycast(gunRay, out RaycastHit hit, bulletRange))
        {
            hitPoint = hit.point;
            Health health = hit.collider.GetComponent<Health>();
            if (health != null)
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
        gunRecoil.TriggerKickback();
    }

    protected virtual void ApplyWeaponRecoilVisual()
    {
        gunRecoil.ApplyWeaponRecoilVisual();
    }

    protected virtual void PlayShootSound()
    {
        gunEffects.PlayShootSound();
    }

    protected virtual void SpawnBulletTrail(Vector3 hitPoint)
    {
        gunEffects.SpawnBulletTrail(hitPoint);
    }

    protected virtual void HandleAimInput()
    {
        gunAiming.HandleAimInput();
        Vector3 aimPosition = gunAiming.GetAimPosition(gunRecoil.OriginalWeaponPosition);
        gunRecoil.SetAimTargetPosition(aimPosition);
    }
}