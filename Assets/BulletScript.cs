using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Transform target;
    private float speed = 10f;
    private float weaponDamage = 20;
    // need to go to target 
    void Update()
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

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            CapsuleCollider capsule = collision.collider as CapsuleCollider;
            if (capsule != null)
            {
                GameObject enemy = collision.gameObject;
                Debug.Log($"Hit enemy capsule collider on {enemy.name}");
                EnemyController enemyController = enemy.GetComponent<EnemyController>();
                if(enemyController!=null){
                    Hit(enemyController);
                }
            }
        }    
    }

    void Hit(EnemyController enemyController){
        //will need to do area damage here
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
