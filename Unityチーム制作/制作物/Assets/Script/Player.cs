using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody rb;
    private Animator anim;

    [SerializeField] private float MoveSpeed = 3.0f;
    [SerializeField] private float RotationSpeed = 180f;

    [SerializeField] public int PlayerHp = 1;

    [SerializeField] private float jumpBufferTime = 0.2f; 
    private float jumpBufferCounter = 0f;


    private Vector3 respawnPoint;
    private bool IsGround = false;
    private bool isInvincible = false;
    private float invincibleTime = 1.0f;
    private Renderer playerRenderer;

    private bool isDead = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        respawnPoint = transform.position;
        playerRenderer = GetComponentInChildren<Renderer>();

        anim.applyRootMotion = false;
        rb.freezeRotation = true; // •¨—‚Å“|‚ê‚È‚¢‚æ‚¤‚É
    }

    private void Update()
    {
        if (isDead) return;

        Move();
        Rotate();

    
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpBufferCounter = jumpBufferTime;
        }

      
        if (jumpBufferCounter > 0)
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if (jumpBufferCounter > 0 && IsGround)
        {
            Jump();
            jumpBufferCounter = 0; 
        }
    }


    private void Move()
    {
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.forward * z * MoveSpeed * Time.deltaTime;
        rb.MovePosition(rb.position + move);

        anim.SetBool("Is Move", Mathf.Abs(z) > 0.1f);
    }

    private void Rotate()
    {
        float x = Input.GetAxis("Horizontal");

        Quaternion deltaRot = Quaternion.Euler(0, x * RotationSpeed * Time.deltaTime, 0);
        rb.MoveRotation(rb.rotation * deltaRot);
    }

    private void Jump()
    {
        rb.AddForce(Vector3.up * 5f, ForceMode.Impulse);
        anim.SetTrigger("Jump");
        IsGround = false;
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible || isDead) return;

        PlayerHp -= damage;
        StartCoroutine(DamageBlink());

        if (PlayerHp <= 0)
        {
            StartCoroutine(DeathProcess());
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            IsGround = true;
        }

        if (collision.gameObject.CompareTag("Take Damage"))
        {
            TakeDamage(1);
        }
    }

    private IEnumerator DeathProcess()
    {
        isDead = true;

        anim.SetTrigger("Death");
        rb.velocity = Vector3.zero;

        yield return new WaitForSeconds(2.0f);

        Respawn();
        isDead = false;
    }

    private void Respawn()
    {
        DeathMarkerManager.Instance.CreateMarker(transform.position);

        PlayerHp = 1;
        rb.velocity = Vector3.zero;
        transform.position = respawnPoint;
    }

    private IEnumerator DamageBlink()
    {
        isInvincible = true;

        float blinkInterval = 0.1f;
        float timer = 0f;

        while (timer < invincibleTime)
        {
            playerRenderer.enabled = !playerRenderer.enabled;
            yield return new WaitForSeconds(blinkInterval);
            timer += blinkInterval;
        }

        playerRenderer.enabled = true;
        isInvincible = false;
    }
}
