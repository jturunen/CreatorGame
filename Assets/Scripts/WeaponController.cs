using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public List<GameObject> allWeapons { get; set; }

    void Start()
    {
        allWeapons = new List<GameObject>();

        Object[] subListObjects = Resources.LoadAll("Weapons", typeof(GameObject));

        foreach (GameObject subListObject in subListObjects)
        {
            GameObject lo = (GameObject)subListObject;
            Debug.Log("Found " + lo);
            allWeapons.Add(lo);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
