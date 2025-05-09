using UnityEngine.UI; 
using UnityEngine;

public class ArmorBar : HealthBar
{
    public Image armorBarImage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main; 
        healthBarImage.fillAmount = 1.0f; 
        armorBarImage.fillAmount = 1.0f; 
    }

    public void SetArmor(double currentArmor,double maxArmor){
        armorBarImage.fillAmount = (float)(currentArmor / maxArmor);
    }

}
