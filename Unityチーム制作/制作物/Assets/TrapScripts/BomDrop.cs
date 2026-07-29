using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class BomDrop : MonoBehaviour
{
    public GameObject bombPrefab;
    public float interval = 2f;
    public float backOffset = 1f;

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(DropBombLoop());
    }

    IEnumerator DropBombLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);

            // Œ»İ‚ÌˆÚ“®•ûŒü
            Vector3 moveDirection = agent.velocity.normalized;

            // “®‚¢‚Ä‚¢‚È‚¢ê‡‚Í‰½‚à‚µ‚È‚¢
            if (moveDirection.magnitude < 0.1f)
                continue;

            // Œã‚ë‘¤‚ÌˆÊ’u‚ğŒvZ
            Vector3 dropPosition = transform.position - moveDirection * backOffset;

            Instantiate(bombPrefab, dropPosition, Quaternion.identity);
        }
    }
}