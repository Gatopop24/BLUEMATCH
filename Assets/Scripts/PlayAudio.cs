using UnityEngine;

public class PlayAudio : MonoBehaviour
{
    public static void PlayClip(AudioSource source, AudioClip clip)
    {
        if (source == null || clip == null) return;
        source.Stop();
        source.clip = clip;
        source.Play();
    }
}
