using UnityEngine;

public class WoodSpawner : MonoBehaviour
{
    public GameObject woodTrapPrefab;
    public float spawnInterval = 3f;

    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            timer = 0f;
            Instantiate(woodTrapPrefab, transform.position, transform.rotation);
        }
    }
}
