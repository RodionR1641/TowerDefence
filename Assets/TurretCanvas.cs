using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurretCanvas : MonoBehaviour
{    
    [SerializeField] GameObject upgradeButton;
    [SerializeField] GameObject upgradePrice;
    private TMP_Text upgradePriceText;
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
        upgradePriceText = upgradePrice.GetComponent<TMP_Text>();   
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
        upgradeButton.SetActive(false);
        upgradePriceText.enabled = false;
    }
}
