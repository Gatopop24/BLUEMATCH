using System.Collections;
using UnityEngine;

public class GunEffects : MonoBehaviour
{
    [SerializeField] private ObjectPooler pooler;
    [SerializeField] private Transform muzzle;
    [SerializeField] private float bulletSpeed = 300f;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip shootSound;

    public void PlayShootSound()
    {
        PlayAudio.PlayClip(audioSource, shootSound);
    }

    public void SpawnBulletTrail(Vector3 hitPoint)
    {
        GameObject bullet = pooler.GetPooledObject();
        if (bullet == null)
        {
            return;
        }
        TrailRenderer trail = bullet.GetComponent<TrailRenderer>();
        if (trail != null)
        {
            trail.emitting = false;
            trail.Clear();
        }
        Vector3 startPosition = muzzle.position;
        bullet.transform.position = startPosition;
        Vector3 direction = hitPoint - startPosition;
        bullet.transform.rotation = Quaternion.LookRotation(direction);
        bullet.SetActive(true);
        if (trail != null)
        {
            trail.Clear();
            trail.emitting = true;
        }
        StartCoroutine(AnimateBullet(bullet, startPosition, hitPoint));
    }

    private IEnumerator AnimateBullet(GameObject bullet, Vector3 start, Vector3 end)
    {
        float distance = Vector3.Distance(start, end);
        float duration = distance / bulletSpeed;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            bullet.transform.position = Vector3.Lerp(start, end, t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        bullet.transform.position = end;
        TrailRenderer trail = bullet.GetComponent<TrailRenderer>();
        if (trail != null)
        {
            trail.emitting = false;
        }
        bullet.SetActive(false);
        if (trail != null)
        {
            trail.Clear();
        }
    }
}
