using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnController : MonoBehaviour {

    /*
    * Spawn enemies from list:
    * When button is pressed, monster is saved to list with position.
    * Then for example, when players enter room, go through list and spawn monsters then.
    */
    #region Variables

    public int maxUnits = 0; //How many units can be spawned to map
    private int currentUnit = 0; // Which unit(slot) is currently being used 
    private bool minionSpawning = false; //Check to prevent multiple minion spawns

    public float spawnRate = 0.5F; // How fast can be minions put to list (spawned to list)
    private float nextSpawn = 0.0F; 

    private List<GameObject> chosenMinionsList = null; //List where chosen minions are put

    private List<GameObject> minionsFromPrefabs = new List<GameObject>(); // List for minions from prefabs

    //Spawnpoint variables
    private List<Vector2> spawnPointPositions = new List<Vector2>(); //List for spawnpoint positions
    public GameObject spawnPointPrefab; //Prefab for spawnpoint
    private GameObject spawnPointObject; //Temporary spawnpoint object to name it on instantiate
    private GameObject spawnPointToFind; //Temporary spawnpoint object to find it on scene

    #endregion

    // Use this for initialization
    void Start() {

        // Get Minions folder as object.
        Object[] subListObjects = Resources.LoadAll("Prefabs/Characters", typeof(GameObject));

        // Get all minion prefabs from folder "Minions" and put to list
        foreach (GameObject subListObject in subListObjects)
        {
            GameObject listObject = subListObject;
            Debug.Log("Created minion: " + listObject);
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
        if (currentUnit <= maxUnits && Time.time > nextSpawn) { 
            if (Input.GetButton("Fire1"))
            {
                Debug.Log("Created minion Fire1: " + minionsFromPrefabs[0]);
                SpawnToPoint(minionsFromPrefabs[0]);
            }
            if (Input.GetButton("Fire2"))
            {
                SpawnToPoint(minionsFromPrefabs[1]);
            }
            if (Input.GetButton("Fire3"))
            {
                SpawnToPoint(minionsFromPrefabs[2]);
            }
            if (Input.GetButton("Fire4"))
            {
                SpawnToPoint(minionsFromPrefabs[3]);
            }
            /*if (Input.GetButton("Left Bumber"))
            {
                SpawnToPoint(minionsFromPrefabs[4]);
            }
            if (Input.GetButton("Right Bumber"))
            {
                SpawnToPoint(minionsFromPrefabs[5]);
            }*/
        }
        // Spawn enemies from list
        if (currentUnit == maxUnits && !minionSpawning)
        {
            SpawnUnitsToPoints();
        }
    }

    private void SpawnToPoint(GameObject chosenEnemy)
    {
        nextSpawn = Time.time + spawnRate;
        //Add chosen enemy to list.
        chosenMinionsList.Add(chosenEnemy);
        //Change "current" spawnpoint to used
        changeSpawnPointColor(toUsed: true);
        currentUnit++;
        //Change next spawnpoint to be current
        if (currentUnit < maxUnits)
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
            Instantiate(chosenMinionsList[i], spawnPointPositions[i], Quaternion.identity);
        }
        //Enable minionSpawning so to prevent update to spawn enemies multiple times
        minionSpawning = true;
    }

    private void changeSpawnPointColor(bool toUsed)
    {
        //Find current spawnpoint from the scene and access its sprite
        spawnPointToFind = GameObject.Find("SpawnPoint" + currentUnit);
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

    //Temporary solution, until we get real positions that changes with maps.
    private void createSpawnPoints()
    {
        //Creating temporary list, the positions must be placed better
        spawnPointPositions.Add(new Vector2(-7.5f, -2));
        spawnPointPositions.Add(new Vector2(-5.5f, -2));
        spawnPointPositions.Add(new Vector2(-3.5f, -2));
        spawnPointPositions.Add(new Vector2(-7.5f, -4));
        spawnPointPositions.Add(new Vector2(-5.5f, -4));
        spawnPointPositions.Add(new Vector2(-3.5f, -4));

        //Go thrgouh given coordiatens and instatiate a spawnpoint to that location
        for (int i = 0; i < maxUnits; i++)
        {
            spawnPointObject = Instantiate(spawnPointPrefab, spawnPointPositions[i], Quaternion.identity);
            //give spawnpoint a name with the number, to indentify
            spawnPointObject.name = "SpawnPoint" + i;
            Debug.Log("Created SPAWNPOINT: " + i);

        }
    }
}