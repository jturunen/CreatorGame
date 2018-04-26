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

    public static SpawnController instance;

    [Tooltip("Positions where Spawners will spawn. X is horizontal, Y is vertical")]
    public float spawnerMinX, spawnerMinY, spawnerMaxX, spawnerMaxY;
    [Tooltip("For disabling the spawner mode and activating single spawns")]
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

    public int maxGoldfishSelections, maxTrashySelections, maxBigsySelections; //Restrict how many of minion type can be chosen to the map

    private int goldFishSelections, trashySelections, bigsySelections; //How many of minion type is chosen

    public GameObject minimap;
    public Sprite sea, chinatown, bar;
    private int round;

    //Spawnpoint variables
    private List<Vector2> minimapSpawnPointPositions = new List<Vector2>(); //List for minimap spawnpoint positions
    private List<Vector2> levelSpawnPointPositions = new List<Vector2>(); //List for spawnpoint positions in the level
    public GameObject spawnPointPrefab; //Prefab for spawnpoint
    private GameObject spawnPointObject; //Temporary spawnpoint object to name it on instantiate
    private GameObject spawnPointToFind; //Temporary spawnpoint object to find it on scene

    //Sprites for selections
    public Sprite trashy, trashyDisabled, goldfish, goldfishDisabled, bigsy, bigsyDisabled;
    GameObject trashySpriteObject;
    GameObject bigsySpriteObject;
    GameObject goldfishSpriteObject;
    #endregion

    // Use this for initialization
    void Start() {
        DontDestroyOnLoad(this.gameObject);
        instance = this;
        round = RoundController.roundIndex;
        if(round == 0)
        {
            minimap.GetComponent<SpriteRenderer>().sprite = sea;
            maxMinions = 2;
        }
        if (round == 1)
        {
            minimap.GetComponent<SpriteRenderer>().sprite = bar;
            maxMinions = 3;
        }
        if (round == 2)
        {
            minimap.GetComponent<SpriteRenderer>().sprite = chinatown;
            maxMinions = 4;
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

        //Find the sprite objects to set the sprites (for disabled sprite)
        trashySpriteObject = GameObject.Find("selection_minion_trashy");
        bigsySpriteObject = GameObject.Find("selection_minion_shark");
        goldfishSpriteObject = GameObject.Find("selection_minion_goldfish");



    }

    // Update is called once per frame
    void Update()
    {
        #region SpawingFromButtons
        if (currentMinionCount <= maxMinions && Time.time > nextSpawn && !fighting) {
            //TODO: change buttons to Fire1 etc. after the 4th controller 
            #region firstMinion
            if (Input.GetKeyDown("joystick 2 button 2") || Input.GetKeyDown(KeyCode.UpArrow))
            { 
                if(bigsySelections < maxBigsySelections)
                {
                    bigsySelections++;
                    SpawnToPoint(minionsFromPrefabs[0]);
                    SoundManagerController.instance.PlaySound("Hit", 1f);
                    if (bigsySelections == maxBigsySelections)
                    {
                        //Show that selection is disabled
                        Debug.Log("Bigsy disabled");
                        bigsySpriteObject.GetComponent<SpriteRenderer>().sprite = bigsyDisabled;
                    }
                } else
                {

                    SoundManagerController.instance.PlaySound("Denied", 1f);
                }
            }
            #endregion
            #region secondMinion

            if (Input.GetKeyDown("joystick 2 button 1") || Input.GetKeyDown(KeyCode.RightArrow) && trashySelections < maxTrashySelections)
            {
                trashySelections++;
                SpawnToPoint(minionsFromPrefabs[1]);
                if (trashySelections == maxTrashySelections)
                {
                    //Show that selection is disabled
                    Debug.Log("Trashy disabled");
                    trashySpriteObject.GetComponent<SpriteRenderer>().sprite = trashyDisabled;

                }
                SoundManagerController.instance.PlaySound("Hit", 1f);
            }
            #endregion
            #region secondMinion
            if (Input.GetKeyDown("joystick 2 button 0") || Input.GetKeyDown(KeyCode.LeftArrow) && goldFishSelections < maxGoldfishSelections)
            {
                goldFishSelections++;
                SpawnToPoint(minionsFromPrefabs[2]);
                if (goldFishSelections == maxGoldfishSelections)
                {
                    //Show that selection is disabled
                    Debug.Log("Goldfish disabled");
                    goldfishSpriteObject.GetComponent<SpriteRenderer>().sprite = goldfishDisabled;

                }
                SoundManagerController.instance.PlaySound("Reload", 1f);
            }
            #endregion
            #endregion

        }
        // Spawn enemies from list
        if (currentMinionCount == maxMinions && !minionSpawning)
        {
            fighting = true; //Creating phase is over, start fighting

            waitTime -= Time.deltaTime;
            if (waitTime < 0)
            {
                if (SceneManager.GetActiveScene().name == "creationPhase")
                {
                    switch (round)
                    {
                        case 0:
                            SceneManager.LoadScene("FirstScene", LoadSceneMode.Single);
                            break;
                        case 1:
                            SceneManager.LoadScene("Bar", LoadSceneMode.Single);
                            break;
                        case 2:
                            SceneManager.LoadScene("Chinatown", LoadSceneMode.Single);
                            break;
                    }
                }
                else if (SceneManager.GetActiveScene().name != "creationPhase")
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
                #region spawnOnlyOne
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
                #endregion
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
        spawnPointToFind.transform.localScale = new Vector2(0.7f,0.7f);
        SpriteRenderer spawnPointSprite = spawnPointToFind.GetComponent<SpriteRenderer>();
        if (toUsed)
        {
            Animator spawnPointAnimator = spawnPointToFind.GetComponent<Animator>();
            spawnPointAnimator.enabled = false;
            //Change to chosen sprite minion, (and back to white)
            spawnPointSprite.color = new Color(1f, 1f, 1f, 1f);
            //Find the chosen minion from the list and get its sprite
            spawnPointSprite.sprite = chosenMinionsList[currentMinionCount].GetComponent<SpriteRenderer>().sprite;
        } else
        {
            spawnPointToFind.SetActive(true);
            //Change to next, (red)
            //spawnPointSprite.color = new Color(1f, 0f, 0f, 1f);
        }

    }

    private void createSpawnPoints()
    {
        //Creating list for minimap spawnpoints (Temporary solution)
        minimapSpawnPointPositions.Add(new Vector2(-2.5f, -2f));
        // minimapSpawnPointPositions.Add(new Vector2(-5.5f, -2));
        minimapSpawnPointPositions.Add(new Vector2(3f, -2));
        minimapSpawnPointPositions.Add(new Vector2(-2.5f, -4));
        // minimapSpawnPointPositions.Add(new Vector2(-5.5f, -4));
        minimapSpawnPointPositions.Add(new Vector2(3f, -4));

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