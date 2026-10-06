using UnityEngine;

public class GunAiming : MonoBehaviour
{
    [SerializeField] private Vector3 aimPositionOffset = new Vector3(0f, -0.05f, 0.1f);
    [SerializeField] private float aimSpeed = 10f;
    [SerializeField] private float aimFOV = 40f;
    [SerializeField] private float aimSensitivityMultiplier = 0.5f;
    private Transform playerCamera;
    private CameraController cameraController;
    private bool isAiming = false;
    public bool IsAiming
    {
        get { return isAiming; }
    }

    public void Initialize(Transform ownerCamera)
    {
        playerCamera = ownerCamera;
        cameraController = ownerCamera.GetComponent<CameraController>();
    }

    public void HandleAimInput()
    {
        bool aimButtonHeld = InputController.Instance.GetButton(InputController.InputAction.Aim);

        if (aimButtonHeld != isAiming)
        {
            isAiming = aimButtonHeld;

            if (cameraController != null)
            {
                cameraController.SetAiming(isAiming, aimFOV, aimSensitivityMultiplier);
            }
        }
    }

    public Vector3 GetAimPosition(Vector3 originalWeaponPosition)
    {
        if (isAiming)
        {
            return originalWeaponPosition + aimPositionOffset;
        }

        return originalWeaponPosition;
    }

    public float AimSpeed
    {
        get { return aimSpeed; }
    }

    public void StopAiming()
    {
        if (!isAiming)
        {
            return;
        }

        isAiming = false;
        cameraController.SetAiming(false, aimFOV, aimSensitivityMultiplier);
    }
}
