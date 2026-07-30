using UnityEngine;

public class RotateFloor2 : MonoBehaviour
{
    [SerializeField]
    private float rotatespeed = 0.1f;
    private void Update()
    {
        this.transform.Rotate(0, rotatespeed, 0);
    }
}
