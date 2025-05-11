using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurretCanvas : MonoBehaviour
{    
    [SerializeField] Button upgradeButton;
    [SerializeField] GameObject upgradePrice;
    private TextMeshPro upgradePriceText;
    private Vector3 fixedPosition;
    private bool fixedPositionSet = false;
    private Quaternion fixedRotation;
    private Canvas canvas;

    void Start()
    {
        fixedRotation = transform.rotation;
    }

    void Awake()
    {
        canvas = GetComponent<Canvas>();
        canvas.enabled = false; 
        upgradePriceText = upgradePrice.GetComponent<TextMeshPro>();   
    }

    void LateUpdate()
    {
        if(fixedPositionSet){
            transform.position = fixedPosition;
        }
        transform.rotation = fixedRotation; //make sure the canvas never rotates when the turret does
    }

    public void EnableCanvas(){
        canvas.enabled = !canvas.enabled;//constantly change between being active and not everytime turret is clicked
    }

    //once the turret has been placed, fix the position of the canvas in place
    public void SetFixedPosition(){
        fixedPositionSet = true;
        fixedPosition = transform.position;
    }

    public void HideUpgradeButton()
    {
        upgradeButton.enabled = false;
        upgradePriceText.enabled = false;
    }
}
