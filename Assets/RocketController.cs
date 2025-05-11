using System.Collections.Generic;
using UnityEngine;

public class RocketController : TurretController
{
    public GameObject rocketPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        summonCost = 25;
        range = 20;
        rangeIndicator.transform.localScale = Vector3.one * (range-3);
        weaponDamage = 20;//does area damage
        fireRate = 4.5f;
        nextFire = fireRate;
        upgradeStats = new List<float>{10f,-1.5f};
    }

    // Update is called once per frame
    void Update()
    {
        if(placed){
            FindNearestEnemy();

            if (target != null)
            {

                if(Time.time > nextFire){
                    FireBolt();
                    Debug.Log($"fire rate = {fireRate}");
                    nextFire = Time.time + fireRate;
                }

            }
        }
    }

    //instantiate a bullet
    private void FireBolt(){        
        //only fire within range
        if (shortestDistance <= range)
        {
            GameObject rocketObject = Instantiate(rocketPrefab,firePoint.position,firePoint.rotation);
            Rocket bullet = rocketObject.GetComponent<Rocket>();

            if(bullet != null){
                bullet.SetWeaponDamage(weaponDamage);
                bullet.SetTarget(target);
            }
        }
    }
}
