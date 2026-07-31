using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();

        if (player != null)
        {
            player.SetRespawnPoint(transform.position);
            Debug.Log("チェックポイント更新");
        }
    }
}