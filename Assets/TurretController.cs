using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    [SerializeField] protected float range = 15.0f;

    [SerializeField] protected float fireRate = 0.5F;
    [SerializeField] protected Transform firePoint;
    [SerializeField] protected float weaponDamage = 2;
    [SerializeField] protected LayerMask enemyLayer;
    [SerializeField] protected int turretType = 0; //turret type is just for denoting that this is e.g. a laser turret
    [SerializeField] protected GameObject rangeIndicator;//circle indicating the range the turret can shoot
    protected float nextFire;
    public float rotationSpeed=10000.0f;
    protected Transform target;
    protected bool placed = false;
    protected int summonCost = 10;
    protected float shortestDistance;
    protected bool upgraded = false;//keep track of whether this turret has already been upgraded or not
    protected int upgradeCost = 15;
    protected List<float> upgradeStats = new List<float> {1f,-0.15f};//upgrades: weaponDamage, fireRate by appending these values
    protected int sellReward = 5;//how much money you get back by selling the turret
    protected TurretCanvas turretCanvas;


    virtual protected void Start()
    {
        turretCanvas = gameObject.GetComponentInChildren<TurretCanvas>();
    }

    public void FindNearestEnemy(){
        EnemyController[] enemies = FindObjectsOfType<EnemyController>();

        shortestDistance = Mathf.Infinity;
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

    public void UpgradeTurret(){
        if(GameStats.Instance.GetCurrentMoney() >= upgradeCost && upgraded == false){
            weaponDamage += upgradeStats[0];
            fireRate += upgradeStats[1];
            GameStats.Instance.ChangeMoney(-upgradeCost);
            upgraded = true;
            turretCanvas.HideUpgradeButton();//make sure user doesn't think they can upgrade again
        }
    }

    public void SellTurret(){
        GameStats.Instance.ChangeMoney(sellReward);
        Destroy(gameObject);
    }

    //if press,set the canvas to be active or inactive depending on if it was previously active
    protected void OnMouseDown()
    {
        turretCanvas.EnableCanvas();
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
    public int GetSummonCost(){
        return summonCost;
    }
}
