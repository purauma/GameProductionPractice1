using UnityEngine;

public class MovingGoal : MonoBehaviour, IResettable
{
    [SerializeField] private Transform player;

    [SerializeField] private Transform escapePoint;

    [SerializeField] private float triggerDistance = 5f;
    [SerializeField] private float moveSpeed = 2f;


    private Vector3 startPosition;


    private void Start()
    {
        startPosition = transform.position;

        StageResetManager.Instance.Register(this);
    }


    private void Update()
    {
        float distance = Vector3.Distance(
            player.position,
            transform.position
        );


        // ƒvƒŒƒCƒ„[‚ª”ÍˆÍ“à‚É‚¢‚é‚¾‚¯“¦‚°‚é
        if (distance <= triggerDistance)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                escapePoint.position,
                moveSpeed * Time.deltaTime
            );
        }
    }


    public void ResetObject()
    {
        transform.position = startPosition;
    }
}