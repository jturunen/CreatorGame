using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour {

    #region Variables

    public GameObject chosenMinion; //Minion that was chosen in spawn

    //Spawnrates for minions
    public float trashySpawnRate = 3; 
    public float goldfishSpawnRate = 7;
    public float bigsySpawnRate = 12;
   
    //Maximum spawns for minions
    public int trashyMaxSpawns = 4;
    public int goldfishMaxSpawns = 3;
    public int bigsyMaxSpawns = 2;

    private int maxSpawns = 5;

    private GameObject gameControllerObject; //Game controller for indexing
    private GameController gameController = null; //Script from gameController
    private int index = 0; //Index for spawned minion (not used currently)

    public float spawnRate = 0.1F; // How fast minions spawn
    private float nextSpawn = 0.0F;
    private bool noControl =true;

    #endregion
    // Use this for initialization
    void Start () {
        //Find the GameController object, for indexing the minions
        gameControllerObject = GameObject.Find("GameController");
        gameController = gameControllerObject.GetComponent<GameController>();
    }
	
	// Update is called once per frame
	void Update () {
        setControl();
        if (Time.time > nextSpawn && maxSpawns > 0)
        {
            nextSpawn = Time.time + spawnRate;
            //Spawn the minion to the spawnpoint position, and name it with index number from the gameController
            //(Instantiate(chosenMinion, transform.position, Quaternion.identity) as GameObject).GetComponent<EnemyController>().myName = gameController.minionIndex.ToString();
            (Instantiate(chosenMinion, transform.position, Quaternion.identity) as GameObject).name += gameController.minionIndex.ToString();

            //Add one to index
            gameController.minionIndex++;
            //Reduce one maxSpawns
            maxSpawns--;
        }
    }

    void setControl()
    {
        if (noControl)
        {
            GameObject minionToControl = GameObject.Find(chosenMinion.name +"(Clone)0");
            if(minionToControl != null)
            {
                minionToControl.GetComponent<EnemyController>().isControlled = true;
            }
            //Toggle
            noControl = !noControl;
        }
    }

    //Set what minion is spawned and stats according
    public void setMinion(GameObject minion)
    {
        chosenMinion = minion;

        switch (minion.name)
        {
            case "Enemy":
                spawnRate = trashySpawnRate;
                maxSpawns = trashyMaxSpawns;
                break;
            case "Goldfish":
                spawnRate = goldfishSpawnRate;
                maxSpawns = goldfishMaxSpawns;
                break;
            case "Bigsy":
                spawnRate = bigsySpawnRate;
                maxSpawns = bigsyMaxSpawns;
                break;
        }


    }
}
