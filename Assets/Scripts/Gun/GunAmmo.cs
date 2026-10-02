using System.Collections;
using UnityEngine;

public class GunAmmo : MonoBehaviour
{
    [SerializeField] private int magazineSize = 30;
    [SerializeField] private int currentAmmo;
    [SerializeField] private int maxReserveAmmo = 90;
    [SerializeField] private int reserveAmmo;
    [SerializeField] private float reloadTime = 1.5f;
    private bool isReloading = false;
    private Coroutine reloadRoutine;
    public int CurrentAmmo
    {
        get { return currentAmmo; }
    }
    public int ReserveAmmo
    {
        get { return reserveAmmo; }
    }
    public bool IsReloading
    {
        get { return isReloading; }
    }

    private void Start()
    {
        currentAmmo = magazineSize;
        reserveAmmo = maxReserveAmmo;
    }

    private void OnDisable()
    {
        if (reloadRoutine != null)
        {
            StopCoroutine(reloadRoutine);
            reloadRoutine = null;
        }

        isReloading = false;
    }

    public void UseAmmo()
    {
        currentAmmo--;
    }

    public void TryReload()
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

    private IEnumerator Reload()
    {
        isReloading = true;
        yield return new WaitForSeconds(reloadTime);
        int ammoNeeded = magazineSize - currentAmmo;
        int ammoToLoad = Mathf.Min(ammoNeeded, reserveAmmo);
        currentAmmo += ammoToLoad;
        reserveAmmo -= ammoToLoad;
        isReloading = false;
    }

    public void RefillAmmo()
    {
        if (reloadRoutine != null)
        {
            StopCoroutine(reloadRoutine);
            reloadRoutine = null;
        }

        isReloading = false;

        currentAmmo = magazineSize;
        reserveAmmo = maxReserveAmmo;
    }
}
