using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestController : MonoBehaviour {

	public GameObject loot;
	public float health = 1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (health <= 0) {
			Instantiate (loot, transform.position, Quaternion.identity);
			Destroy (gameObject);
		}
	}

}
