using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterList : MonoBehaviour {

    public List<GameObject> allMonsters { get; set; }

    // Use this for initialization
    void Start()
    {
        allMonsters = new List<GameObject>();

        Object[] subListObjects = Resources.LoadAll("Prefabs", typeof(GameObject));

        foreach (GameObject subListObject in subListObjects)
        {
            GameObject lo = (GameObject)subListObject;
            Debug.Log("Created " + lo);
            allMonsters.Add(lo);
        }
    }

}
