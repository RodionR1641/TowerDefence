using UnityEngine;

public class ArmorEnemyController : EnemyController
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] ArmorBar armorBar;
    [SerializeField] private double maxArmor=100; //similar to health
    private double currentArmor;
    private double damageModifierArmor = 0.2;//only take 20% damage if armor is present
    
    override protected void Start()
    {
        base.Start();
        currentArmor = maxArmor;
    }


    //both armor and health damage is affected if there is an armor present or not
    public override void TakeDamage(float damage,bool armorBolt=false){
        double damageModifier = 1;
        if(currentArmor>0){
            damageModifier = damageModifierArmor;
        }

        //check if armor bolt shot us, then do full damage to armor * 2
        if(armorBolt){
            currentArmor -= damage*2;//do double damage against armor
        }
        else{
            currentArmor -= damage*damageModifier;
        }

        currentHealth -= damage*damageModifier; 
        //Debug.Log($"Took {damage*damageModifier} damage");
        armorBar.SetHealth(currentHealth,maxHealth); 
        armorBar.SetArmor(currentArmor,maxArmor);
        if(currentHealth<=0) 
        { 
            GameStats.Instance.ChangeMoney(deathReward);//money for killing the enemy
            //Debug.Log($"Got reward of {deathReward}");
            Die(); 
        }
    }
}
