using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
 
    [SerializeField] 
    private Vector3 launchDirection = new Vector3(0, 1, 1);
    [SerializeField] 
    private float launchPower = 10f;

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        { 
            rb.linearVelocity = Vector3.zero;

            // �w������ɔ�΂�
            rb.AddForce(launchDirection.normalized * launchPower, ForceMode.VelocityChange);
        }
    }
}
