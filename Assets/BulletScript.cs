using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Transform target;
    private float speed = 80f;

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

        //check if something has been hit
        if(direction.magnitude <= distanceFrame){
            Hit();
            return;
        }
        //moving
        transform.Translate(direction.normalized * distanceFrame, Space.World);
    }

    void Hit(){
        Debug.Log("hit something");
    }


    public void SetTarget(Transform targetToChase){
        target = targetToChase;
    }
}
