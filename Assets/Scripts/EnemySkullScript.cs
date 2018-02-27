using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkullScript : MonoBehaviour {

	public float speed;
	Transform player;
	Transform target;

	// Use this for initialization
	void Start () {

		// Find player location
		player = GameObject.FindGameObjectWithTag("Player").transform;
		target = player;

	}
	
	// Update is called once per frame
	void Update () {

		// Move towards player (old) location
		float step = speed * Time.deltaTime;
		transform.position = Vector3.MoveTowards(transform.position, target.position, step);
		// Rotation
		transform.Rotate (0, 0, -2);

	}

	// Touch player to kill
	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			Destroy (other.gameObject);
			SoundManagerScript.PlaySound ("GoblinDeath");
		}
	}
}
