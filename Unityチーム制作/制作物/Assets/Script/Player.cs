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
    [SerializeField] private float fallDeathY = -10f;
    [SerializeField] private Transform cameraTransform;

    private Vector3 respawnPoint;

    private bool IsGround = false;
    private bool isInvincible = false;
    private bool isDead = false;

    private float invincibleTime = 1.0f;

    private Renderer playerRenderer;

    private CharacterController controller;
    private Rigidbody rb;

    // CharacterController用
    private float gravity = -15f;
    private float jumpPower = 7f;
    private float verticalVelocity = 0f;
    private Vector3 launchVelocity = Vector3.zero;


    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();

        respawnPoint = transform.position;

        playerRenderer = GetComponentInChildren<Renderer>();

        rb = GetComponent<Rigidbody>();
      
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

        // 接地判定
        IsGround = controller.isGrounded;

        if (IsGround && verticalVelocity < 0)
        {
            verticalVelocity = -1f;
        }

        Vector3 move = Vector3.zero;

        if (!isDead)
        {
            // ジャンプ入力受付
            if (Input.GetButtonDown("Jump"))
            {
                jumpBufferCounter = jumpBufferTime;
            }


            if (jumpBufferCounter > 0)
            {
                jumpBufferCounter -= Time.deltaTime;
            }

            // ジャンプ
            if (jumpBufferCounter > 0 && IsGround)
            {
                Jump();
                jumpBufferCounter = 0;
            }


            move = Move();

        }



        // 重力
        verticalVelocity += gravity * Time.deltaTime;

        // 吹っ飛び速度を加算
        move += launchVelocity;

        move.y = verticalVelocity;

        controller.Move(move * Time.deltaTime);



        // 吹っ飛び速度を徐々に減らす
        launchVelocity = Vector3.Lerp(
            launchVelocity,
            Vector3.zero,
            8f * Time.deltaTime);

        // 落下したらリスポーン
        if (!isDead && transform.position.y <= fallDeathY)
        {
            StartCoroutine(DeathProcess());
        }


    }



    private Vector3 Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        //Debug.Log($"x={x} z={z}");

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
        Debug.Log("ダメージ処理開始");

        if (isInvincible || isDead)
        {
            Debug.Log("無敵または死亡中");
            return;
        }

        PlayerHp -= damage;

        Debug.Log("HP:" + PlayerHp);

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

        if (hit.gameObject.CompareTag("Take Damage"))
        {
            TakeDamage(1);
        }

        if (hit.gameObject.CompareTag("FallingFloor"))
        {
            FallingFloor floor = hit.gameObject.GetComponent<FallingFloor>();
            if (floor != null)
            {
                floor.TriggerFall();
            }
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger Enter: " + other.gameObject.name + " / Tag: " + other.tag);

        if (other.CompareTag("Goal"))
        {
            SceneManager.LoadScene("EndScene");
        }

        if (other.CompareTag("Take Damage"))
        {
            TakeDamage(1);
        }

        if (other.CompareTag("CheckPoint"))
        {
            respawnPoint = other.transform.position + new Vector3(0, 1.0f, -2.0f);

            Debug.Log("リスポーン地点更新");
        }

        if (other.CompareTag("BounceTrap"))
        {
            Debug.Log("Bounce Trap Hit!");
            Launch(Vector3.up, 30f);
        }
    }

    private IEnumerator DeathProcess()
    {
        isDead = true;

        anim.SetTrigger("Death");

        verticalVelocity = 0;


        yield return new WaitForSeconds(1.0f);


        Respawn();


        if (StageResetManager.Instance != null)
        {
            StageResetManager.Instance.ResetStage();
        }


        isDead = false;
    }



    private void Respawn()
    {
        Debug.Log("リスポーン実行");

        DeathMarkerManager.Instance.CreateMarker(transform.position);

        PlayerHp = 1;

        controller.enabled = false;

        transform.position = respawnPoint;

        controller.enabled = true;

        verticalVelocity = 0;
        anim.ResetTrigger("Death");
        anim.Play("Idle", 0, 0f);
    }

    public void SetRespawnPoint(Vector3 point)
    {
        respawnPoint = point;
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

    public void Launch(Vector3 direction, float power)
    {
        direction.Normalize();

        // 横方向
        Vector3 horizontal = new Vector3(direction.x, 0, direction.z);

        // 飛ぶ速度を一時的に保存
        launchVelocity = horizontal * power;

        // 縦方向
        verticalVelocity = direction.y * power;
    }
}