using System.Collections;
using UnityEngine;

public class GunEffects : MonoBehaviour
{
    [SerializeField] private ObjectPooler pooler;
    [SerializeField] private Transform muzzle;
    [SerializeField] private GameObject bulletTrail;
    [SerializeField] private float bulletTrailSpeed = 300f;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip shootSound;

    public void PlayShootSound()
    {
        PlayAudio.PlayClip(audioSource, shootSound);
    }

    public void SpawnBulletTrail(Vector3 hitPoint)
    {
        GameObject trailObj = pooler.GetPooledObject();
        trailObj.transform.position = muzzle.position;
        trailObj.transform.rotation = Quaternion.identity;
        trailObj.SetActive(true);
        LineRenderer line = trailObj.GetComponent<LineRenderer>();

        StartCoroutine(AnimateTrail(line, muzzle.position, hitPoint));
    }

    private IEnumerator AnimateTrail(LineRenderer line, Vector3 start, Vector3 end)
    {
        line.SetPosition(0, start);
        line.SetPosition(1, start);
        float distance = Vector3.Distance(start, end);
        float duration = distance / bulletTrailSpeed;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            Vector3 currentEnd = Vector3.Lerp(start, end, elapsed / duration);
            line.SetPosition(1, currentEnd);
            elapsed += Time.deltaTime;
            yield return null;
        }
        line.SetPosition(1, end);
        yield return new WaitForSeconds(0.05f);
        line.gameObject.SetActive(false);
    }
}
