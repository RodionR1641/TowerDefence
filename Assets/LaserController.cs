using UnityEngine;

public class LaserController: TurretController
{
    private LineRenderer laser;
    [SerializeField] float laserWidth = 10f;
    [SerializeField] Material laserMaterial;
    private float nextFire = 0.02f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        laser = gameObject.AddComponent<LineRenderer>();
        //laser.SetWidth(laserWidth,laserWidth);
        laser.startWidth = laserWidth;
        laser.endWidth = laserWidth;
        laser.material = laserMaterial;
        laser.enabled = false;   

        rangeIndicator.transform.localScale = Vector3.one * (range-3);//range and actual collider calculations seem to be slightly off
        rangeIndicator.SetActive(false);

        turretType = 0;
        summonCost = 12;
    }

    // Update is called once per frame
    void Update()
    {
        //only track and shoot once placed on the map
        if(placed){
            FindNearestEnemy();

            if (target != null)
            {
                laser.enabled = true;
                //RotateTowardsTarget();
                UpdateLaser();

                if(Time.time > nextFire){
                    FireLaser();
                    nextFire = Time.time + fireRate;
                }

            }
            else
            {
                laser.enabled = false;
            }
        }
    }

    void UpdateLaser(){
        laser.SetPosition(0, firePoint.position);
        laser.SetPosition(1, target.position);
    }

    void FireLaser()
    {
        laser.enabled = true;

        Vector3 rayOrigin = firePoint.position;
        Vector3 rayDirection = (target.position - firePoint.position).normalized;
        
        if (Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hit, range,enemyLayer))
        {
            Debug.Log($"Hit: {hit.collider.name} at {hit.point}");
            if (hit.collider.CompareTag("Enemy"))
            {
                hit.collider.GetComponent<EnemyController>().TakeDamage(weaponDamage);
            }
        }
    }
}
