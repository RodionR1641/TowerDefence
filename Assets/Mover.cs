using UnityEngine;

public class Mover : MonoBehaviour
{   
    public Waypoint waypoint;
    public HealthBar healthBar;
    double health = 100;
    public float speed = 10f;
    private Transform target;

    private int pointIndex = 0;

    void Start(){
        target = Waypoints.waypoints[0];
    }
    
    void Update () { 
        
        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized*speed*Time.deltaTime,Space.World);//normalise so there is a unit vector with same dir


        if(Vector3.Distance(transform.position,target.position) <=0.2f){
            pointIndex++;
            if(pointIndex >= Waypoints.waypoints.Length){
                Destroy(gameObject);
            }
            target = Waypoints.waypoints[pointIndex];
        }
    }

    void OnTriggerStay(Collider other){
        if(other.CompareTag("Weapon"))
        {
            health -= 0.5; // Damage per second
            healthBar.SetHealth((int)health, 100);  
            if(health<=0) 
            { 
             Destroy(gameObject); 
            } 
        }
    }
}

