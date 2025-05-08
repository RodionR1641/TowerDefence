using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TurretController : MonoBehaviour
{
    [SerializeField] float range = 15.0f;

    [SerializeField] float fireRate = 0.5F;
    [SerializeField] Material laserMaterial;
    [SerializeField] Transform firePoint;
    [SerializeField] float laserWidth = 10f;
    [SerializeField] float weaponDamage = 2;
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] int turretType = 0; //turret type is just for denoting that this is e.g. a laser turret
    [SerializeField] private GameObject rangeIndicator;//circle indicating the range the turret can shoot
    public float rotationSpeed=10000.0f;
    private Transform target;
    private LineRenderer laser;
    private float nextFire = 0.02f;
    private bool canFire = false;
    private bool placed = false;


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

    void FindNearestEnemy(){
        EnemyController[] enemies = FindObjectsOfType<EnemyController>();

        float shortestDistance = Mathf.Infinity;
        EnemyController nearestEnemy = null;

        foreach (EnemyController enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            
            if(distanceToEnemy < shortestDistance){
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }

            if(nearestEnemy != null && shortestDistance <= range){
                target = nearestEnemy.transform;
                transform.rotation = Quaternion.LookRotation(target.position - transform.position);
            }
            else{
                target = null;
            }
        }
    }

    void UpdateLaser(){
        laser.SetPosition(0, firePoint.position);
        laser.SetPosition(1, target.position);
    }

    //sometimes laser doesnt hit, change the way it works    
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

    public int GetTurretType(){
        return turretType;
    }

    public float GetRange(){
        return range;
    }
    public GameObject GetRangeIndicator(){
        return rangeIndicator;
    }

    public void PlaceTurret(){
        placed = true;
    }
}
