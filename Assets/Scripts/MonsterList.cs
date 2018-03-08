using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterList {

    public List<GameObject> allMonsters { get; set; }
                                       
    // Use this for initialization
    public MonsterList() {
        allMonsters = new List<GameObject>();
        for (int i = 0; i <= 8; i++)
        {
            allMonsters.Add(new GameObject());
        }
    }
	

}
