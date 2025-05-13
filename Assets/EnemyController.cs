using UnityEngine;

//responsible for controlling the enemy movement, damage
public abstract class EnemyController : MonoBehaviour
{
    public Waypoint waypoint;
    [SerializeField] protected double maxHealth = 100;
    protected UnityEngine.AI.NavMeshAgent agent; 
    [SerializeField] protected float deathReward = 0.5f; //money reward for killing this enemy
    [SerializeField] protected int baseDamage = 2; //how much damage it does when reaching end of map to base
    protected double currentHealth = 100;
    public System.Action<EnemyController> OnEnemyDied;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    virtual protected void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>(); 
        agent.destination = waypoint.transform.position;  
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        //move on to the next waypoint once close enough
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

    public abstract void TakeDamage(float damage,bool armorBolt=false);

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
