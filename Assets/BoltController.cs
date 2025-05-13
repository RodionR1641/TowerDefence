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
        rangeIndicator.transform.localScale = Vector3.one * (range-3);
        nextFire = fireRate;
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
