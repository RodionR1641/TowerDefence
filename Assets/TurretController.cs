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
    [SerializeField] float laserWidth = 1f;
    [SerializeField] int weaponDamage = 50;
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] int turretType = 0; //turret type is just for denoting that this is e.g. a laser turret

    public float rotationSpeed=10000.0f;
    private Transform target;
    private LineRenderer laser;
    private float nextFire = 1f;


    void Start()
    {
        laser = gameObject.AddComponent<LineRenderer>();
        laser.startWidth = laserWidth;
        laser.endWidth = laserWidth;
        laser.material = laserMaterial;
        laser.enabled = false;   
    }

    // Update is called once per frame
    void Update()
    {

        FindNearestEnemy();

        if (target != null)
        {
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

}
