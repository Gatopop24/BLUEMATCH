using UnityEngine;
using UnityEngine.AI;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        FindPlayer();
    }

    // Update is called once per frame
    private void Update()
    {
        if(player == null)
        {
            FindPlayer();
            return;
        }
        agent.SetDestination(player.position);
    }

    private void FindPlayer()
    {
        PlayerController playerController = FindObjectOfType<PlayerController>();
        if (playerController != null)
        {
            player = playerController.transform;
        }
    }
}
