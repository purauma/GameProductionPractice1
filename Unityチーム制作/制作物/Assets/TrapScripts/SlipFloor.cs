using UnityEngine;

public class IceFloor : MonoBehaviour
{
    public float force = 15f;

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();

            Vector3 dir = transform.forward;

            rb.AddForce(dir * force);
        }
    }
}