using UnityEngine;

public class DashFloor : MonoBehaviour
{
    [SerializeField] 
    private float speed = 15f;

    private void OnCollisionStay(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

        Rigidbody rb = collision.rigidbody;

        Vector3 dir = transform.forward.normalized;

        rb.AddForce(dir * speed, ForceMode.Acceleration);
    }
}