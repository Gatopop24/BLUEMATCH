using TMPro;
using UnityEngine;

public class AmmoUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private BaseGun currentGun;

    private void Update()
    {
        ammoText.text = $"{currentGun.CurrentAmmo} | {currentGun.ReserveAmmo}";
    }

    public void SetGun(BaseGun gun)
    {
        currentGun = gun;
    }
}
