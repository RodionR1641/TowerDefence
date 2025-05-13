using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

// Class responsible for the base health,UI element changes and economy mechanics. It is a singleton
public class GameStats : MonoBehaviour
{
    [SerializeField] private int baseHealth = 10;
    [SerializeField] private float currentMoney = 10;//enough for 1 laser turret at the start
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI gameEndText;
    [SerializeField] private TextMeshProUGUI turretCountRemainingText;
    [SerializeField] private TextMeshProUGUI specialAbilityRemainingText;
    [SerializeField] public const int maxNumTurrets = 7;
    [SerializeField] public const int maxNumAbilities = 2;
    private int currentNumTurrets = 0; //number of turrets already placed on map
    private int currentNumAbilities = 0;//number of times the ability was used

    private static GameStats _instance; //singleton
    public static GameStats Instance {get {return _instance;}}


    private void Start()
    {
        moneyText.SetText($"£{currentMoney}");
        healthText.SetText($"Health {baseHealth}");
        turretCountRemainingText.SetText($"Current remaining turrets allowed: {maxNumTurrets}");
        specialAbilityRemainingText.SetText($"Current remaining ability uses: {maxNumAbilities}");
    }

    //singleton pattern
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    //function that can either increase or decrease the current income
    //get money for killing enemies and completing waves
    public void ChangeMoney(float money){
        currentMoney += money;
        moneyText.SetText($"$ {currentMoney}");
    }

    public float GetCurrentMoney(){
        return currentMoney;
    }

    //ends the game once run out of health
    public void ChangeHealth(int health){
        baseHealth += health;
        healthText.SetText($"Health {baseHealth}");

        if(baseHealth <= 0){
            EndGame(false);
        }
    }

    //either tells the player they lost or won, can go back to menu now
    public void EndGame(bool win){
        Time.timeScale = 0f; //pause game
        gameOverPanel.SetActive(true);

        if(win){
            gameEndText.SetText("YOU WON!");
        }
    }

    public void ReturnToStart(){
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    public int GetBaseHealth(){
        return baseHealth;
    }

    //registering that an ability was used
    public void RegisterAbility(){
        currentNumAbilities++;
        specialAbilityRemainingText.SetText($"Current remaining ability uses: {maxNumAbilities-currentNumAbilities}");
    }

    //keep track of turret count in these 2 functions
    public void AddTurret(){
        currentNumTurrets++;
        turretCountRemainingText.SetText($"Current remaining turrets allowed: {maxNumTurrets-currentNumTurrets}");
    }

    public void RemoveTurret(){
        currentNumTurrets--;
        turretCountRemainingText.SetText($"Current remaining turrets allowed: {maxNumTurrets-currentNumTurrets}");
    }

    public int GetCurrentNumTurrets(){
        return currentNumTurrets;
    }

    public int GetCurrentNumAbilities(){
        return currentNumAbilities;
    }

}
