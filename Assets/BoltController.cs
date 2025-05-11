using System.Collections.Generic;
using NUnit.Framework;
using Unity.IntegerTime;
using UnityEngine;

public class BoltController : TurretController
{
    public GameObject bulletPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        summonCost = 10;
        range = 20;
        rangeIndicator.transform.localScale = Vector3.one * (range-3);
        weaponDamage = 30;
        fireRate = 4.0f;
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
            GameObject bulletObject = Instantiate(bulletPrefab,firePoint.position,firePoint.rotation);
            Bullet bullet = bulletObject.GetComponent<Bullet>();

            if(bullet != null){
                bullet.SetWeaponDamage(weaponDamage);
                bullet.SetTarget(target);
            }
        }
    }
}
