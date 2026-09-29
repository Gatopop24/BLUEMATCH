using UnityEngine;
using UnityEngine.UI;

public class GunSelectorUI : MonoBehaviour
{
    [System.Serializable]
    public class WeaponSlot
    {
        public Image icon;
    }

    [SerializeField] private WeaponSlot[] slots;
    [SerializeField] private Color equippedColor = Color.white;
    [SerializeField] private Color unequippedColor = new Color(1f, 1f, 1f, 0.35f);

    public void SetEquippedWeapon(int equippedIndex)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].icon == null)
            {
                continue; // avoid any null reference
            }
            if (i == equippedIndex)
            {
                slots[i].icon.color = equippedColor;
            }
            else
            {
                slots[i].icon.color = unequippedColor;
            }
        }
    }
}
