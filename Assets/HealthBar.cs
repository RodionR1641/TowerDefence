using UnityEngine;
using UnityEngine.UI; 
 
public class HealthBar : MonoBehaviour 
{ 
    public Image healthBarImage; 
 
    Camera cam; 
 
    void Start() 
    { 
        cam = Camera.main; 
        healthBarImage = GetComponentInChildren<Image>(); 
        healthBarImage.fillAmount = 1.0f; 
    } 
 
    public void SetHealth(float currentHealth, float maxHealth) 
    { 
        healthBarImage.fillAmount = currentHealth / maxHealth; 
    } 
 
    void Update() 
    { 
        transform.forward = cam.transform.forward; 
    } 
}

