using UnityEngine;

public class SpecialAbilityManager : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask enemyLayers;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private int areaDamage = 80;
    [SerializeField] private float areaDamageRange = 8f;
    [SerializeField] private int summonCost = 30;
    private GameObject abilityCircle = null;

    void Start()
    {
        mainCamera = Camera.main; 
    }

    // Update is called once per frame
    void Update()
    {
        if(abilityCircle!=null){
            Ray camray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            //just moving the tower decal around where mouse is pointing
            if(Physics.Raycast(camray,out hitInfo,1000f,groundLayer)){
                Vector3 newPosition = hitInfo.point;
                newPosition.y = 3f;//make sure the turret is above the ground
                abilityCircle.transform.position = newPosition;


                //need to finalise the position of tower - and check if we have a hit info
                if(Input.GetMouseButtonDown(0))
                {
                    Debug.Log("Used Ability");
                    ActivateAbility();
                }
                else if(Input.GetMouseButton(1)){
                    Debug.Log("Cancelled ability");
                    Destroy(abilityCircle);
                    abilityCircle=null;
                }
            }
        }
    }

    private void ActivateAbility(){
        if(GameStats.Instance.GetCurrentMoney() >= summonCost){
            GameStats.Instance.ChangeMoney(-summonCost);

            Collider[] hitAreaColliders = Physics.OverlapSphere(abilityCircle.transform.position,areaDamageRange,enemyLayers);

            foreach (Collider enemyCollider in hitAreaColliders){
                EnemyController enemyController = enemyCollider.GetComponent<EnemyController>();
                enemyController.TakeDamage(areaDamage);
            }

            Destroy(abilityCircle);
            abilityCircle = null;
            GameStats.Instance.RegisterAbility();
        }
    }

    public void SetActivityCircle(GameObject areaCirclePrefab){
        int currentTurretNum = GameStats.Instance.GetCurrentNumAbilities();
        //only can select if have enough money
        if(abilityCircle == null && (GameStats.Instance.GetCurrentMoney() >= summonCost) &&
            currentTurretNum < GameStats.maxNumAbilities)
        {   
            abilityCircle = Instantiate(areaCirclePrefab, Vector3.zero, areaCirclePrefab.transform.rotation);

            SpriteRenderer spriteRenderer = abilityCircle.GetComponent<SpriteRenderer>();

            float spriteWidth = spriteRenderer.sprite.bounds.size.x;//native size

            float scale = areaDamageRange*2 / spriteWidth;//diameter
            abilityCircle.transform.localScale = new Vector3(scale,scale,1);//resize
        }
    }    
}
