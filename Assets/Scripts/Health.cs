using DamageNumbersPro;
using UnityEngine;


public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] public int currentHealth;
    public DamageNumber damagePopup;
    public bool isDead = false;
    public int MaxHealth
    {
        get { return maxHealth; }
    }

    private void Awake()
    {
        currentHealth = maxHealth;
    }
    
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        damagePopup.Spawn(transform.position + Vector3.up, damage);
        if(currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        Enemy enemy = GetComponent<Enemy>();
        ExplosiveEnemy explosiveEnemy = GetComponent<ExplosiveEnemy>();
        if (enemy != null)
        {
            enemy.GiveCoin();
        }
        if(explosiveEnemy != null)
        {
            explosiveEnemy.Explode();
            explosiveEnemy.GiveCoin();
        }
        gameObject.SetActive(false);
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        isDead = false;
    }
}
