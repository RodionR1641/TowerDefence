using UnityEngine;
using UnityEngine.UIElements;

public class Waypoints : MonoBehaviour
{
    public static Transform[] waypoints;

    //find all the children objects and load them in
    void Awake()
    {
        waypoints = new Transform[transform.childCount];

        for(int i=0;i<waypoints.Length;i++){
            waypoints[i] = transform.GetChild(i);
        } 
    }
}
