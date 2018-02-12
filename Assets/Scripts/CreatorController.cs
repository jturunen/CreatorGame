using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatorController : MonoBehaviour {

    public GameObject enemyObject;
    public PlayerMovement pMovement;
    public EnemyMovementBot eMovement;
    public int maxUnits;
    private int unitCounter = 0;
    public float spawnRate = 1;
    private float nextSpawn = 1;
    float MinX = -5;
    float MaxX = 5;
    float MinY = -5;
    float MaxY = 5;
    public Canvas spawnUi;
    // Use this for initialization
    void Start () {
        pMovement.enabled = false;
        eMovement.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButton("Jump"))
        {
            if (unitCounter < maxUnits && Time.time > nextSpawn)
            {
                nextSpawn = Time.time + spawnRate;
                unitCounter++;
                SpawnObject();
            } else if (unitCounter == maxUnits)
            {
                spawnUi.enabled = false;
                pMovement.enabled = true;
                eMovement.enabled = true;
            }
        }
    }

    void SpawnObject()
    {
        float x = Random.Range(MinX, MaxX);
        float y = Random.Range(MinY, MaxY);
        Instantiate(enemyObject, new Vector3(x, y, 0), Quaternion.identity);
    }
}
