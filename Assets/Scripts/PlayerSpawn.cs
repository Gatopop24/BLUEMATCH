using System.Collections;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Health playerHealth;
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private float respawnDelay = 3f;
    [SerializeField] private PlayerController playerController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        StartCoroutine(WatchPlayer());
    }

    private IEnumerator WatchPlayer()
    {
        while (true)
        {
            while (playerHealth.isDead == false)
            {
                yield return null;
            }

            playerController.enabled = false; 

            yield return new WaitForSeconds(respawnDelay);

            Respawn();
        }
    }

    private void Respawn()
    {
        player.transform.position = respawnPoint.position;
        playerHealth.ResetHealth();
        player.SetActive(true);
        playerController.enabled = true;
    }
}
