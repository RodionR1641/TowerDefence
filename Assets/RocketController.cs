using UnityEngine;

//keeps track of rocket launcher turret mechanics
public class RocketController : TurretController
{
    public GameObject rocketPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        rangeIndicator.transform.localScale = Vector3.one * (range-3);
        nextFire = fireRate;
        Debug.Log($"Rocket controller price = {summonCost}");
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
                    //Debug.Log($"fire rate = {fireRate}");
                    nextFire = Time.time + fireRate;
                }

            }
        }
    }

    //instantiate a rocket to fire at enemies
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
