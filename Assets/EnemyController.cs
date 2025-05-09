using UnityEngine;

public class EnemyController : MonoBehaviour
{   
    public Waypoint waypoint;
    public HealthBar healthBar;
    [SerializeField] protected double maxHealth = 100;
    protected UnityEngine.AI.NavMeshAgent agent; 
    public System.Action<EnemyController> OnEnemyDied; //use this to tell gameloop about this enemy dying
    [SerializeField] protected int deathReward = 2;
    [SerializeField] protected int baseDamage = 2; //how much the enemy damages the base if it gets there
    protected double currentHealth = 100;

    // Use this for initialization 
    void Start () 
    { 
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>(); 
        agent.destination = waypoint.transform.position;  
        currentHealth = maxHealth;
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

    public virtual void TakeDamage(float damage,bool armorBolt=false){
        currentHealth -= damage; 
        healthBar.SetHealth(currentHealth, maxHealth); 
        if(currentHealth<=0) 
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
}
