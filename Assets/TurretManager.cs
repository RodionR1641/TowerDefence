using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;

public class TurretManager : MonoBehaviour
{
    public GameObject turret; 
    public LayerMask groundLayer;

    public DecalProjector decalProjector; 
    private Camera mainCamera;
    private Mouse mouse;
    RaycastHit2D hit2D;


    void Start()
    {
        mainCamera = Camera.main;   
        mouse = Mouse.current;
    }

    void Update() 
    { 

        
        RaycastHit hit; 
        bool valid = true; 

        Vector3 mouseScreenPosition = new Vector3(mouse.position.ReadValue().x, 
                                           mouse.position.ReadValue().y, 
                                           mainCamera.nearClipPlane);

        Ray ray = mainCamera.ScreenPointToRay(mouseScreenPosition);


        Vector3 mouseTilePosition = mouse.position.ReadValue();
        mouseTilePosition.z = -mainCamera.transform.position.y;

        Vector3 worldPos = mainCamera.ScreenToWorldPoint(mouseTilePosition);

        Vector2 overlapPoint = new Vector2(worldPos.x, worldPos.z); // Use XZ plane!
        Collider2D hit2 = Physics2D.OverlapPoint(overlapPoint, groundLayer);

        if (hit2 != null)
        {
            Debug.Log($"Blocked by: {hit2.name} (Tilemap)");
        }


        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer)) 
        {

            Collider[] hitColliders = Physics.OverlapSphere(hit.point, 2.0f); 
            foreach(Collider col in hitColliders) 
            {   
                if (col.CompareTag("Turret") || col.CompareTag("Path")) 
                { 
                    valid = false; 
                } 
            } 
            if (valid){
                if(Mouse.current.leftButton.wasPressedThisFrame){
                    Instantiate(turret, hit.point, Quaternion.identity);
                }
                decalProjector.gameObject.SetActive(true); 
                decalProjector.transform.position = hit.point;  
            }
            else
            {
                decalProjector.gameObject.SetActive(false); 
            } 
        } 
        else
        {
            decalProjector.gameObject.SetActive(false);
        }
    }

    /*
    void OnDrawGizmos()
    {
        if (hit2D.collider != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(hit2D.point, Vector3.one * 0.5f);
        }
    }
    */
}
