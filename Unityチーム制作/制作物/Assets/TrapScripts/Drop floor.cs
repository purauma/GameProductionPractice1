using UnityEngine;

public class Dropfloor : MonoBehaviour
{
    public float fallDelay = 1f;      // �����܂ł̎���
    public float respawnDelay = 3f;   // �Ĕz�u�܂ł̎���

    private Vector3 startPos;         // �����ʒu
    private Quaternion startRot;      // ������]
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;        // �ŏ��͓����Ȃ�
        startPos = transform.position;
        startRot = transform.rotation;
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Invoke("Fall", fallDelay);
        }
    }

    void Fall()
    {
        rb.isKinematic = false;       // �����J�n
        Invoke("Respawn", respawnDelay);
    }

    void Respawn()
    {
        rb.isKinematic = true;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        transform.position = startPos;
        transform.rotation = startRot;
    }
}
