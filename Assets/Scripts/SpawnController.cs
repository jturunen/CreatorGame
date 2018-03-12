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

    public MonsterList allMonsters;
    public LevelController levelController;

    public List<Vector2> positionList;
    public List<GameObject> placeableEnemies;

    public int maxUnits = 0;
    public int currentUnit = 0;
    private bool monsterSpawning = false;

    public float spawnRate = 0.5F;
    private float nextSpawn = 0.0F;

    // Use this for initialization
    void Start () {

        chosenMonstersList = new List<GameObject>();
        placeableEnemies = allMonsters.allMonsters;
        positionList = levelController.spawnPoints;

    }

    // Update is called once per frame
    void Update()
    {
        if (currentUnit <= maxUnits && Time.time > nextSpawn) { 
            if (Input.GetButton("Fire1"))
            {
                Debug.Log("A button pressed");

                SpawnToPoint(placeableEnemies[0]);
            }
            if (Input.GetButton("Fire2"))
            {
                Debug.Log("B button pressed");
                SpawnToPoint(placeableEnemies[1]);
            }
            if (Input.GetButton("Fire3"))
            {
                Debug.Log("X button pressed");
                SpawnToPoint(placeableEnemies[2]);
            }
            if (Input.GetButton("Fire4"))
            {
                Debug.Log("Y button pressed");
                SpawnToPoint(placeableEnemies[3]);
            }
            if (Input.GetButton("Left Bumber"))
            {
                Debug.Log("Left bumber pressed");
                SpawnToPoint(placeableEnemies[4]);
            }
            if (Input.GetButton("Right Bumber"))
            {
                Debug.Log("Right bumber pressed");
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

        currentUnit++;
    }

    private void SpawnUnitsToPoints()
    {
        for (int i = 0; i < chosenMonstersList.Count; i++)
        {
            Instantiate(chosenMonstersList[i], positionList[i], Quaternion.identity);
        }
        monsterSpawning = true;
    }
}