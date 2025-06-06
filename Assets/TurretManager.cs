using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


//class responsible for managing the turret placement on map and costs
public class TurretManager : MonoBehaviour
{
    public LayerMask groundLayer;

    [SerializeField] private Camera mainCamera;
    [SerializeField] private List<Material> normalMaterials; //keeps track of normal material designs for all turrets
    [SerializeField] private Material validMaterial;
    [SerializeField] private Material invalidMaterial;
    [SerializeField] private Material validMaterialRange;
    [SerializeField] private Material invalidMaterialRange;
    private GameObject turret; //the current turret selected
    private Renderer currentTurretRenderer;
    private Renderer currentTurretRangeRenderer;
    private int chosenTurretType = 0;//responsible for keeping track of the turret prefab type e.g. laser
    private TurretController turretController = null;
    private GameObject turretRangeIndicator = null;
    private int summonCost = 0;

    void Start()
    {
        mainCamera = Camera.main;   
    }

    //checks if can place the turret on the current map or not. cant place turrets on enemy path and on other turrets or outs
    //ide the map
    void Update()
    {
        if(turret != null){
            Ray camray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            //just moving the tower decal around where mouse is pointing
            if(Physics.Raycast(camray,out hitInfo,1000f,groundLayer)){
                Vector3 newPosition = hitInfo.point;
                newPosition.y = 3f;//make sure the turret is above the ground
                turret.transform.position = newPosition;

                bool isValid = IsValid(hitInfo.point);
                turretRangeIndicator.SetActive(true);
                UpdateTurretColor(isValid);

                //need to finalise the position of tower - and check if we have a hit info
                if(Input.GetMouseButtonDown(0) && isValid)
                {
                    Debug.Log("Placed Turret");
                    PlaceTurret();
                }
                else if(Input.GetMouseButton(1)){
                    Debug.Log("Cancelled turret");
                    Destroy(turret);
                    turret=null;
                }
            }
            else{
                turretRangeIndicator.SetActive(false);
            }
        }   
    }

    //check that the are is neither another turret or a path for enemies to go on
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

    //updates visually to tell player if they can place the turret at said location or no
    void UpdateTurretColor(bool isValid)
    {
        currentTurretRenderer.material = isValid ? validMaterial : invalidMaterial;
        currentTurretRangeRenderer.material = isValid ? validMaterialRange : invalidMaterialRange;
    }

    //finilise placement and fix position of turret. it can now fire at enemies
    void PlaceTurret()
    {
        // Deduct money and finalize placement
        if(GameStats.Instance.GetCurrentMoney() >= summonCost)
        {
            GameStats.Instance.ChangeMoney(-summonCost);
            Collider turretCollider = turret.GetComponent<BoxCollider>();
            //collider so then cant e.g. place 2 turrets on top of each other
            if(turretCollider != null){
                Debug.Log("Turret collider set");
                turretCollider.enabled = true;
            }
            turretController.PlaceTurret();
            TurretCanvas turretCanvas = turret.GetComponentInChildren<TurretCanvas>();
            turretCanvas.SetFixedPosition();

            currentTurretRenderer.material = normalMaterials[chosenTurretType];
            turret = null;
            currentTurretRenderer = null;
            turretRangeIndicator.SetActive(false);

            GameStats.Instance.AddTurret();
        }
    }

    //instantiates said turret so the player can place it on the map
    //turret type shoud match the materials list for this game object
    public void SetTurretPlace(GameObject turretPrefab){
        int currentTurretNum = GameStats.Instance.GetCurrentNumTurrets();

        int costToCheck = turretPrefab.GetComponent<TurretController>().GetSummonCost();
        //only can select if have enough money
        Debug.Log($"current money = {GameStats.Instance.GetCurrentMoney()}, summon cost = {costToCheck}");
        if(turret == null && (GameStats.Instance.GetCurrentMoney() >= costToCheck) && currentTurretNum < GameStats.maxNumTurrets)
        {   
            Debug.Log("MADE TURRET");
            turret = Instantiate(turretPrefab, Vector3.zero, Quaternion.identity);
            GetTurretInfo();
            currentTurretRenderer = turret.GetComponent<Renderer>();
            UpdateTurretColor(false); // Start as invalid
        }
    }

    //get additional information about turret components
    private void GetTurretInfo(){
        turretController = turret.GetComponent<TurretController>();
        turretRangeIndicator = turretController.GetRangeIndicator();
        currentTurretRenderer = turret.GetComponent<Renderer>();
        currentTurretRangeRenderer = turretController.GetRangeIndicator().GetComponent<Renderer>();
        
        chosenTurretType = turretController.GetTurretType();
        //Debug.Log($"Turret type = {chosenTurretType}");
        summonCost = turretController.GetSummonCost();
    }
}