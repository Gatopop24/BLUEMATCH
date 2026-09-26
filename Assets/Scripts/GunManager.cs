using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    [SerializeField] private Transform gunPosition;
    [SerializeField] private Transform playerCamera;
    [SerializeField] private List<GameObject> weaponPrefabs;
    [SerializeField] private AmmoUI ammoDisplay;
    private GunSelectorUI weaponSelectorUI;
    private List<BaseGun> equippedWeapons = new List<BaseGun>();
    private int currentGunIndex = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        weaponSelectorUI = FindFirstObjectByType<GunSelectorUI>();
        ammoDisplay = FindFirstObjectByType<AmmoUI>();
        SpawnAllGuns();
        SelectGun(currentGunIndex);
    }

    // Update is called once per frame
    private void Update()
    {
        float scroll = InputController.Instance.GetAxis(InputController.InputAction.ChangeGun);
        if(scroll < 0f)
        {
            CycleGun(1);
        }
        else if(scroll > 0f)
        {
            CycleGun(-1);
        }
    }

    private void SpawnAllGuns()
    {
        foreach (GameObject prefab in weaponPrefabs)
        {
            GameObject weaponInstance = Instantiate(prefab, gunPosition.position, gunPosition.rotation, gunPosition);
            BaseGun gun = weaponInstance.GetComponent<BaseGun>();
            gun.Initialize(playerCamera);
            weaponInstance.SetActive(false);
            equippedWeapons.Add(gun);
        }
    }

    private void CycleGun(int direction)
    {

        int newIndex = currentGunIndex + direction;

        if (newIndex >= equippedWeapons.Count)
        {
            newIndex = 0;
        } 
        if (newIndex < 0)
        {
            newIndex = equippedWeapons.Count - 1;
        } 
        SelectGun(newIndex);
    }

    private void SelectGun(int index)
    {
        for (int i = 0; i < equippedWeapons.Count; i++)
        {
            equippedWeapons[i].gameObject.SetActive(i == index);
        }
        currentGunIndex = index;
        if (weaponSelectorUI != null)
        {
            weaponSelectorUI.SetEquippedWeapon(index);
        }
        ammoDisplay.SetGun(equippedWeapons[index]);
    }
}
