using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float jumpForce = 50f;
    [SerializeField] private bool onFloor;
    [SerializeField] private Transform floorCheck;
    [SerializeField] private float floorDistance = 0.2f;
    [SerializeField] private LayerMask floorMask;
    private Rigidbody playerRB;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    private void Start()
    {
        playerRB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void Update()
    {
        IsOnFloor();
        MovePlayer();
        if(InputController.Instance.GetButtonDown(InputController.InputAction.Jump) && onFloor)
        {
            Jump();
        }
    }
/*
    private void FixedUpdate()
    {
        MovePlayer();
    }
*/
    private void MovePlayer()
    {
        float moveX = InputController.Instance.GetAxis(InputController.InputAction.MoveX);
        float moveZ = InputController.Instance.GetAxis(InputController.InputAction.MoveY);
        Vector3 movement = (transform.right * moveX) + (transform.forward * moveZ);
        Vector3 velocity = movement * speed;
        velocity.y = playerRB.linearVelocity.y;
        playerRB.linearVelocity = velocity;
    }

    private void Jump()
    {
        playerRB.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void IsOnFloor()
    {
        onFloor = Physics.CheckSphere(floorCheck.position, floorDistance, floorMask);
    }

}
