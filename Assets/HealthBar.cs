using UnityEngine;
using UnityEngine.UI; 
 
//responsible for tracking the health of the enemy as it moves down the path 
public class HealthBar : MonoBehaviour 
{ 
    [SerializeField]protected Image healthBarImage; 
 
    protected Camera cam; 
 
    protected virtual void Start() 
    { 
        cam = Camera.main; 
        healthBarImage.fillAmount = 1.0f; //track of health amount
    } 
 
    public void SetHealth(double currentHealth, double maxHealth) 
    { 
        //fraction between current and max health
        healthBarImage.fillAmount = (float)(currentHealth / maxHealth); 
    } 
 
    void Update() 
    { 
        transform.forward = cam.transform.forward; 
    } 
}

