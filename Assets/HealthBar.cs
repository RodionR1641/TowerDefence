using UnityEngine;
using UnityEngine.UI; 
 
public class HealthBar : MonoBehaviour 
{ 
    [SerializeField]protected Image healthBarImage; 
 
    protected Camera cam; 
 
    void Start() 
    { 
        cam = Camera.main; 
        healthBarImage.fillAmount = 1.0f; 
    } 
 
    public void SetHealth(double currentHealth, double maxHealth) 
    { 
        healthBarImage.fillAmount = (float)(currentHealth / maxHealth); 
    } 
 
    void Update() 
    { 
        transform.forward = cam.transform.forward; 
    } 
}

