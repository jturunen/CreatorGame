using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour {

    public List<Vector2> spawnPoints { get; set; }
    public GameObject spawnPointPrefab;
    GameObject spawnPointObject;
    public int maxUnits = 6;

    // Use this for initialization
    void Start () {

        spawnPoints = new List<Vector2>();

        spawnPoints.Add(new Vector2(0, -4));
        spawnPoints.Add(new Vector2(3, -4));
        spawnPoints.Add(new Vector2(6, -4));
        spawnPoints.Add(new Vector2(0, 4));
        spawnPoints.Add(new Vector2(3, 4));
        spawnPoints.Add(new Vector2(6, 4));

        for (int i = 0; i < maxUnits; i++)
        {
            //spawnPoints.Add(new Vector2(i, i));
            spawnPointObject = Instantiate(spawnPointPrefab, spawnPoints[i], Quaternion.identity);
            spawnPointObject.name = "SpawnPoint" + i;
            //Debug.Log(positionList[i]);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
