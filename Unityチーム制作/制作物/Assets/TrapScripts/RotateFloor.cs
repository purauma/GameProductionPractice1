using UnityEngine;
using System.Collections;

public class RotateFloor : MonoBehaviour
{
    public float interval = 3f;
    public float rotateTime = 1f;

    private bool rotating = false;


    void Start()
    {
        InvokeRepeating(nameof(Flip), interval, interval);
    }


    void Flip()
    {
        if (!rotating)
        {
            StartCoroutine(FlipFloor());
        }
    }


    IEnumerator FlipFloor()
    {
        rotating = true;


        Quaternion start = transform.rotation;

        // åªç›ÇÃäpìxÇ©ÇÁXé≤180ìxâÒì]
        Quaternion end = start * Quaternion.Euler(180, 0, 0);


        float time = 0;

        while (time < rotateTime)
        {
            time += Time.deltaTime;

            transform.rotation =
                Quaternion.Slerp(
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