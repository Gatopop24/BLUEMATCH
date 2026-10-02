using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected int damage = 10;
    [SerializeField] protected float damageCooldown = 2; 
    [SerializeField] protected float currentCooldown;
    [SerializeField] protected Health playerHealth;
    [SerializeField] protected Health Health;
    [SerializeField] protected bool canAttack = true;
    [SerializeField] private float minSpeed = 3.5f;
    [SerializeField] private float maxSpeed = 10f;
    private NavMeshAgent agent;

    protected virtual void Start()
    {
        Health = GetComponent<Health>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = Random.Range(minSpeed, maxSpeed);
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
        if (Health.isDead)
        {
            GiveCoin();
        }
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            playerHealth = collision.gameObject.GetComponent<Health>();
            if(canAttack)
            {
                DealDamage(playerHealth);
            }
        }
    }

    protected virtual void DealDamage(Health playerHealth)
    {
        playerHealth.TakeDamage(damage);
        canAttack = false;
        currentCooldown = damageCooldown;
    }

    public void GiveCoin()
    {
        ScoreManager.Instance.AddPoint();
    }
}
