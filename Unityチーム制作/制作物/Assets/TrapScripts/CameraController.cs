using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public float fadeAlpha = 0.3f;
    public float fadeSpeed = 5f;

    private List<Renderer> currentObstacles = new List<Renderer>();
    private Dictionary<Renderer, Material[]> originalMaterials = new Dictionary<Renderer, Material[]>();

    void Update()
    {
        HandleObstacles();
    }

    void HandleObstacles()
    {
        //まず前回の透明化を解除
        foreach (var r in currentObstacles)
        {
            RestoreMaterial(r);
        }
        currentObstacles.Clear();

        //Raycast で遮蔽物を検出
        Vector3 dir = player.position - transform.position;
        float dist = Vector3.Distance(player.position, transform.position);

        RaycastHit[] hits = Physics.RaycastAll(transform.position, dir, dist);

        foreach (var hit in hits)
        {
            //  Wall 以外なら無視
            if (!hit.collider.CompareTag("Wall")) continue;

            Renderer rend = hit.collider.GetComponent<Renderer>();
            if (rend == null) continue;

            if (hit.collider.transform == player) continue;

            // 透明化
            FadeMaterial(rend);
            currentObstacles.Add(rend);
        }

    }

    void FadeMaterial(Renderer rend)
    {
        if (!originalMaterials.ContainsKey(rend))
        {
            originalMaterials[rend] = rend.materials;
        }

        foreach (var mat in rend.materials)
        {
            SetMaterialTransparent(mat);

            Color c = mat.color;
            c.a = Mathf.Lerp(c.a, fadeAlpha, Time.deltaTime * fadeSpeed);
            mat.color = c;
        }
    }

    void RestoreMaterial(Renderer rend)
    {
        if (!originalMaterials.ContainsKey(rend)) return;

        Material[] mats = rend.materials;
        foreach (var mat in mats)
        {
            SetMaterialOpaque(mat);

            Color c = mat.color;
            c.a = Mathf.Lerp(c.a, 1f, Time.deltaTime * fadeSpeed);
            mat.color = c;
        }
    }

    // 透明モードへ
    void SetMaterialTransparent(Material mat)
    {
        mat.SetFloat("_Surface", 1); // Transparent
        mat.SetFloat("_Blend", 0);   // Alpha blending
        mat.SetFloat("_SrcBlend", (float)UnityEngine.Rendering.BlendMode.SrcAlpha);
        mat.SetFloat("_DstBlend", (float)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        mat.SetFloat("_ZWrite", 0);  // ZWrite Off

        mat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
    }


    // 不透明モードへ
    void SetMaterialOpaque(Material mat)
    {
        mat.SetFloat("_Surface", 0); // Opaque
        mat.SetFloat("_Blend", 0);
        mat.SetFloat("_SrcBlend", (float)UnityEngine.Rendering.BlendMode.One);
        mat.SetFloat("_DstBlend", (float)UnityEngine.Rendering.BlendMode.Zero);
        mat.SetFloat("_ZWrite", 1);  // ZWrite On

        mat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Geometry;
    }

}
