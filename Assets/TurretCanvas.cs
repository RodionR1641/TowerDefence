using UnityEngine;

public class TurretCanvas : MonoBehaviour
{   
    private Vector3 fixedPosition;
    private bool fixedPositionSet = false;
    private Quaternion fixedRotation;
    void Start()
    {
        fixedRotation = transform.rotation;
    }

    void LateUpdate()
    {
        if(fixedPositionSet){
            transform.position = fixedPosition;
        }
        transform.rotation = fixedRotation; //make sure the canvas never rotates when the turret does
    }

    //once the turret has been placed, fix the position of the canvas in place
    public void SetFixedPosition(){
        fixedPositionSet = true;
        fixedPosition = transform.position;
    }
}
