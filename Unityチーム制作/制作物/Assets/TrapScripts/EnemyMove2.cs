using UnityEngine;

public class EnemyMove2 : MonoBehaviour, IResettable
{
    [SerializeField] private float moveSpeed = 3f;

    // å¸Ç©Ç§ï˚å¸
    [SerializeField] private Vector3 moveDirection = Vector3.forward;

    private Vector3 startPosition;
    private Quaternion startRotation;

    private Rigidbody rb;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        startPosition = transform.position;
        startRotation = transform.rotation;

        StageResetManager.Instance.Register(this);

        moveDirection.Normalize();
    }


    private void FixedUpdate()
    {
        Vector3 velocity = rb.linearVelocity;

        // â°à⁄ìÆÇæÇØïœçX
        velocity.x = moveDirection.x * moveSpeed;
        velocity.z = moveDirection.z * moveSpeed;

        rb.linearVelocity = velocity;
    }


    public void ResetObject()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        transform.position = startPosition;
        transform.rotation = startRotation;
    }
}