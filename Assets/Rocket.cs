using UnityEngine;

public class Rocket : MonoBehaviour
{
    private Transform target;
    [SerializeField] private float areaDamageRange = 5f;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float weaponDamage = 20;
    [SerializeField] private LayerMask enemyLayers;
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
                if(enemyController!= null){
                    //pass the position of the hit
                    Hit(enemyController,collision.collider.transform.position);
                }
            }
        }    
    }

    void Hit(EnemyController primaryEnemy, Vector3 impactPoint){

        //first damage the primary enemy we hit

        primaryEnemy.TakeDamage(weaponDamage);

        Collider[] hitAreaColliders = Physics.OverlapSphere(impactPoint,areaDamageRange,enemyLayers);
            
        //everyone in the radius should get hit, area damage
        foreach (Collider otherEnemyCollider in hitAreaColliders){
            EnemyController enemyController = otherEnemyCollider.GetComponent<EnemyController>();
            //avoid damaging the original enemy twice
            if(enemyController != null && enemyController != primaryEnemy){
                enemyController.TakeDamage(weaponDamage);
            }
        }

        Destroy(gameObject);
    }


    public void SetTarget(Transform targetToChase){
        target = targetToChase;
    }

    public void SetWeaponDamage(float damage){
        weaponDamage = damage;
    }
}
