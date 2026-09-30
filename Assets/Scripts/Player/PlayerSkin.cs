using UnityEngine;

public class PlayerSkin : MonoBehaviour
{
    public Renderer targetRenderer;

    void Start()
    {
        if (SkinManager.Instance == null)
        {
            return;
        }

        Material mat = SkinManager.Instance.GetCurrentMaterial();
        if (mat != null)
        {
            targetRenderer.material = mat;
        }
    }
}
