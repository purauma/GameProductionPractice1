using UnityEngine;

public class Turret : MonoBehaviour
{
    public Transform target;      
    public Transform head;        
    public float range = 10f;

    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 1f;
    private float fireCountdown = 0f;

    void Update()
    {
        if (target == null) return;

        float distance = Vector3.Distance(transform.position, target.position);

        if (distance <= range)
        {
            RotateToTarget(); 
            Shoot();         
        }

        fireCountdown -= Time.deltaTime;
    }

    void RotateToTarget()
    {
      
        Vector3 dir = target.position - head.position;

    
        dir.y = 0;

        Quaternion lookRotation = Quaternion.LookRotation(dir);
        head.rotation = Quaternion.Lerp(head.rotation, lookRotation, Time.deltaTime * 5f);
    }

    void Shoot()
    {
        if (fireCountdown > 0) return;

        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        fireCountdown = 1f / fireRate;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
