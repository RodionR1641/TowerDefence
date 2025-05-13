using UnityEngine;

//similar to bullet but does area damage
public class Rocket : Bullet
{
    [SerializeField] private float areaDamageRange = 5f;
    [SerializeField] private LayerMask enemyLayers;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            CapsuleCollider capsule = collision.collider as CapsuleCollider;
            if (capsule != null)
            {
                GameObject enemy = collision.gameObject;
                Debug.Log($"Hit enemy capsule collider on {enemy.name}, weapon damage = {weaponDamage}");
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
        //overlap sphere checks for every collision that intersects the range of the area damage
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
}
