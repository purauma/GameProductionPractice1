using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public float moveDistance = 3f;
    public float speed = 2f;

    public Vector3 moveDirection = Vector3.right; // 移動方向

    private Vector3 startPos;
    private int direction = 1;

    void Start()
    {
        startPos = transform.position;
        moveDirection.Normalize();
    }

    void Update()
    {
        // 指定した方向へ移動
        transform.position += moveDirection * speed * direction * Time.deltaTime;

        // 移動距離チェック
        float distance = Vector3.Distance(startPos, transform.position);

        if (distance > moveDistance)
        {
            direction *= -1;
            Turn();
        }
    }

    void Turn()
    {
        // 向きを反転
        transform.Rotate(0, 180, 0);
    }
}