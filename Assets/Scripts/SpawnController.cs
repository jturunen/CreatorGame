using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnController : MonoBehaviour {
     
     /*
     * Spawn enemies from list:
     * When button is pressed, monster is saved to list with position.
     * Then for example, when players enter room, go through list and spawn monsters then.
     */

    public List<GameObject> chosenMonstersList = null;

   // public MonsterList allMonsters;
    public GameObject gameController;
    public LevelController levelController;

    public List<Vector2> positionList;
    public List<GameObject> placeableEnemies;

    private GameObject spawnPointToFind;

    public int maxUnits = 0;
    public int currentUnit = 0;
    private bool monsterSpawning = false;

    public float spawnRate = 0.5F;
    private float nextSpawn = 0.0F;

    // Use this for initialization
    void Start() {

        chosenMonstersList = new List<GameObject>();
        placeableEnemies = new List<GameObject>();

        placeableEnemies = gameController.GetComponent<MonsterList>().allMonsters;

        Debug.Log("placeableEnemies" + placeableEnemies);
      //  Debug.Log("allMonsters.allMonsters " + allMonsters.allMonsters);

        positionList = levelController.spawnPoints;
        //changeSpawnPointColor(toUsed: false);

    }

    // Update is called once per frame
    void Update()
    {
        if (currentUnit <= maxUnits && Time.time > nextSpawn) { 
            if (Input.GetButton("Fire1"))
            {
                SpawnToPoint(placeableEnemies[0]);
            }
            if (Input.GetButton("Fire2"))
            {
                SpawnToPoint(placeableEnemies[1]);
            }
            if (Input.GetButton("Fire3"))
            {
                SpawnToPoint(placeableEnemies[2]);
            }
            if (Input.GetButton("Fire4"))
            {
                SpawnToPoint(placeableEnemies[3]);
            }
            if (Input.GetButton("Left Bumber"))
            {
                SpawnToPoint(placeableEnemies[4]);
            }
            if (Input.GetButton("Right Bumber"))
            {
                SpawnToPoint(placeableEnemies[5]);
            }
        } else if (currentUnit == maxUnits && !monsterSpawning)
        {
            // Spawn enemies from list - move this to room controller?
            SpawnUnitsToPoints();
        }
    }

    private void SpawnToPoint(GameObject chosenEnemy)
    {
        nextSpawn = Time.time + spawnRate;
        chosenMonstersList.Add(chosenEnemy);
        changeSpawnPointColor(toUsed: true);
        currentUnit++;
        if(currentUnit < maxUnits)
        {
            changeSpawnPointColor(toUsed: false);
        }
    }

    private void SpawnUnitsToPoints()
    {
        for (int i = 0; i < chosenMonstersList.Count; i++)
        {
            spawnPointToFind = GameObject.Find("SpawnPoint" + i);
            Destroy(spawnPointToFind);
            Instantiate(chosenMonstersList[i], positionList[i], Quaternion.identity);
        }
        monsterSpawning = true;
    }

    private void changeSpawnPointColor(bool toUsed)
    {
        spawnPointToFind = GameObject.Find("SpawnPoint" + currentUnit);
        SpriteRenderer spawnPointSprite = spawnPointToFind.GetComponent<SpriteRenderer>();
        if (toUsed)
        {
            spawnPointSprite.color = new Color(0f, 0f, 0f, 1f);
        } else
        {
            spawnPointSprite.color = new Color(1f, 0.5f, 0.5f, 1f);
        }

    }
}