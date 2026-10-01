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
        bullet.transform.position = muzzle.position;
        Vector3 direction = hitPoint - muzzle.position;
        bullet.transform.rotation = Quaternion.LookRotation(direction);
        bullet.SetActive(true);
        StartCoroutine(AnimateBullet(bullet, muzzle.position, hitPoint));
    }

    private IEnumerator AnimateBullet(GameObject bullet,Vector3 start, Vector3 end)
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
        bullet.SetActive(false);
    }
}
