using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;

//class responsible for managing the turret placement and costs
public class TurretManager : MonoBehaviour
{
    public LayerMask groundLayer;

    //public DecalProjector decalProjector; 
    [SerializeField] private Camera mainCamera;
    [SerializeField] private List<Material> normalMaterials; //keeps track of normal material designs for all turrets
    [SerializeField] private Material validMaterial;
    [SerializeField] private Material invalidMaterial;
    private Mouse mouse;
    private GameObject turret; //the current turret selected
    private int summonCost = 10;
    private Renderer currentTurretRenderer;
    private int chosenTurretType = 0;//responsible for keeping track of the turret prefab type e.g. laser


    void Start()
    {
        mainCamera = Camera.main;   
    }

    void Update2() 
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
                    
                    int currentMoney = GameStats.Instance.GetCurrentMoney();
                    Debug.Log($"Current money is {currentMoney}");
                    if(currentMoney >= summonCost){
                        Instantiate(turret, hit.point, Quaternion.identity);
                        GameStats.Instance.ChangeMoney(-summonCost);
                    }
                }
                //decalProjector.gameObject.SetActive(true); 
                //decalProjector.transform.position = hit.point;  
            }
            else
            {
                //decalProjector.gameObject.SetActive(false); 
            } 
        } 
        else
        {
            //decalProjector.gameObject.SetActive(false);
        }
    }


    void Update()
    {
        if(turret != null){
            Ray camray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            //just moving the tower decal around where mouse is pointing
            if(Physics.Raycast(camray,out hitInfo,1000f,groundLayer)){
                turret.transform.position = hitInfo.point;//this updates the turret position

                bool isValid = IsValid(hitInfo.point);
                UpdateTurretColor(isValid);
                
                //need to finalise the position of tower - and check if we have a hit info
                if(Input.GetMouseButtonDown(0) && isValid)
                {
                    Debug.Log("Placed Turret");
                    PlaceTurret();
                }
            }
        }   
    }

    bool IsValid(Vector3 position){
        Collider[] hitColliders = Physics.OverlapSphere(position, 2.0f); 
        foreach(Collider col in hitColliders) 
        {   
            if (col.CompareTag("Turret") || col.CompareTag("Path")) 
            { 
                return false;
            } 
        }
        return true;
    }

    void UpdateTurretColor(bool isValid)
    {
        if(currentTurretRenderer == null)
        {
            currentTurretRenderer = turret.GetComponent<Renderer>();
        }
        currentTurretRenderer.material = isValid ? validMaterial : invalidMaterial;
    }

    void PlaceTurret()
    {
        // Deduct money and finalize placement
        if(GameStats.Instance.GetCurrentMoney() >= summonCost)
        {
            GameStats.Instance.ChangeMoney(-summonCost);
            //turret.GetComponent<Turret>().Activate(); // Add activation logic
            Collider turretCollider = turret.GetComponent<BoxCollider>();
            //collider so then cant e.g. place 2 turrets on top of each other
            if(turretCollider != null){
                Debug.Log("Turret collider set");
                turretCollider.enabled = true;
            }
            currentTurretRenderer.material = normalMaterials[chosenTurretType];
            turret = null;
            currentTurretRenderer = null;
        }
    }

    //turret type shoud match the materials list for this game object
    public void SetTurretPlace(GameObject turretPrefab){
        //only can select if have enough money
        if(turret == null && (GameStats.Instance.GetCurrentMoney() >= summonCost))
        {   
            chosenTurretType = GetTurretType(turretPrefab);
            turret = Instantiate(turretPrefab, Vector3.zero, Quaternion.identity);
            currentTurretRenderer = turret.GetComponent<Renderer>();
            UpdateTurretColor(false); // Start as invalid

        }
    }

    private int GetTurretType(GameObject turretPrefab){
        return turretPrefab.GetComponent<TurretController>().GetTurretType();
    }
}
