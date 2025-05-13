using System.Collections.Generic;
using UnityEngine;

//responsible for general turret behaviour like tracking enemies
public class TurretController : MonoBehaviour
{
    [SerializeField] protected float range = 15.0f;

    [SerializeField] protected float fireRate = 0.5F;
    [SerializeField] protected Transform firePoint;
    [SerializeField] protected float weaponDamage = 2;
    [SerializeField] protected LayerMask enemyLayer;
    [SerializeField] protected int turretType = 0; //turret type is just for denoting that this is e.g. a laser turret
    [SerializeField] protected GameObject rangeIndicator;//circle indicating the range the turret can shoot
    [SerializeField] protected int summonCost = 10; //price to just place the turret on map
    [SerializeField] protected int upgradeCost = 15;
    [SerializeField] protected int sellReward = 5;//how much money you get back by selling the turret
    [SerializeField] protected List<float> upgradeStats = new List<float> {1f,-0.15f};//upgrades: weaponDamage, fireRate by appending these values
    protected float nextFire; //tracks when it can fire again
    protected Transform target;
    protected bool placed = false;
    protected float shortestDistance; //shortest enemy in vicinity
    protected bool upgraded = false;//keep track of whether this turret has already been upgraded or not
    protected TurretCanvas turretCanvas;


    virtual protected void Start()
    {
        turretCanvas = gameObject.GetComponentInChildren<TurretCanvas>();
    }


    //continuosly search for enemy closest to you, then instantly turn to face them
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
            //only look at enemy if they are within range
            if(nearestEnemy != null && shortestDistance <= range){
                target = nearestEnemy.transform;
                transform.rotation = Quaternion.LookRotation(target.position - transform.position);
            }
            else{
                target = null;
            }
        }
    }

    //check if can upgrade and if it hasnt been before - alters damage and fire rate
    public void UpgradeTurret(){
        if(GameStats.Instance.GetCurrentMoney() >= upgradeCost && upgraded == false){
            weaponDamage += upgradeStats[0];
            fireRate += upgradeStats[1];
            GameStats.Instance.ChangeMoney(-upgradeCost);
            upgraded = true;
            turretCanvas.HideUpgradeButton();//make sure user doesn't think they can upgrade again
        }
    }
    //sell for a fraction of build price
    public void SellTurret(){
        GameStats.Instance.ChangeMoney(sellReward);
        GameStats.Instance.RemoveTurret();
        //Debug.Log($"Current Turret Num = {GameStats.Instance.GetCurrentNumTurrets()}");
        
        Destroy(gameObject);
    }

    //if press on the turret prefab ,set the canvas to be active or inactive depending on if it was previously active
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
    //responsible for showing how much range the turret has
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
