using UnityEngine;

public class WoodRollingTrap : MonoBehaviour
{
    public float rotationSpeed = 30f;
    public float moveDistance = 5f;
    public float moveSpeed = 2f;

    private Vector3 startPos;
    private Vector3 moveDir;
    private float moved = 0f;

    void Start()
    {
        startPos = transform.position;
        moveDir = transform.forward;
    }

    void Update()
    {
        // ‰ñ“]
        transform.Rotate(rotationSpeed * Time.deltaTime, 0f, 0f);

        // ˆÚ“®—Ê‚ð‰ÁŽZ
        float delta = moveSpeed * Time.deltaTime;
        moved += delta;

        // Žw’è‹——£‚Ü‚ÅˆÚ“®
        if (moved < moveDistance)
        {
            transform.position += moveDir * delta;
        }
        else
        {
            // I“_‚É“ž’B ¨ Ž©•ª‚ðíœ
            Destroy(gameObject);
        }
    }
}
