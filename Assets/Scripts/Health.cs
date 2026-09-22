using UnityEngine;


public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] public int currentHealth;
    public bool isDead = false;

    private void Awake()
    {
        currentHealth = maxHealth;
    }
    
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if(currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        gameObject.SetActive(false);
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        isDead = false;
    }
}
