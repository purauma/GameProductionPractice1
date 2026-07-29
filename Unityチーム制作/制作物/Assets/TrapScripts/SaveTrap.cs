using UnityEngine;

public class SaveTrap : MonoBehaviour
{
    [SerializeField]
    private float range = 1f;

    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= range)
        {
            //ƒŒƒ“ƒW“à‚É“ü‚Á‚½‚ç0.1.0‚É–ß‚³‚ê‚é
            //’†ŠÔ’Ç‰Á‚µ‚½‚ç‘‚«Š·‚¦‚é
            player.position = new Vector3(0, 1, 0);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}