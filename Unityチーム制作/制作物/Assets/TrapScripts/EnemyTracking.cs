using UnityEngine;
using UnityEngine.AI;

public class EnemyTracking : MonoBehaviour
{
    public Transform target;
    public float range = 10f;

    private NavMeshAgent agent;
    private Vector3 startPosition;
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        startPosition = transform.position; // 元の位置を記録
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, target.position);

        if (distance <= range)
        {
            // プレイヤーを追跡
            agent.SetDestination(target.position);
            anim.SetBool("islock", true);

            
        }
        else
        {
            // 元の位置へ戻る
            agent.SetDestination(startPosition);
            anim.SetBool("islock", false);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
