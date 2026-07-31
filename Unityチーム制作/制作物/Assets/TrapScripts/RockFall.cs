using UnityEngine;

public class RockFall : MonoBehaviour, IResettable
{
    private Vector3 startPosition;
    private Quaternion startRotation;

    private Rigidbody rb;


    private void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;

        rb = GetComponent<Rigidbody>();

        StageResetManager.Instance.Register(this);
    }


    public void ResetObject()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        transform.position = startPosition;
        transform.rotation = startRotation;
    }
}