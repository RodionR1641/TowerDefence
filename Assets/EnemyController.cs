using UnityEngine;

public class EnemyController : MonoBehaviour
{   
    public Waypoint waypoint;
    public HealthBar healthBar;
    double health = 100;
    UnityEngine.AI.NavMeshAgent agent; 
    public System.Action<EnemyController> OnEnemyDied; //use this to tell gameloop about this enemy dying
    [SerializeField] private int deathReward = 2;
    [SerializeField] private int baseDamage = 1; //how much the enemy damages the base if it gets there

    // Use this for initialization 
    void Start () 
    { 
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>(); 
        agent.destination = waypoint.transform.position;  
    } 
    // Update is called once per frame 
    void Update () { 
        if (!agent.pathPending && agent.remainingDistance < 3f) 
        { 
            if(waypoint.nextWaypoint!=null) 
            { 
                Waypoint nextWaypoint = waypoint.nextWaypoint; 
                waypoint = nextWaypoint; 
                agent.destination = waypoint.transform.position; 
            } 
            else 
            { 
                // attack the base code here
                GameStats.Instance.ChangeHealth(-baseDamage);
                Die(); 
            } 
        }
    }

    public void TakeDamage(float damage){
        health -= damage;
        healthBar.SetHealth((int)health, 100);
        if(health<=0) 
        { 
            GameStats.Instance.ChangeMoney(deathReward);//money for killing the enemy
            Debug.Log($"Got reward of {deathReward}");
            Die(); 
        }
    }

    public void SetSpeed(float speed){
        if(agent!=null){
            agent.speed = speed;
        }
    }


    public void Die(){
        OnEnemyDied?.Invoke(this); //trigger the death event
        Destroy(gameObject);
    }

    /* 
    void OnTriggerStay(Collider other){
        if(other.CompareTag("Weapon"))
        {
            Debug.Log("Got here weapon");
            health -= 0.5; // Damage per second
            healthBar.SetHealth((int)health, 100);  
            if(health<=0) 
            { 
             Destroy(gameObject); 
            } 
        }
    }
    */
}
