using UnityEngine.UI; 
using UnityEngine;

public class ArmorBar : HealthBar
{
    public Image armorBarImage;
    protected override void Start()
    {
        base.Start();
        armorBarImage.fillAmount = 1.0f; 
    }

    public void SetArmor(double currentArmor,double maxArmor){
        armorBarImage.fillAmount = (float)(currentArmor / maxArmor);
    }

}
