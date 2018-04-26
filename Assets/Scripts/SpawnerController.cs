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
    public int trashyMaxSpawnsRoundOne = 2;
    public int goldfishMaxSpawnsRoundOne = 2;
    public int bigsyMaxSpawnsRoundOne = 2;

    //Maximum spawns for minions
    public int trashyMaxSpawnsRoundTwo = 3;
    public int goldfishMaxSpawnsRoundTwo = 3;
    public int bigsyMaxSpawnsRoundTwo = 3;

    //Maximum spawns for minions
    public int trashyMaxSpawnsRoundThree = 4;
    public int goldfishMaxSpawnsRoundThree = 4;
    public int bigsyMaxSpawnsRoundThree = 4;

    private int maxSpawns = 2;

    private GameObject gameControllerObject; //Game controller for indexing
    private GameController gameController = null; //Script from gameController
    private int index = 0; //Index for spawned minion (not used currently)

    public float spawnRate = 0.1F; // How fast minions spawn
    private float nextSpawn = 0.0F;
    private bool noControl =true;
    private int round;
    #endregion
    // Use this for initialization
    void Start () {
        //Find the GameController object, for indexing the minions
        gameControllerObject = GameObject.Find("GameController");
        gameController = gameControllerObject.GetComponent<GameController>();

        round = RoundController.roundIndex;

    }
	
	// Update is called once per frame
	void Update () {
        setControl();
        if (Time.time > nextSpawn && maxSpawns > 0)
        {
            nextSpawn = Time.time + spawnRate;
            //Spawn the minion to the spawnpoint position, and name it with index number from the gameController
            (Instantiate(chosenMinion, transform.position, Quaternion.identity) as GameObject).name += gameController.minionIndex.ToString();
            //Add one to index
            gameController.minionIndex++;
            //Reduce one maxSpawns
            maxSpawns--;
            if(maxSpawns == 0)
            {
                gameController.allMinionsSpawned = true;
            }
        }
    }

    void setControl()
    {
        if (noControl)
        {

            //find first spawned minion
            GameObject minionToControl = GameObject.Find(chosenMinion.name +"(Clone)0");
            Debug.Log("minionToControl" + minionToControl);

            if (minionToControl != null)
            {
                minionToControl.GetComponent<EnemyController>().isControlled = true;
                minionToControl.transform.position = new Vector2(minionToControl.transform.position.x +1 , minionToControl.transform.position.y);
                //Toggle
                noControl = false;
            }

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
                if (round == 0)
                {
                    maxSpawns = trashyMaxSpawnsRoundOne;
                }
                if (round == 1)
                {
                    maxSpawns = trashyMaxSpawnsRoundTwo;
                }
                if (round == 2)
                {
                    maxSpawns = trashyMaxSpawnsRoundThree;
                }
                break;
            case "Goldy":
                spawnRate = goldfishSpawnRate;
                if (round == 0)
                {
                    maxSpawns = goldfishMaxSpawnsRoundOne;
                }
                if (round == 1)
                {
                    maxSpawns = goldfishMaxSpawnsRoundTwo;
                }
                if (round == 2)
                {
                    maxSpawns = goldfishMaxSpawnsRoundThree;
                }
                break;
            case "Bigsy":
                spawnRate = bigsySpawnRate;
                if (round == 0)
                {
                    maxSpawns = bigsyMaxSpawnsRoundOne;
                }
                if (round == 1)
                {
                    maxSpawns = bigsyMaxSpawnsRoundTwo;
                }
                if (round == 2)
                {
                    maxSpawns = bigsyMaxSpawnsRoundThree;
                }
                break;
        }


    }
}
