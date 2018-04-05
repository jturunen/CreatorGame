using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnController : MonoBehaviour {

    /*
    * Spawn enemies from list:
    * When button is pressed, monster is saved to list with position.
    * Then for example, when players enter room, go through list and spawn monsters then.
    */
    #region Variables
    public int maxMinions = 0; //How many units can be spawned to map

    private int currentMinionCount = 0; // Which minion(count) is currently being used 
    private bool minionSpawning = false; //Check to prevent multiple minion spawns

    public float spawnRate = 0.5F; // How fast can be minions put to list (spawned to list)
    private float nextSpawn = 0.0F; 

    private List<GameObject> chosenMinionsList = null; //List where chosen minions are put

    private List<GameObject> minionsFromPrefabs = new List<GameObject>(); // List for minions from prefabs

    private static bool created = false;

    //Spawnpoint variables
    private List<Vector2> minimapSpawnPointPositions = new List<Vector2>(); //List for minimap spawnpoint positions
    private List<Vector2> levelSpawnPointPositions = new List<Vector2>(); //List for spawnpoint positions in the level
    public GameObject spawnPointPrefab; //Prefab for spawnpoint
    private GameObject spawnPointObject; //Temporary spawnpoint object to name it on instantiate
    private GameObject spawnPointToFind; //Temporary spawnpoint object to find it on scene

    #endregion

    // Use this for initialization
    void Start() {
        if (!created)
        {
            DontDestroyOnLoad(this.gameObject);
            created = true;
        }
            // Get Minions folder as object.
            Object[] subListObjects = Resources.LoadAll("Prefabs/Characters", typeof(GameObject));

        // Get all minion prefabs from folder "Minions" and put to list
        foreach (GameObject subListObject in subListObjects)
        {
            GameObject listObject = subListObject;
            //Debug.Log("Created minion: " + listObject);
            minionsFromPrefabs.Add(listObject);
        }

        //Create spawnpoints for level minimap
        //Positions are found in spawnPointPositions list
        createSpawnPoints();

        //Initialize list for minions that will be chosen to the level
        chosenMinionsList = new List<GameObject>();

        //Set first spawnpoint color to be next used.
        changeSpawnPointColor(toUsed: false);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentMinionCount <= maxMinions && Time.time > nextSpawn) {
            //TODO: change buttons to Fire1 etc. after the 4th controller 
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                SpawnToPoint(minionsFromPrefabs[0]);
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                SpawnToPoint(minionsFromPrefabs[1]);
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                SpawnToPoint(minionsFromPrefabs[2]);
            }
            /*if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                SpawnToPoint(minionsFromPrefabs[3]);
            }
            if (Input.GetButton("Left Bumber"))
            {
                SpawnToPoint(minionsFromPrefabs[4]);
            }
            if (Input.GetButton("Right Bumber"))
            {
                SpawnToPoint(minionsFromPrefabs[5]);
            }*/
        }
        // Spawn enemies from list
        if (currentMinionCount == maxMinions && !minionSpawning)
        {
            if (SceneManager.GetActiveScene().name == "creatingPhaseScene")
            {
                SceneManager.LoadScene("fightScene", LoadSceneMode.Single);
            } else if (SceneManager.GetActiveScene().name == "fightScene")
            {
                SpawnUnitsToPoints();
            }

        }
    }

    private void SpawnToPoint(GameObject chosenEnemy)
    {
        nextSpawn = Time.time + spawnRate;
        //Add chosen enemy to list.
        chosenMinionsList.Add(chosenEnemy);
        //Change "current" spawnpoint to used
        changeSpawnPointColor(toUsed: true);
        currentMinionCount++;
        //Change next spawnpoint to be current
        if (currentMinionCount < maxMinions)
        {
            changeSpawnPointColor(toUsed: false);
        }
    }

    //This is activated when the camera moves to the fight phase.
    private void SpawnUnitsToPoints()
    {
        //go throught the list, and instantiate to them to the spawning point
        for (int i = 0; i < chosenMinionsList.Count; i++)
        {
            if(i ==3)
            {
                GameObject minion = chosenMinionsList[i];
                EnemyController c = minion.GetComponent<EnemyController>();
                c.isControlled = true;
                Instantiate(minion, levelSpawnPointPositions[i], Quaternion.identity);
            } else
            {
                Instantiate(chosenMinionsList[i], levelSpawnPointPositions[i], Quaternion.identity);
            }
        }
        //Enable minionSpawning so to prevent update to spawn enemies multiple times
        minionSpawning = true;
    }

    private void changeSpawnPointColor(bool toUsed)
    {
        //Find current spawnpoint from the scene and access its sprite
        spawnPointToFind = GameObject.Find("SpawnPoint" + currentMinionCount);
        SpriteRenderer spawnPointSprite = spawnPointToFind.GetComponent<SpriteRenderer>();
        if (toUsed)
        {
            //Change to used, (black)
            spawnPointSprite.color = new Color(0f, 0f, 0f, 1f);
        } else
        {
            //Change to next, (red)
            spawnPointSprite.color = new Color(1f, 0.5f, 0.5f, 1f);
        }

    }

    private void createSpawnPoints()
    {
        //Creating list for minimap spawnpoints (Temporary solution)
        minimapSpawnPointPositions.Add(new Vector2(-7.5f, -3));
        // minimapSpawnPointPositions.Add(new Vector2(-5.5f, -2));
        minimapSpawnPointPositions.Add(new Vector2(-2.5f, -3));
        minimapSpawnPointPositions.Add(new Vector2(-7.5f, -4));
        // minimapSpawnPointPositions.Add(new Vector2(-5.5f, -4));
        minimapSpawnPointPositions.Add(new Vector2(-2.5f, -4));

        //Creating list for spawnpoints on the level (where chosen minions will be spawned) (Temporary solution)
        levelSpawnPointPositions.Add(new Vector2(-6, 0));
        // minimapSpawnPointPositions.Add(new Vector2(-5.5f, -2));
        levelSpawnPointPositions.Add(new Vector2(6, 0));
        levelSpawnPointPositions.Add(new Vector2(-6, -4));
        // minimapSpawnPointPositions.Add(new Vector2(-5.5f, -4));
        levelSpawnPointPositions.Add(new Vector2(6, -4));

        //Go through given coordiatens and instatiate a spawnpoint to that location
        for (int i = 0; i < maxMinions; i++)
        {
            spawnPointObject = Instantiate(spawnPointPrefab, minimapSpawnPointPositions[i], Quaternion.identity);
            //give spawnpoint a name with the number, to indentify
            spawnPointObject.name = "SpawnPoint" + i;
            //Debug.Log("Created SPAWNPOINT: " + i);

        }
    }


}