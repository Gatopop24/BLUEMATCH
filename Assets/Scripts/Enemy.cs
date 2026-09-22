using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected int damage = 10;
    [SerializeField] protected float damageCooldown; 
    [SerializeField] protected float currentCooldown;

    protected virtual void Start()
    {
        //currentCooldown = damageCooldown; for future cooldown
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Health health = collision.gameObject.GetComponent<Health>();
            DealDamage(health);
        }
    }

    protected virtual void DealDamage(Health health)
    {
        health.TakeDamage(damage);
    }
}
