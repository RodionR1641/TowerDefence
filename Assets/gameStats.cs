using TMPro;
using Unity.VisualScripting;
using UnityEngine;

// Class responsible for the base health and economy mechanics. It is a singleton
public class GameStats : MonoBehaviour
{
    [SerializeField] private int baseHealth = 25;
    [SerializeField] private float currentMoney = 100;//enough for 1 laser turret at the start
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI healthText;
    public const int maxNumTurrets = 1;
    private int currentNumTurrets = 0;

    private static GameStats _instance;
    public static GameStats Instance {get {return _instance;}}


    private void Start()
    {
        moneyText.SetText($"£{currentMoney}");
        healthText.SetText($"Heart {baseHealth}");

    }

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

    public void ChangeHealth(int health){
        baseHealth += health;
        healthText.SetText($"Health {baseHealth}");
    }

    public int GetBaseHealth(){
        return baseHealth;
    }

    public void AddTurret(){
        currentNumTurrets++;
    }

    public void RemoveTurret(){
        currentNumTurrets--;
    }

    public int GetCurrentNumTurrets(){
        return currentNumTurrets;
    }

}
