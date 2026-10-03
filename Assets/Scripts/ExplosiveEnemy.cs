using UnityEngine;

public class ExplosiveEnemy : Enemy
{
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private float explosionRadius = 5f;
    [SerializeField] private int explosionDamage = 30;

    protected override void Update()
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
            Explode();
            GiveCoin();
        }
    }

    public void Explode()
    {
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                Health playerHealth = collider.GetComponent<Health>();
                playerHealth.TakeDamage(explosionDamage);
            }
        }
        gameObject.SetActive(false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
