
public class BasicEnemyController : EnemyController
{   
    public HealthBar healthBar;

    //takes damage based on weapon
    public override void TakeDamage(float damage,bool armorBolt=false){
        currentHealth -= damage; 
        healthBar.SetHealth(currentHealth, maxHealth); 
        if(currentHealth<=0) 
        { 
            GameStats.Instance.ChangeMoney(deathReward);//money for killing the enemy
            //Debug.Log($"Got reward of {deathReward}");
            Die(); 
        }
    }
}
