using UnityEngine;

public class TakeDamage : MonoBehaviour
{
    [SerializeField] private int damage = 1;

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Goal"))
        {
            Player player = hit.gameObject.GetComponent<Player>();

            if (player != null)
            {
                player.TakeDamage(damage);
            }
        }
    }
}