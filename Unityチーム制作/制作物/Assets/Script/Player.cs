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
    [SerializeField] private Transform cameraTransform;

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
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        respawnPoint = transform.position;
        playerRenderer = GetComponentInChildren<Renderer>();

        if (anim != null)
        {
            anim.applyRootMotion = false;
        }

        rb.freezeRotation = true;
    }

    private void Update()
    {
        if (isDead) return;

        Move();

        if (Input.GetButtonDown("Jump"))
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
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");


        Vector3 input =
            new Vector3(x, 0, -z);


        if (input.magnitude < 0.1f)
        {
            anim.SetBool("Is Move", false);
            return;
        }


        // カメラの水平回転だけ取得
        float cameraY =
            cameraTransform.eulerAngles.y;


        Quaternion cameraRotation =
            Quaternion.Euler(
                0,
                cameraY,
                0
            );


        // カメラ基準の移動方向
        Vector3 move =
            cameraRotation * input;


        move.Normalize();


        rb.MovePosition(
            rb.position +
            move * MoveSpeed * Time.deltaTime
        );


        // 移動方向へ向きを変える
        Quaternion targetRotation =
            Quaternion.LookRotation(move);


        transform.rotation =
            Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                RotationSpeed * Time.deltaTime
            );


        anim.SetBool("Is Move", true);
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
        rb.linearVelocity = Vector3.zero;

        yield return new WaitForSeconds(2.0f);

        Respawn();
        isDead = false;
    }

    private void Respawn()
    {
        DeathMarkerManager.Instance.CreateMarker(transform.position);

        PlayerHp = 1;
        rb.linearVelocity = Vector3.zero;
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