using UnityEngine;

public abstract class EnemyController : MonoBehaviour
{
    public Waypoint waypoint;
    [SerializeField] protected double maxHealth = 100;
    protected UnityEngine.AI.NavMeshAgent agent; 
    [SerializeField] protected float deathReward = 0.5f;
    [SerializeField] protected int baseDamage = 2;
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
