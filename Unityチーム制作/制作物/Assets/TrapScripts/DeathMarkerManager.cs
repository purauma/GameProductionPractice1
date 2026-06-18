using System.Collections.Generic;
using UnityEngine;

public class DeathMarkerManager : MonoBehaviour
{
    public static DeathMarkerManager Instance;

    [SerializeField] private GameObject markerPrefab;
    private Queue<GameObject> markers = new Queue<GameObject>();

    private const int MAX_MARKERS = 30;

    private void Awake()
    {
        Instance = this;
    }

    public void CreateMarker(Vector3 pos)
    {
        // 新しいマーカー生成
        GameObject obj = Instantiate(markerPrefab, pos, Quaternion.identity);
        markers.Enqueue(obj);

        // 30個を超えたら古いものから削除
        if (markers.Count > MAX_MARKERS)
        {
            GameObject old = markers.Dequeue();
            Destroy(old);
        }
    }
}
