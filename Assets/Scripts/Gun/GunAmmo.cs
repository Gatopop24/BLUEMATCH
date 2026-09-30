using System.Collections;
using UnityEngine;

public class GunAmmo : MonoBehaviour
{
    [SerializeField] private int magazineSize = 30;
    [SerializeField] private int currentAmmo;
    [SerializeField] private int reserveAmmo = 90;
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
}
