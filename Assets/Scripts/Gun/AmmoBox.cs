using UnityEngine;

public class AmmoBox : MonoBehaviour
{
    public void RefillAmmo(BaseGun gun)
    {
        gun.RefillAmmo();
    }
}
