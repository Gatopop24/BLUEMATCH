using UnityEngine;

public class AmmoBox : MonoBehaviour
{
    [SerializeField] private int cost = 25; 
    public int Cost
    {
        get { return cost;}
    }
    public void RefillAmmo(BaseGun gun)
    {
        if(gun.IsAmmoFull)
        {
            return;
        }
        if(!ScoreManager.Instance.Spend(cost))
        {
            return;
        }
        gun.RefillAmmo();
    }
}
