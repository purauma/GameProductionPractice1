using UnityEngine;

public class TrapFloorTrigger : MonoBehaviour
{
    public Transform trap;     // 動かしたい罠
    public float moveY = 2f;   // 上にどれだけ動くか
    public float speed = 5f;   // 動く速さ
    public float returnDelay = 2f; // ★何秒後に戻すか

    private bool activated = false;
    private bool returning = false;

    private Vector3 startPos;   //元の位置
    private Vector3 targetPos;  // 飛び出す位置

    void Start()
    {
        startPos = trap.position;
        targetPos = startPos + new Vector3(0, moveY, 0);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !activated)
        {
            activated = true;
            Invoke(nameof(StartReturn), returnDelay); //指定秒後に戻す処理を開始
        }
    }

    void StartReturn()
    {
        returning = true;
    }

    void Update()
    {
        if (activated && !returning)
        {
            // 上に飛び出す
            trap.position = Vector3.Lerp(trap.position, targetPos, Time.deltaTime * speed);
        }

        if (returning)
        {
            // 元の位置に戻る
            trap.position = Vector3.Lerp(trap.position, startPos, Time.deltaTime * speed);

            // ほぼ戻ったらリセット
            if (Vector3.Distance(trap.position, startPos) < 0.01f)
            {
                trap.position = startPos;
                activated = false;
                returning = false;
            }
        }
    }
}
