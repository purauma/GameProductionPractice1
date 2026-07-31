using System.Collections;
using UnityEngine;

public class Thwomp : MonoBehaviour
{
    [Header("落下設定")]
    [SerializeField] private float fallSpeed = 20f;

    [Header("戻る速度")]
    [SerializeField] private float returnSpeed = 5f;

    [Header("停止時間")]
    [SerializeField] private float waitTime = 1f;


    [Header("落下する位置")]
    [SerializeField] private Transform groundPoint;


    private Vector3 startPosition;

    private bool isMoving = false;


    private void Start()
    {
        startPosition = transform.position;
    }


    public void Activate()
    {
        if (!isMoving)
        {
            StartCoroutine(Slam());
        }
    }


    private IEnumerator Slam()
    {
        isMoving = true;


        // 落下
        while (transform.position.y > groundPoint.position.y)
        {
            Debug.Log(
                "現在Y:" + transform.position.y +
                " 目標Y:" + groundPoint.position.y
            );

            transform.position = Vector3.MoveTowards(
                transform.position,
                groundPoint.position,
                fallSpeed * Time.deltaTime
            );

            yield return null;
        }


        // 必ず床位置に固定
        transform.position = groundPoint.position;


        yield return new WaitForSeconds(waitTime);


        // 戻る
        while (transform.position.y < startPosition.y)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                startPosition,
                returnSpeed * Time.deltaTime
            );

            yield return null;
        }


        // 必ず元位置
        transform.position = startPosition;


        isMoving = false;
    }
}