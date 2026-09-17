using UnityEngine;

public class BaseGun : MonoBehaviour
{
    [SerializeField] protected int damage = 10;
    [SerializeField] protected float fireCooldown;
    [SerializeField] protected float currentCooldown;
    [SerializeField] protected float bulletRange;
    [SerializeField] protected Transform playerCamera;
    public bool automatic;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        currentCooldown = fireCooldown;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if(automatic)
        {
            if(InputController.Instance.GetButton(InputController.InputAction.Fire))
            {
                if(currentCooldown <= 0f)
                {
                    Shoot();
                    currentCooldown = fireCooldown;
                }
            }
        }
        else
        {
            if(InputController.Instance.GetButtonDown(InputController.InputAction.Fire))
            {
                if(currentCooldown <= 0f)
                {
                    Shoot();
                    currentCooldown = fireCooldown;
                }
            }
        }
        currentCooldown -= Time.deltaTime;
    }

    protected virtual void Shoot()
    {
        Ray gunRay = new Ray(playerCamera.position, playerCamera.forward);
        if(Physics.Raycast(gunRay, out RaycastHit hit, bulletRange))
        {
            Health health = hit.collider.GetComponent<Health>();
            if(health != null)
            {
                health.TakeDamage(damage);
            }
        }
    }
}
