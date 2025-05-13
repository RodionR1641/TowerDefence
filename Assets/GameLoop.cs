using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Collections;

//repsonsible for wave scheduling and managing the gameplay progression
public class GameLoop : MonoBehaviour
{
    [SerializeField] private GameObject baseEnemyPrefab;
    [SerializeField] private GameObject armorEnemyPrefab;
    [SerializeField] private GameObject miniBossEnemyPrefab;
    [SerializeField] private GameObject spawnPointRight;
    [SerializeField] private GameObject spawnPointLeft;
    [SerializeField] private TMP_Text waveText; 
    public Waypoint startingWaypointRight;
    public Waypoint startingWaypointLeft;

    public int numWaves = 5;

    private int currentWave = 0;

    private float waveCooldown = 4f; //cooldown between waves
    private float miniBossCooldown = 10f;//how much to wait after spawning miniboss and before spawning next enemies

    //A list per wave, stores the minEnemies,maxEnemies, spawnRate, enemySpeed
    private List<Dictionary<string, float>> enemyWaves = new List<Dictionary<string, float>>();

    private List<EnemyController> activeEnemies = new List<EnemyController>();

    void Start()
    {
        InitialiseWaves();
        StartCoroutine(WaveScheduler());
    }

    //list of stats related to waves
    void InitialiseWaves(){
        enemyWaves.Add(new Dictionary<string, float>
        {
            { "minEnemies", 4 }, //min and max here are for number of enemy burst groups
            { "maxEnemies", 4 },
            { "spawnRate", 5 },
            { "enemySpeed", 3 },
            { "waveCompletionReward", 10},
            {"leftSideEnemies",0}, //chance of an enemy spawning at the left side of map
            {"armorEnemyChance",0},
            {"burstDelayModifier",0.6f}, //time between each enemy spawning inside the burst
            {"miniBoss",0} //if 1 -> miniboss 

        });

        enemyWaves.Add(new Dictionary<string, float>
        {
            { "minEnemies", 5 },
            { "maxEnemies", 6 },
            { "spawnRate", 4 },
            { "enemySpeed", 3.5f },
            { "waveCompletionReward", 10},
            {"leftSideEnemies",0.3f},
            {"armorEnemyChance",0},
            {"burstDelayModifier",0.3f},
            {"miniBoss",0}
        });

        enemyWaves.Add(new Dictionary<string, float>
        {
            { "minEnemies", 6 },
            { "maxEnemies", 8 },
            { "spawnRate", 4 },
            { "enemySpeed", 4 },
            { "waveCompletionReward", 15},
            {"leftSideEnemies",0.4f},
            {"armorEnemyChance",0.3f},
            {"burstDelayModifier",0.3f},
            {"miniBoss",0}
        });

        enemyWaves.Add(new Dictionary<string, float>
        {
            { "minEnemies", 8 },
            { "maxEnemies", 10 },
            { "spawnRate", 3 },
            { "enemySpeed", 4 },
            { "waveCompletionReward", 15},
            {"leftSideEnemies",0.3f},
            {"armorEnemyChance",0.4f},
            {"burstDelayModifier",0.3f},
            {"miniBoss",1}
        });

        enemyWaves.Add(new Dictionary<string, float>
        {
            { "minEnemies", 12 },
            { "maxEnemies", 12 },
            { "spawnRate", 3 },
            { "enemySpeed", 4.5f },
            { "waveCompletionReward", 10},
            {"leftSideEnemies",0.5f},
            {"armorEnemyChance",0.6f},
            {"burstDelayModifier",0.25f},
            {"miniBoss",1}
        });
    }


    //game logic is as this: spawn a Wave. Then check until all enemies have died, get reward for wave and wait a bit before
    //starting next wave
    private IEnumerator WaveScheduler(){
        //wait a bit before starting waves to start the game
        yield return new WaitForSeconds(waveCooldown*2);

        Debug.Log("WAVES: Starting Waves");
        while(currentWave < numWaves){
            yield return StartCoroutine(SpawnWave());

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
        GameStats.Instance.EndGame(true);//won game once all waves done
    }

    //spawn a random number of enemies and bursts of enemies in the wave. Enemies spawn quickly in bursts, then there
    // is a cooldown period until next burst
    private IEnumerator SpawnWave(){
        Dictionary<string,float> waveData = enemyWaves[currentWave];
        int numEnemies = Random.Range((int)waveData["minEnemies"],(int)waveData["maxEnemies"]+1);
        //Debug.Log($"WAVES: Starting wave {currentWave+1} with {numEnemies} groups of enemies");

        //dynamic wave phases
        int burstSize = Random.Range(3,5);//number of enemies spawned in a "burst"  
        float burstDelay = waveData["spawnRate"] * waveData["burstDelayModifier"];
        float cooldownTime = Random.Range(2f,5f);

        waveText.SetText($"Wave {currentWave+1} - Number Enemy Groups = {numEnemies}");

        if(waveData["miniBoss"] == 1){
            SpawnMiniBoss(waveData);
            yield return new WaitForSeconds(miniBossCooldown);
        }

        for(int i=0;i<numEnemies;i++){

            //burst time
            for(int j=0;j<burstSize;j++){
                SpawnEnemy(waveData);
                yield return new WaitForSeconds(burstDelay);
            }
            yield return new WaitForSeconds(cooldownTime); //cooldown between waves
            
            burstSize = Random.Range(3,5);
            cooldownTime = Random.Range(2f,4f);
        }
    }

    //spawn an enemy with a chance of left or right side
    void SpawnEnemy(Dictionary<string,float> waveData) 
    { 
        //randomly choose starting point
        GameObject spawnPoint;
        Waypoint startingWaypoint;
        float leftChance = waveData["leftSideEnemies"];

        if(Random.Range(0f,1f) > leftChance){
            spawnPoint = spawnPointRight;
            startingWaypoint = startingWaypointRight;
        }
        else{
            spawnPoint = spawnPointLeft;
            startingWaypoint = startingWaypointLeft;
        }
        //now randomly choose enemy type based on armor probability
        GameObject enemyPrefab = Random.Range(0f, 1f) > waveData["armorEnemyChance"] ? baseEnemyPrefab : armorEnemyPrefab;
        
        InstantiateEnemy(waveData,enemyPrefab,spawnPoint,startingWaypoint);
    }

    void SpawnMiniBoss(Dictionary<string,float> waveData){
        InstantiateEnemy(waveData,miniBossEnemyPrefab,spawnPointRight,startingWaypointRight);
    }


    //instantiate an enemy at a specified spawn point, and pass the next waypoint it should go towards
    void InstantiateEnemy(Dictionary<string,float> waveData, GameObject enemyPrefab,
     GameObject spawnPoint ,Waypoint startingWaypoint){
        
        GameObject enemyObject = Instantiate(
            enemyPrefab,
            spawnPoint.transform.position,
            spawnPoint.transform.rotation
        );

        EnemyController enemy = enemyObject.GetComponent<EnemyController>();
        enemy.waypoint = startingWaypoint;
        enemy.OnEnemyDied += HandleEnemyDeath; //subsribe to the event of death 
        enemy.SetSpeed(waveData["enemySpeed"]); //waves vary the speed of enemies
        activeEnemies.Add(enemy);
    }

    //a way to link the enemy dying code of Enemy class and wave manager
    void HandleEnemyDeath(EnemyController enemy){
        enemy.OnEnemyDied -= HandleEnemyDeath; //unscubscribe
        activeEnemies.Remove(enemy);
    }
}
