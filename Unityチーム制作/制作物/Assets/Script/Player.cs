using System.Collections;
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

    [SerializeField]
    public int PlayerHp = 1;

    private Vector3 respawnPoint;


    private float moveY = 0f;

    bool IsGround = false;
    private bool isInvincible = false;
    private float invincibleTime = 1.0f; // 無敵時間（1秒）
    private Renderer playerRenderer;


    private void Awake()
    {
        Debug.Log("Player Awake");
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();   // ★追加
        respawnPoint = transform.position;
        playerRenderer = GetComponentInChildren<Renderer>(); // ★モデルの見た目を取得
    }

    private void Update()
    {
        Vector3 moveXZ = Vector3.zero;

        // --- 回転 ---
        float rotY = transform.localEulerAngles.y;

        if (Input.GetKey(KeyCode.A))
        {
            rotY -= m_RotationSpeed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            rotY += m_RotationSpeed;
        }

        // --- 移動 ---
        if (Input.GetKey(KeyCode.W))
        {
            moveXZ = transform.forward * m_MoveSpeed;
        }

        // --- Animator に移動量を送る（★これが無いと遷移しない） ---
        anim.SetFloat("MoveSpeed", moveXZ.magnitude);

        // --- ジャンプ ---
        if (Input.GetKeyDown(KeyCode.Space) && IsGround)
        {
            moveY = m_JumpPow;
            IsGround = false;   
        }


        // --- Rigidbody に反映 ---
        rb.rotation = Quaternion.Euler(0, rotY, 0);
       
        rb.linearVelocity = new Vector3(moveXZ.x, rb.linearVelocity.y + moveY, moveXZ.z);
    
        moveY = 0f;
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible) return; // ★無敵中はダメージ無効

        PlayerHp -= damage;

        StartCoroutine(DamageBlink()); // ★点滅開始

        if (PlayerHp <= 0)
        {
            Respawn();
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
            TakeDamage(1);   //ダメージを与える
        }
    }


    private void Respawn()
    {
        DeathMarkerManager.Instance.CreateMarker(transform.position);

        PlayerHp = 1;  // HP 回復
        rb.linearVelocity = Vector3.zero;  // 速度リセット
        transform.position = respawnPoint; // 初期位置へ戻す
    }

    private IEnumerator DamageBlink()
    {
        isInvincible = true;

        float blinkInterval = 0.1f;
        float timer = 0f;

        while (timer < invincibleTime)
        {
            playerRenderer.enabled = !playerRenderer.enabled; // ON/OFF 切り替え
            yield return new WaitForSeconds(blinkInterval);
            timer += blinkInterval;
        }

        playerRenderer.enabled = true; // 最後に表示を戻す
        isInvincible = false;
    }




}