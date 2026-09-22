using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected int damage = 10;
    [SerializeField] protected float damageCooldown = 2; 
    [SerializeField] protected float currentCooldown;
    [SerializeField] protected Health health;
    [SerializeField] protected bool canAttack = true;

    protected virtual void Start()
    {
        currentCooldown = damageCooldown;
    }

    protected virtual void Update()
    {
        if(currentCooldown > 0f)
        {
            currentCooldown -= Time.deltaTime;
        }
        else
        {
            canAttack = true;
        }
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            health = collision.gameObject.GetComponent<Health>();
            if(canAttack)
            {
                DealDamage(health);
            }
        }
    }

    protected virtual void DealDamage(Health health)
    {
        health.TakeDamage(damage);
        canAttack = false;
        currentCooldown = damageCooldown;
    }
}
