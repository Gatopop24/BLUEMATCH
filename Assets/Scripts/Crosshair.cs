using UnityEngine;

public class Crosshair : MonoBehaviour
{
    [SerializeField] private RectTransform crosshairRect;
    [SerializeField] private float normalSize = 20f;
    [SerializeField] private float expandedSize = 35f;
    [SerializeField] private float expandSpeed = 10f;
    private float targetSize;

    private void Start()
    {
        targetSize = normalSize;
    }

    private void Update()
    {
        if (InputController.Instance.GetButton(InputController.InputAction.Fire))
        {
            targetSize = expandedSize;
        }
        else
        {
            targetSize = normalSize;
        }
        AnimateCrosshair(targetSize);
    }

    private void AnimateCrosshair(float target)
    {
        float currentSize = crosshairRect.sizeDelta.x;
        float newSize = Mathf.Lerp(currentSize, target, Time.deltaTime * expandSpeed);
        crosshairRect.sizeDelta = new Vector2(newSize, newSize);
    }
}
