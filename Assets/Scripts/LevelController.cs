using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour {

    public List<Vector2> spawnPoints { get; set; }
    public int maxUnits = 6;

    // Use this for initialization
    void Start () {

        spawnPoints = new List<Vector2>();

        for (int i = 0; i <= maxUnits; i++)
        {
            spawnPoints.Add(new Vector2(i, i));
            //Debug.Log(positionList[i]);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
