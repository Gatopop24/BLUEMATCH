using System.Collections;
using UnityEngine;


public class DeathCamera : MonoBehaviour
{
    [SerializeField] private float riseHeight = 3f;
    [SerializeField] private float pullBack = 2f;
    [SerializeField] private float lookDown = 60f;
    [SerializeField] private float duration = 2.5f;

    private Coroutine routine;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void Play(Vector3 startPosition, Quaternion startRotation)
    {
        transform.SetPositionAndRotation(startPosition, startRotation);
        gameObject.SetActive(true);

        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(Animate(startPosition, startRotation));
    }

    public void Stop()
    {
        if (routine != null) StopCoroutine(routine);
        routine = null;
        gameObject.SetActive(false);
    }

    private IEnumerator Animate(Vector3 startPos, Quaternion startRot)
    {
        Vector3 endPos = startPos + Vector3.up * riseHeight - startRot * Vector3.forward * pullBack;
        Quaternion endRot = Quaternion.Euler(lookDown, startRot.eulerAngles.y, 0f);

        float elapsed = 0f;
        while (elapsed < duration)
        {
            float t = Mathf.SmoothStep(0f, 1f, elapsed / duration);
            transform.position = Vector3.Lerp(startPos, endPos, t);
            transform.rotation = Quaternion.Slerp(startRot, endRot, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.SetPositionAndRotation(endPos, endRot);
    }
}