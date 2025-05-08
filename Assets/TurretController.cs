using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TurretController : MonoBehaviour
{
    [SerializeField] protected float range = 15.0f;

    [SerializeField] protected float fireRate = 0.5F;
    [SerializeField] protected Transform firePoint;
    [SerializeField] protected float weaponDamage = 2;
    [SerializeField] protected LayerMask enemyLayer;
    [SerializeField] protected int turretType = 0; //turret type is just for denoting that this is e.g. a laser turret
    [SerializeField] protected GameObject rangeIndicator;//circle indicating the range the turret can shoot
    public float rotationSpeed=10000.0f;
    protected Transform target;
    protected bool placed = false;


    public void FindNearestEnemy(){
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
