using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;

public class cameraController : MonoBehaviour
{
    public float speed = 10f;
    public float borderThickness = 10;
    public float scrollSpeed = 5;
    public float minY = 40;
    public float maxY = 80;
    public float minX = -50;
    public float maxX = 50;
    public float minZ = -60;
    public float maxZ = 60;

    private bool moveAllowed = true;
    private Mouse currentMouse;

    void Start()
    {
        currentMouse = Mouse.current;
    }

    // Update is called once per frame
    void Update()
    {   
        /*
        //escape prevents movement
        if(Input.GetKeyDown(KeyCode.Escape)){
            moveAllowed = !moveAllowed;
        }

        if(!moveAllowed){
            return;
        }

        //check for key press or mouse at the top of screen
        if(Input.GetKey("w") || Input.mousePosition.y >= Screen.height - borderThickness){
            transform.Translate(Vector3.forward*speed*Time.deltaTime, Space.World);//world coordinates, ignore rotation of camera
        }
        if(Input.GetKey("s") || Input.mousePosition.y <= borderThickness){
            transform.Translate(Vector3.back*speed*Time.deltaTime, Space.World);//world coordinates, ignore rotation of camera
        }
        if(Input.GetKey("a") || Input.mousePosition.x <= borderThickness){
            transform.Translate(Vector3.left*speed*Time.deltaTime, Space.World);//world coordinates, ignore rotation of camera
        }
        if(Input.GetKey("d") || Input.mousePosition.x >= Screen.width - borderThickness){
            transform.Translate(Vector3.right*speed*Time.deltaTime, Space.World);//world coordinates, ignore rotation of camera
        }

        Vector3 position = transform.position;

        position.x = Mathf.Clamp(position.x,minX,maxX);
        position.z = Mathf.Clamp(position.z,minZ,maxZ);

        transform.position = position;
        */
    }
}
