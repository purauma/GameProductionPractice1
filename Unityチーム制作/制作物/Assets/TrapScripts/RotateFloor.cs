using UnityEngine;

public class RotateFloor: MonoBehaviour
{
    public float interval = 3f;   // ‰½•b‚²‚Æ‚É”½“]‚·‚é‚©
    public float rotateTime = 1f; // ‰ñ“]‚É‚©‚¯‚éŽžŠÔ

    private bool flipped = false;
    private bool rotating = false;

    void Start()
    {
        InvokeRepeating("Flip", interval, interval);
    }

    void Flip()
    {
        if (!rotating)
        {
            flipped = !flipped;
            StartCoroutine(FlipFloor());
        }
    }

    System.Collections.IEnumerator FlipFloor()
    {
        rotating = true;

        Quaternion start = transform.rotation;
        Quaternion end;

        if (flipped)
            end = Quaternion.Euler(180, 0, 0);
        else
            end = Quaternion.Euler(0, 0, 0);

        float time = 0;

        while (time < rotateTime)
        {
            time += Time.deltaTime;
            transform.rotation = Quaternion.Slerp(
                start,
                end,
                time / rotateTime
            );

            yield return null;
        }

        transform.rotation = end;
        rotating = false;
    }
}