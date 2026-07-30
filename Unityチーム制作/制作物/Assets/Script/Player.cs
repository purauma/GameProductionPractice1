using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
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
    private bool isDead = false;

    private float invincibleTime = 1.0f;

    private Renderer playerRenderer;

    private CharacterController controller;

    // CharacterController用
    private float gravity = -20f;
    private float jumpPower = 7f;
    private float verticalVelocity = 0f;


    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();

        respawnPoint = transform.position;

        playerRenderer = GetComponentInChildren<Renderer>();

        if (anim != null)
        {
            anim.applyRootMotion = false;
        }
    }


    private void Start()
    {
        IsGround = true;
    }


    private void Update()
    {
        if (isDead) return;


        // ジャンプ入力受付
        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
        }


        if (jumpBufferCounter > 0)
        {
            jumpBufferCounter -= Time.deltaTime;
        }


        // 接地判定
        IsGround = controller.isGrounded;


        if (IsGround && verticalVelocity < 0)
        {
            verticalVelocity = -1f;
        }


        // ジャンプ
        if (jumpBufferCounter > 0 && IsGround)
        {
            Jump();
            jumpBufferCounter = 0;
        }


        // 重力
        verticalVelocity += gravity * Time.deltaTime;


        Vector3 move = Move();

        move.y = verticalVelocity;

        controller.Move(move * Time.deltaTime);
    }



    private Vector3 Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");


        Vector3 input = new Vector3(x, 0, -z);


        Vector3 targetVelocity = Vector3.zero;


        float cameraY = cameraTransform.eulerAngles.y;

        Quaternion cameraRotation =
            Quaternion.Euler(0, cameraY, 0);


        if (input.magnitude >= 0.1f)
        {
            input.Normalize();


            targetVelocity =
                cameraRotation * input * MoveSpeed;


            Quaternion targetRotation =
                Quaternion.LookRotation(targetVelocity);


            transform.rotation =
                Quaternion.Slerp(
                    transform.rotation,
                    targetRotation,
                    RotationSpeed * Time.deltaTime
                );


            anim.SetBool("Is Move", true);
        }
        else
        {
            anim.SetBool("Is Move", false);
        }


        return targetVelocity;
    }



    private void Jump()
    {
        verticalVelocity = jumpPower;

        anim.SetTrigger("Jump");
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



    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Goal"))
        {
            SceneManager.LoadScene("EndScene");
        }
    }



    private IEnumerator DeathProcess()
    {
        isDead = true;

        anim.SetTrigger("Death");

        verticalVelocity = 0;


        yield return new WaitForSeconds(2.0f);


        Respawn();

        isDead = false;
    }



    private void Respawn()
    {
        DeathMarkerManager.Instance.CreateMarker(transform.position);


        PlayerHp = 1;

        controller.enabled = false;

        transform.position = respawnPoint;

        controller.enabled = true;


        verticalVelocity = 0;
    }



    private IEnumerator DamageBlink()
    {
        isInvincible = true;


        float blinkInterval = 0.1f;

        float timer = 0f;


        while (timer < invincibleTime)
        {
            playerRenderer.enabled =
                !playerRenderer.enabled;


            yield return new WaitForSeconds(blinkInterval);


            timer += blinkInterval;
        }


        playerRenderer.enabled = true;

        isInvincible = false;
    }
}