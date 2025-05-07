using UnityEngine;

public class playerMovement : MonoBehaviour
{

    private Vector3 Velocity;
    private Vector3 PlayerMovement;
    private Vector2 PlayerMouse;
    private float xRotation; //camera movement rotation

    [SerializeField] private Transform playerCamera;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private float speed;
    [SerializeField] private float sensitivity;

    void Start(){
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //set movement input vectors, based on horizontal and vertical axis, also x and y
        PlayerMovement = new Vector3(Input.GetAxisRaw("Horizontal"),0f,Input.GetAxisRaw("Vertical"));
        PlayerMouse = new Vector2(Input.GetAxis("Mouse X"),Input.GetAxis("Mouse Y"));
    
        MovePlayer();
        MovePlayerCamera();
    }

    //directional move vector based 
    private void MovePlayer(){
        Vector3 MoveVector = transform.TransformDirection(PlayerMovement)*speed;

        //determine if the velocity should be going up or down
        if(Input.GetKey(KeyCode.Space)){
            Velocity.y = 1f;
        }
        else if(Input.GetKey(KeyCode.LeftShift)){
            Velocity.y = -1f;
        }
        //move controller based on move input and velocity
        characterController.Move(MoveVector*speed*Time.deltaTime);
        characterController.Move(Velocity*speed*Time.deltaTime);

        Velocity.y = 0f;//prevent character controller moving up/down for infinite amount of time with no
    }

    private void MovePlayerCamera(){
        xRotation -= PlayerMouse.y * sensitivity; //tkae the x rotation, substract by player mouse input y
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); 

        transform.Rotate(0f,PlayerMouse.x * sensitivity,0f);//rotate the player object
        playerCamera.transform.localRotation = Quaternion.Euler(xRotation,0f,0f); //set the local rotation
    }
}
