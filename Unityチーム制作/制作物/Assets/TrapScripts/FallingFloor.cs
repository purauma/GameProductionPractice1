using System.Collections;
using UnityEngine;

/// <summary>
/// プレイヤーが乗ると一定時間後に崩れ落ちる床。
/// CharacterController を使う Player.cs 側は OnControllerColliderHit で
/// 判定しているため、こちらも同じ仕組みで衝突を受け取る。
///
[RequireComponent(typeof(Collider))]
public class FallingFloor : MonoBehaviour
{
    [Header("タイミング設定")]
    [Tooltip("乗ってから崩れ落ちるまでの時間(秒)")]
    [SerializeField] private float delayBeforeFall = 1.0f;

    [Tooltip("崩れてから復活するまでの時間(秒)。0以下なら復活しない")]
    [SerializeField] private float respawnDelay = 3.0f;

    [Header("揺れ演出")]
    [SerializeField] private bool shakeBeforeFall = true;
    [SerializeField] private float shakeStrength = 0.05f;

    [Header("落下演出")]
    [Tooltip("trueなら物理落下、falseなら即座に非表示")]
    [SerializeField] private bool physicalFall = true;
    [SerializeField] private float fallGravityScale = 2.0f;
    [SerializeField] private float destroyAfterFallTime = 2.0f;

    private bool isTriggered = false;
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Collider floorCollider;
    private Renderer floorRenderer;
    private Rigidbody rb;

    private void Awake()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;

        floorCollider = GetComponent<Collider>();
        floorRenderer = GetComponent<Renderer>();

        rb = GetComponent<Rigidbody>();
        if (physicalFall && rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
        if (rb != null)
        {
            rb.isKinematic = true; // 落ちる瞬間まで固定
        }
    }

    /// <summary>
    /// 外部(プレイヤー側)から呼び出すためのトリガー。
    /// CharacterControllerを使うプレイヤーだと OnControllerColliderHit は
    /// プレイヤー側にしか発火しないため、この床側の判定は使えない。
    /// Player.cs の OnControllerColliderHit / OnTriggerEnter からタグ判定して
    /// このメソッドを呼び出すこと。
    /// </summary>
    public void TriggerFall()
    {
        if (isTriggered) return;
        isTriggered = true;
        StartCoroutine(FallSequence());
    }

    private IEnumerator FallSequence()
    {
        // 1. 警告(揺れ)
        if (shakeBeforeFall)
        {
            float timer = 0f;
            while (timer < delayBeforeFall)
            {
                float offsetX = Random.Range(-shakeStrength, shakeStrength);
                float offsetZ = Random.Range(-shakeStrength, shakeStrength);
                transform.position = initialPosition + new Vector3(offsetX, 0, offsetZ);

                timer += Time.deltaTime;
                yield return null;
            }
            transform.position = initialPosition;
        }
        else
        {
            yield return new WaitForSeconds(delayBeforeFall);
        }

        // 2. 崩れる
        if (physicalFall && rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = false; // 独自の重力で落とす(演出調整しやすい)
            StartCoroutine(ApplyFallGravity());
        }
        else
        {
            // 見た目・当たり判定だけ消す(シンプル版)
            if (floorRenderer != null) floorRenderer.enabled = false;
            if (floorCollider != null) floorCollider.enabled = false;
        }

        // 3. 一定時間後に復活 or 削除
        if (respawnDelay > 0f)
        {
            yield return new WaitForSeconds(respawnDelay);
            Respawn();
        }
        else if (physicalFall)
        {
            yield return new WaitForSeconds(destroyAfterFallTime);
            Destroy(gameObject);
        }
    }

    private IEnumerator ApplyFallGravity()
    {
        // 落下中もColliderは残しておくと下の当たり判定を挟める。
        // 不要ならここで floorCollider.enabled = false; してすり抜けさせる。
        float elapsed = 0f;
        while (elapsed < destroyAfterFallTime)
        {
            rb.linearVelocity += Physics.gravity * fallGravityScale * Time.deltaTime;
            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    private void Respawn()
    {
        StopAllCoroutines();

        transform.position = initialPosition;
        transform.rotation = initialRotation;

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        if (floorRenderer != null) floorRenderer.enabled = true;
        if (floorCollider != null) floorCollider.enabled = true;

        isTriggered = false;
    }
}