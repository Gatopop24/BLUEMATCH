using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float jumpForce = 50f;
    [SerializeField] private bool onFloor;
    [SerializeField] private Transform floorCheck;
    [SerializeField] private float floorDistance = 0.2f;
    [SerializeField] private LayerMask floorMask;
    [SerializeField] private float swingControlForce = 20f;
    [SerializeField] private float airControlForce = 15f;
    private Health playerHealth;
    public TextMeshProUGUI playerHealthText;
    private Rigidbody playerRB;
    public bool isSwinging;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    private void Start()
    {
        playerRB = GetComponent<Rigidbody>();
        playerHealth = GetComponent<Health>();
    }

    // Update is called once per frame
    private void Update()
    {
        playerHealthText.text = "" + playerHealth.currentHealth;
        IsOnFloor();
        if(InputController.Instance.GetButtonDown(InputController.InputAction.Jump) && onFloor)
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }
    private void MovePlayer()
    {
        float moveX = InputController.Instance.GetAxis(InputController.InputAction.MoveX);
        float moveZ = InputController.Instance.GetAxis(InputController.InputAction.MoveY);
        Vector3 movement = (transform.right * moveX) + (transform.forward * moveZ);
        if(onFloor)
        {
            Vector3 velocity = movement * speed;
            velocity.y = playerRB.linearVelocity.y;
            playerRB.linearVelocity = velocity;
        }
        else if (isSwinging)
        {
            playerRB.AddForce(movement * swingControlForce, ForceMode.Acceleration);
        }
        else
        {
            playerRB.AddForce(movement * airControlForce, ForceMode.Acceleration);//this is going to work when the player let go the hook
        }
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
