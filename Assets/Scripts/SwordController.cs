using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour {

	public GameObject blood;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}


	// Touch enemy to kill it
	void OnTriggerEnter2D(Collider2D other) {
		
		if (other.gameObject.tag == "Enemy") {
			//SoundManagerScript.PlaySound ("GoblinDeath");
			//Instantiate (blood, transform.position, Quaternion.identity);
			//Destroy (other.gameObject);

			EnemyController c = other.gameObject.GetComponent<EnemyController> ();
			c.damage++;

		}

		if (other.gameObject.tag == "Destructible") {
			//other.health--;
			//other.gameObject.GetComponent("Health") --;
			//other.gameObject.Health -= 1;
			ChestController c = other.gameObject.GetComponent<ChestController>();
			c.health -= 1;
			//player.NoAttack (); 
			//Collider2D.enabled = false;

			//print(other.gameObject is );

		}
	
	}

}
