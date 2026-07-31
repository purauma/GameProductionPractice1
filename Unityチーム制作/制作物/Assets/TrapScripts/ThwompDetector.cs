using UnityEngine;

public class ThwompDetector : MonoBehaviour
{
    private Thwomp thwomp;


    private void Start()
    {
        thwomp = GetComponentInParent<Thwomp>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            thwomp.Activate();
        }
    }
}