using System.Collections.Generic;
using UnityEngine;


public class CameraController : MonoBehaviour
{
    [Header("í«è]")]
    public Transform player;
    public Transform cameraTarget;

    [SerializeField] float distance = 6f;
    [SerializeField] float height = 2.5f;
    [SerializeField] float followSpeed = 10f;



    [Header("âÒì]")]
    [SerializeField] float lookSpeed = 180f;
    [SerializeField] float smooth = 8f;

    [SerializeField] float minPitch = -25f;
    [SerializeField] float maxPitch = 55f;


    float yaw;
    float pitch;



    [Header("ï«ìßñæâª")]
    public float fadeAlpha = 0.25f;
    public float fadeSpeed = 8f;


    List<Renderer> walls =
        new List<Renderer>();



    Dictionary<Renderer, Material[]> save =
        new Dictionary<Renderer, Material[]>();




    void Start()
    {
        Vector3 a =
            transform.eulerAngles;

        yaw = a.y;
        pitch = a.x;
    }



    void LateUpdate()
    {
        RotateCamera();

        Follow();

        WallFade();
    }





    void RotateCamera()
    {
        float x =
            Input.GetAxis("RightStickX");

        float y =
            Input.GetAxis("RightStickY");


        yaw +=
            x * lookSpeed * Time.deltaTime;


        pitch -=
            y * lookSpeed * Time.deltaTime;


        pitch =
            Mathf.Clamp(
                pitch,
                minPitch,
                maxPitch
            );
    }




    void Follow()
    {
        if (player == null)
            return;


        Quaternion rot =
            Quaternion.Euler(
                pitch,
                yaw,
                0
            );


        Vector3 offset =
            rot * new Vector3(
                0,
                height,
                -distance
            );


        Vector3 target =
            player.position
            + Vector3.up * 1.2f;



        transform.position =
            Vector3.Lerp(
                transform.position,
                target + offset,
                followSpeed * Time.deltaTime
            );



        transform.rotation =
            Quaternion.Lerp(
                transform.rotation,
                rot,
                smooth * Time.deltaTime
            );
    }






    void WallFade()
    {
        foreach (Renderer r in walls)
        {
            Restore(r);
        }

        walls.Clear();



        Vector3 dir =
            player.position
            - transform.position;


        RaycastHit[] hits =
            Physics.RaycastAll(
                transform.position,
                dir.normalized,
                dir.magnitude
            );



        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.CompareTag("Wall"))
            {
                Renderer r =
                    hit.collider.GetComponent<Renderer>();


                if (r)
                {
                    Fade(r);
                    walls.Add(r);
                }
            }
        }
    }





    void Fade(Renderer r)
    {
        if (!save.ContainsKey(r))
            save.Add(
                r,
                r.materials
            );


        foreach (Material m in r.materials)
        {
            Color c = m.color;

            c.a =
                Mathf.Lerp(
                    c.a,
                    fadeAlpha,
                    Time.deltaTime * fadeSpeed
                );

            m.color = c;

            m.SetFloat("_Surface", 1);
            m.renderQueue = 3000;
        }
    }




    void Restore(Renderer r)
    {
        foreach (Material m in r.materials)
        {
            Color c = m.color;

            c.a =
                Mathf.Lerp(
                    c.a,
                    1,
                    Time.deltaTime * fadeSpeed
                );

            m.color = c;
        }
    }
}