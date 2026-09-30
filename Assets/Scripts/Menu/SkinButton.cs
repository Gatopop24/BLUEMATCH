using UnityEngine;

public class SkinButton : MonoBehaviour
{
    public int skinIndex;

    public void Select()
    {
        SkinManager.Instance.SelectSkin(skinIndex);
    }
}
