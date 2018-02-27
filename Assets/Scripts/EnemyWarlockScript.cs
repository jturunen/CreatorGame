using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWarlockScript : MonoBehaviour {


	/*
	public Transform player;
	public float range = 50f;
	public float bulletImpulse = 20f;
	*/

	private float attackTimeMax = 180;
	private float attackTime = 0;

	public GameObject projectile;

	//private bool onRange = false;

	// Use this for initialization
	void Start () {
		//float rand = Random.Range (1f, 2f);
		//InvokeRepeating ("Shoot", 2, rand);
	}
	
	// Update is called once per frame
	void Update () {

		if (attackTime <= 0) {
			attackTime = attackTimeMax;
			Instantiate (projectile, transform.position, Quaternion.identity);
		} attackTime -= 60 * Time.deltaTime;

		//onRange = Vector3.Distance(transform.position, player.position)<range;

		//if (onRange)
		//transform.LookAt(player);		
	}

	void Shoot() {
		/*
		if (onRange) {
			Rigidbody2D bullet = (Rigidbody2D)Instantiate(projectile, transform.position + transform.forward, transform.rotation);
			bullet.AddForce(transform.forward*bulletImpulse, ForceMode.Impulse);

			Destroy (bullet.gameObject, 2);
		}
		*/
	}

	// Touch player to kill
	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			Destroy (other.gameObject);
			SoundManagerScript.PlaySound ("GoblinDeath");
		}
	}

}
