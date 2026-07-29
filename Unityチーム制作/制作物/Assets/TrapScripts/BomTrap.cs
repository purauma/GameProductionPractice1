using System.Collections;
using UnityEngine;

public class BomTrap : MonoBehaviour
{
    [SerializeField] private float explosionRange = 3f;
    [SerializeField] private float blinkInterval = 0.3f;
    [SerializeField] private int blinkCount = 3;
    [SerializeField] private int damage = 1;

    [SerializeField] private GameObject explosionVFX;

    private Renderer bombRenderer;
    private Color originalColor;

    private void Start()
    {
        bombRenderer = GetComponent<Renderer>();
        originalColor = bombRenderer.material.color;

        StartCoroutine(BombRoutine());
    }

    IEnumerator BombRoutine()
    {
        // 赤く3回点滅
        for (int i = 0; i < blinkCount; i++)
        {
            bombRenderer.material.color = Color.red;
            yield return new WaitForSeconds(blinkInterval);

            bombRenderer.material.color = originalColor;
            yield return new WaitForSeconds(blinkInterval);
        }

        Explode();
    }

    void Explode()
    {
        // 爆発エフェクト生成
        GameObject effect = Instantiate(explosionVFX, transform.position, Quaternion.identity);

        // エフェクトの長さ分だけ待って削除
        ParticleSystem ps = effect.GetComponent<ParticleSystem>();

        if (ps != null)
        {
            Destroy(effect, ps.main.duration + ps.main.startLifetime.constantMax);
        }
        else
        {
            // ParticleSystemがない場合の保険
            Destroy(effect, 2f);
        }

        // 範囲内のPlayerを探す
        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRange);

        foreach (Collider hit in hits)
        {
            Player player = hit.GetComponent<Player>();

            if (player != null)
            {
                player.TakeDamage(damage);
            }
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRange);
    }
}