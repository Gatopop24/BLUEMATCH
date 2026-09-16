using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float speed = 100f;
    [SerializeField] private float RotationX = 0f;
    [SerializeField] private Transform playerTransform;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    private void Update()
    {
        MoveCamera();
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
}
