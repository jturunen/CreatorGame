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

    public float spawnerMinX, spawnerMinY, spawnerMaxX, spawnerMaxY;

    public bool SpawnerMode = true; //For testing, disable or activate spawnermode

    public int maxMinions = 0; //How many units can be spawned to map

    private int currentMinionCount = 0; // Which minion(count) is currently being used 
    private bool minionSpawning = false; //Check to prevent multiple minion spawns

    public float spawnRate = 0.5F; // How fast can be minions put to list (spawned to list)
    private float nextSpawn = 0.0F;
    private bool fighting = false; // Has creating phase ended an d fighting begun

    public GameObject spawner; // Prefab for spawner point

    private List<GameObject> chosenMinionsList = null; //List where chosen minions are put

    private List<GameObject> minionsFromPrefabs = new List<GameObject>(); // List for minions from prefabs

    public float waitTime = 1; // Time before the minions spawn

    private List<GameObject> spawnPointObjectList = new List<GameObject>(); //Store spawnpoints so that they can be found when inactivated

    //Spawnpoint variables
    private List<Vector2> minimapSpawnPointPositions = new List<Vector2>(); //List for minimap spawnpoint positions
    private List<Vector2> levelSpawnPointPositions = new List<Vector2>(); //List for spawnpoint positions in the level
    public GameObject spawnPointPrefab; //Prefab for spawnpoint
    private GameObject spawnPointObject; //Temporary spawnpoint object to name it on instantiate
    private GameObject spawnPointToFind; //Temporary spawnpoint object to find it on scene


    #endregion

    // Use this for initialization
    void Start() {
        DontDestroyOnLoad(this.gameObject);
        
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
    }

    // Update is called once per frame
    void Update()
    {
        if (currentMinionCount <= maxMinions && Time.time > nextSpawn && !fighting) {
            //TODO: change buttons to Fire1 etc. after the 4th controller 
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                SpawnToPoint(minionsFromPrefabs[0]);
                SoundManagerController.instance.PlaySound("Hit", 1f);
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                SpawnToPoint(minionsFromPrefabs[1]);
                SoundManagerController.instance.PlaySound("Hit", 1f);
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                SpawnToPoint(minionsFromPrefabs[2]);
                SoundManagerController.instance.PlaySound("Reload", 1f);
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
                SceneManager.LoadScene("Chinatown", LoadSceneMode.Single);
                fighting = true; //Creating phase is over, start fighting
            }
            else if (SceneManager.GetActiveScene().name == "Chinatown")
            {
                waitTime -= Time.deltaTime;
                if (waitTime < 0)
                {
                    SpawnUnitsToPoints();
                }
            }
        }
    }

    private void SpawnToPoint(GameObject chosenEnemy)
    {
        nextSpawn = Time.time + spawnRate;
        //Add chosen enemy to list.
        chosenMinionsList.Add(chosenEnemy);
        //Change "current" spawnpoint to used
        changeSpawnPointColor(true);
        currentMinionCount++;
        //Change next spawnpoint to be current
        if (currentMinionCount < maxMinions)
        {
            changeSpawnPointColor(false);
        }
    }

    //This is activated when the camera moves to the fight phase.
    private void SpawnUnitsToPoints()
    {
        //go throught the list, and instantiate to the spawning point
        for (int i = 0; i < chosenMinionsList.Count; i++)
        {
            if(SpawnerMode)
            {
                //Spawn spawner points
                (Instantiate(spawner, levelSpawnPointPositions[i], Quaternion.identity) as GameObject).GetComponent<SpawnerController>().setMinion(chosenMinionsList[i]);
            } else
            {
                // --------------spawn only one mob per point-------------------------------
                GameObject minion = chosenMinionsList[i];
                EnemyController c = minion.GetComponent<EnemyController>();

                if (minion.name == "Enemy")
                {
                    Instantiate(chosenMinionsList[i], levelSpawnPointPositions[i], Quaternion.identity);
                }
                if (i == 3)
                {
                    c.isControlled = true;
                    Instantiate(minion, levelSpawnPointPositions[i], Quaternion.identity);

                }
                else
                {
                    c.isControlled = false;
                    Instantiate(chosenMinionsList[i], levelSpawnPointPositions[i], Quaternion.identity);
                }
            }
        }
        //Enable minionSpawning so to prevent update to spawn enemies multiple times
        minionSpawning = true;
    }

    private void changeSpawnPointColor(bool toUsed)
    {
        //Find current spawnpoint from the scene and access its sprite
        //spawnPointToFind = GameObject.Find("SpawnPoint" + currentMinionCount);
        spawnPointToFind = spawnPointObjectList[currentMinionCount];
        SpriteRenderer spawnPointSprite = spawnPointToFind.GetComponent<SpriteRenderer>();
        if (toUsed)
        {
            Animator spawnPointAnimator = spawnPointToFind.GetComponent<Animator>();
            spawnPointAnimator.enabled = false;
            //Change to chosen sprite minion, (and back to white)
            spawnPointSprite.color = new Color(1f, 1f, 1f, 1f);
            //Find the chosen minion from the list and get its sprite
            //spawnPointSprite.sprite = chosenMinionsList[currentMinionCount].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
        } else
        {
            spawnPointToFind.SetActive(true);
            //Change to next, (red)
            spawnPointSprite.color = new Color(1f, 0f, 0f, 1f);
        }

    }

    private void createSpawnPoints()
    {
        //Creating list for minimap spawnpoints (Temporary solution)
        minimapSpawnPointPositions.Add(new Vector2(-7.5f, -2.5f));
        // minimapSpawnPointPositions.Add(new Vector2(-5.5f, -2));
        minimapSpawnPointPositions.Add(new Vector2(-2.5f, -2.5f));
        minimapSpawnPointPositions.Add(new Vector2(-7.5f, -4));
        // minimapSpawnPointPositions.Add(new Vector2(-5.5f, -4));
        minimapSpawnPointPositions.Add(new Vector2(-2.5f, -4));

        //Creating list for spawnpoints on the level (where chosen minions will be spawned) (Temporary solution)
        levelSpawnPointPositions.Add(new Vector2(spawnerMinX, spawnerMaxY));
        // minimapSpawnPointPositions.Add(new Vector2(-5.5f, -2));
        levelSpawnPointPositions.Add(new Vector2(spawnerMaxX, spawnerMaxY));
        levelSpawnPointPositions.Add(new Vector2(spawnerMinX, spawnerMinY));
        // minimapSpawnPointPositions.Add(new Vector2(-5.5f, -4));
        levelSpawnPointPositions.Add(new Vector2(spawnerMaxX, spawnerMinY));

        //Go through given coordiatens and instatiate a spawnpoint to that location
        for (int i = 0; i < maxMinions; i++)
        {
            spawnPointObject = Instantiate(spawnPointPrefab, minimapSpawnPointPositions[i], Quaternion.identity);
            //give spawnpoint a name with the number, to indentify
            spawnPointObject.name = "SpawnPoint" + i;
            spawnPointObjectList.Add(spawnPointObject);
            if (i > 0)
            {
                spawnPointObject.SetActive(false);
            }
            //Debug.Log("Created SPAWNPOINT: " + i);

        }
        //Set first spawnpoint color to be next used.
        changeSpawnPointColor(false);
    }


}