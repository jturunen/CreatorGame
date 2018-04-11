using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour {

    #region Variables

    public GameObject chosenMinion;

    public float trashySpawnRate = 3;
    public float goldfishSpawnRate = 7;
    public float bigsySpawnRate = 12;

    public int trashyMaxSpawns = 4;
    public int goldfishMaxSpawns = 3;
    public int bigsyMaxSpawns = 2;

    public int maxSpawns = 5;

    public float spawnRate = 0.1F; // How fast minions spawn
    private float nextSpawn = 0.0F;

    #endregion
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.time > nextSpawn && maxSpawns > 0)
        {
            nextSpawn = Time.time + spawnRate;
            Instantiate(chosenMinion, transform.position, Quaternion.identity);
        }
    }

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
