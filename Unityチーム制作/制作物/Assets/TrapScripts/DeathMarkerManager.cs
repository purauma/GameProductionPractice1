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
        // XŽ²90“x‰ñ“]‚Å¶¬
        GameObject obj = Instantiate(
            markerPrefab,
            pos,
            Quaternion.Euler(90f, 0f, 0f)
        );

        markers.Enqueue(obj);

        // 30ŒÂ‚ð’´‚¦‚½‚çŒÃ‚¢‚à‚Ì‚©‚çíœ
        if (markers.Count > MAX_MARKERS)
        {
            GameObject old = markers.Dequeue();
            Destroy(old);
        }
    }
}
