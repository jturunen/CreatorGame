using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterList : MonoBehaviour {

    public List<GameObject> allMonsters { get; set; }

    void Start()
    {
        allMonsters = new List<GameObject>();

        Object[] subListObjects = Resources.LoadAll("Minions", typeof(GameObject));

        foreach (GameObject subListObject in subListObjects)
        {
            GameObject listObject = subListObject;
            Debug.Log("Created " + listObject);
            allMonsters.Add(listObject);
        }

    }

}
