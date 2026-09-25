using UnityEngine;

public class Shotgun : BaseGun
{
    [SerializeField] private int pelletCount = 8;
    [SerializeField] private float spreadAngle = 5f;

    protected override void Shoot()
    {
        TriggerKickback();
        PlayShootSound();
        for (int i = 0; i < pelletCount; i++)
        {
            Vector3 spreadDirection = Spread(playerCamera.forward, spreadAngle);
            Ray gunRay = new Ray(playerCamera.position, spreadDirection);
            Vector3 hitPoint;

            if (Physics.Raycast(gunRay, out RaycastHit hit, bulletRange))
            {
                hitPoint = hit.point;
                Health health = hit.collider.GetComponent<Health>();
                if (health != null)
                {
                    health.TakeDamage(damage);
                }
            }
            else
            {
                hitPoint = gunRay.origin + gunRay.direction * bulletRange;
            }

            SpawnBulletTrail(hitPoint);
        }
    }

    private Vector3 Spread(Vector3 baseDirection, float maxAngle)
    {
        float randomX = Random.Range(-maxAngle, maxAngle);
        float randomY = Random.Range(-maxAngle, maxAngle);

        Quaternion spreadRotation = Quaternion.Euler(randomX, randomY, 0f);
        return spreadRotation * baseDirection;
    }
}
