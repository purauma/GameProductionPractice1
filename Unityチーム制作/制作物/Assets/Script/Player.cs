using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody rb;
    private Animator anim;

    [SerializeField]
    private float m_MoveSpeed = 5.0f;

    [SerializeField]
    private float m_RotationSpeed = 5.0f;

    [SerializeField]
    private float m_JumpPow = 5.0f;

    private float moveY = 0f;

    private void Awake()
    {
        Debug.Log("Player Awake");
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();   // š’Ç‰Á
    }

    private void Update()
    {
        Vector3 moveXZ = Vector3.zero;

        // --- ‰ñ“] ---
        float rotY = transform.localEulerAngles.y;

        if (Input.GetKey(KeyCode.A))
        {
            rotY -= m_RotationSpeed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            rotY += m_RotationSpeed;
        }

        // --- ˆÚ“® ---
        if (Input.GetKey(KeyCode.W))
        {
            moveXZ = transform.forward * m_MoveSpeed;
        }

        // --- Animator ‚ÉˆÚ“®—Ê‚ğ‘—‚éiš‚±‚ê‚ª–³‚¢‚Æ‘JˆÚ‚µ‚È‚¢j ---
        anim.SetFloat("MoveSpeed", moveXZ.magnitude);

        // --- ƒWƒƒƒ“ƒv ---
        if (Input.GetKeyDown(KeyCode.Space))
        {
            moveY = m_JumpPow;
        }

        // --- Rigidbody ‚É”½‰f ---
        rb.rotation = Quaternion.Euler(0, rotY, 0);
        rb.linearVelocity = new Vector3(moveXZ.x, rb.linearVelocity.y + moveY, moveXZ.z);

        moveY = 0f;
    }
}