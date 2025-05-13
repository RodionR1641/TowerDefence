using UnityEngine;

public class Bullet : MonoBehaviour
{
    protected Transform target;
    protected float speed = 10f;
    protected float weaponDamage = 20;
    // need to go to target 
    protected void Update()
    {   
        //if nothing to chase anymore, just die
        if(target == null){
            Destroy(gameObject);
            return;
        }

        //need to direct it towards the target
        Vector3 direction = target.position - transform.position;
        float distanceFrame = speed * Time.deltaTime;//how much to move this frame

        //moving
        transform.Translate(direction.normalized * distanceFrame, Space.World);
    }

    //check for collisions with enemies
    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            CapsuleCollider capsule = collision.collider as CapsuleCollider;
            if (capsule != null)
            {
                GameObject enemy = collision.gameObject;
                //Debug.Log($"Hit enemy capsule collider on {enemy.name}, weapon damage = {weaponDamage}");
                EnemyController enemyController = enemy.GetComponent<EnemyController>();
                if(enemyController!=null){
                    Hit(enemyController);
                }
            }
        }    
    }

    void Hit(EnemyController enemyController){
        //just a simple hit on a single enemy
        enemyController.TakeDamage(weaponDamage,true);
        Destroy(gameObject);
    }


    public void SetTarget(Transform targetToChase){
        target = targetToChase;
    }

    public void SetWeaponDamage(float damage){
        weaponDamage = damage;
    }
}
