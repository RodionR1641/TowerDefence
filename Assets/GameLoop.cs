using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Collections;

public class GameLoop : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    public Waypoint firstWaypoint;

    public int numWaves = 5;

    private int currentWave = 0;

    private float waveCooldown = 2f; //cooldown between waves

    //A list per wave, stores the minEnemies,maxEnemies, spawnRate, enemySpeed
    private List<Dictionary<string, float>> enemyWaves = new List<Dictionary<string, float>>();

    private List<EnemyController> activeEnemies = new List<EnemyController>();

    void Start()
    {
        InitialiseWaves();
        StartCoroutine(WaveScheduler());
    }


    void InitialiseWaves(){
        enemyWaves.Add(new Dictionary<string, float>
        {
            { "minEnemies", 5 },
            { "maxEnemies", 10 },
            { "spawnRate", 5 },
            { "enemySpeed", 3 },
            { "waveCompletionReward", 5}
        });

        enemyWaves.Add(new Dictionary<string, float>
        {
            { "minEnemies", 10 },
            { "maxEnemies", 15 },
            { "spawnRate", 5 },
            { "enemySpeed", 3 },
            { "waveCompletionReward", 10}
        });

        enemyWaves.Add(new Dictionary<string, float>
        {
            { "minEnemies", 15 },
            { "maxEnemies", 20 },
            { "spawnRate", 4 },
            { "enemySpeed", 4 },
            { "waveCompletionReward", 5}
        });

        enemyWaves.Add(new Dictionary<string, float>
        {
            { "minEnemies", 20 },
            { "maxEnemies", 23 },
            { "spawnRate", 3 },
            { "enemySpeed", 4 },
            { "waveCompletionReward", 15}
        });

        enemyWaves.Add(new Dictionary<string, float>
        {
            { "minEnemies", 23 },
            { "maxEnemies", 26 },
            { "spawnRate", 3 },
            { "enemySpeed", 4.5f },
            { "waveCompletionReward", 10}
        });
    }


    private IEnumerator WaveScheduler(){

        //wait a bit before starting waves to start the game
        yield return new WaitForSeconds(waveCooldown*2);

        Debug.Log("WAVES: Starting Waves");
        while(currentWave < numWaves){
            yield return StartCoroutine(SpawnWave(currentWave));

            //wait for all enemies to die before starting next wave
            yield return new WaitUntil(() => activeEnemies.Count == 0);

            Debug.Log($"WAVES: Wave {currentWave+1} completed!");

            //receive money for completing the wave
            GameStats.Instance.ChangeMoney((int)enemyWaves[currentWave]["waveCompletionReward"]);

            //make sure there is time for the player to prepare for the next wave
            yield return new WaitForSeconds(waveCooldown);

            currentWave++;

        }
        Debug.Log("WAVES: Waves Completed");
        //TODO: can implement a Game Finish code here  
    }


    private IEnumerator SpawnWave(int waveIndex){
        Dictionary<string,float> waveData = enemyWaves[waveIndex];

        int numEnemies = Random.Range((int)waveData["minEnemies"],(int)waveData["maxEnemies"]);

        Debug.Log($"WAVES: Starting wave {waveIndex} with {numEnemies} enemies");

        for(int i=0;i<numEnemies;i++){
            SpawnEnemy(waveData["enemySpeed"]);
            yield return new WaitForSeconds(waveData["spawnRate"]);
        }
    }

    void SpawnEnemy(float enemySpeed) 
    { 
        GameObject gameObject = Instantiate(enemyPrefab, 
        transform.position, transform.rotation); 
        EnemyController enemy = gameObject.GetComponent<EnemyController>(); 

        enemy.waypoint = firstWaypoint; 
        enemy.OnEnemyDied += HandleEnemyDeath; //subsribe to the event of death 
        enemy.SetSpeed(enemySpeed);
        activeEnemies.Add(enemy);
    }

    //a way to link the enemy dying code of Enemy class and wave manager
    void HandleEnemyDeath(EnemyController enemy){
        enemy.OnEnemyDied -= HandleEnemyDeath; //unscubscribe
        Debug.Log("WAVES: Removing enemy");
        activeEnemies.Remove(enemy);
    }
}
