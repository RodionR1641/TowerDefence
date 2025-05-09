using NUnit.Framework;
using Unity.IntegerTime;
using UnityEngine;

public class RocketController : TurretController
{
    public GameObject rocketPrefab;
    private float nextFire = 4.5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        summonCost = 25;
        range = 20;
        rangeIndicator.transform.localScale = Vector3.one * (range-3);
        weaponDamage = 20;//does area damage
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
            GameObject rocketObject = Instantiate(rocketPrefab,firePoint.position,firePoint.rotation);
            Rocket bullet = rocketObject.GetComponent<Rocket>();

            if(bullet != null){
                bullet.SetWeaponDamage(weaponDamage);
                bullet.SetTarget(target);
            }
        }
    }
}
