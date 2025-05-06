using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByTime : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 20);// gameObject refers to the object the script is attached to
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
