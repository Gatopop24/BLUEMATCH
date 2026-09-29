using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float speed = 100f;
    [SerializeField] private float RotationX = 0f;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float normalFOV = 60f;
    [SerializeField] private float fovSpeed = 8f;
    private float targetFOV;
    private float currentAimSensitivity = 1f;
    private bool isAiming = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        playerCamera = GetComponent<Camera>();
        targetFOV = normalFOV;
    }

    // Update is called once per frame
    private void Update()
    {
        MoveCamera();
        ApplyFOV();
    }

    private void MoveCamera()
    {
        float MouseX = InputController.Instance.GetAxis(InputController.InputAction.MouseX) * speed * Time.deltaTime;
        float MouseY = InputController.Instance.GetAxis(InputController.InputAction.MouseY) * speed * Time.deltaTime;
        
        RotationX -= MouseY;
        RotationX = Mathf.Clamp(RotationX, -90, 90);

        transform.localRotation = Quaternion.Euler(RotationX, 0f, 0f);
        playerTransform.Rotate(Vector3.up * MouseX);
    }

    private void ApplyFOV()
    {
        if (playerCamera == null)
        {
            return;
        } 
        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, fovSpeed * Time.deltaTime);
    }

    public void SetAiming(bool aiming, float aimFOV, float aimSensitivityMultiplier)
    {
        isAiming = aiming;
        if (aiming)
        {
            targetFOV = aimFOV;
        }
        else
        {
            targetFOV = normalFOV;
        }
        currentAimSensitivity = aimSensitivityMultiplier;
    }
}
