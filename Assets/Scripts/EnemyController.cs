using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

	/*
	public float moveSpeed;
	public float timeBetweenMove;
	public float timeToMove;

	private float timeToMoveCounter;
	private float timeBetweenMoveCounter;
	private Rigidbody2D myRigidbody;
	private bool moving;
	private Vector3 moveDirection;

	Animator anim;
	Transform player;
	public float speed;
	public float fireRate = 1F;
	private float nextFire = 0.0F;
	*/

	public float health = 3;
	public float damage = 0;
	public float speed;
	public string name;
	public bool creator = false;
	public GameObject blood;

	private bool facingRight = false;

	Transform target; // For finding old player location for skull
	Transform player;

	// Use this for initialization
	void Start () {

		// Find player location
		player = GameObject.FindGameObjectWithTag("Player").transform;
		target = player;

		//blood = GameObject.Find ("BloodSplash");

		/*
		myRigidbody = GetComponent<Rigidbody2D> ();

		//timeBetweenMoveCounter = timeBetweenMove;
		//timeToMoveCounter = timeToMove;
		
		timeBetweenMoveCounter = Random.Range (timeBetweenMove * 0.75f, timeBetweenMove * 1.25f);
		timeToMoveCounter = Random.Range (timeToMove * 0.75f, timeBetweenMove * 1.25f);
		*/

		//player = GameObject.FindGameObjectWithTag("Player").transform;
		//anim = GetComponent<Animator>();

	}
	
	// Update is called once per frame
	void Update () {

		// Creator monster movement
		if (creator) {
			// Movement

			float moveHorizontal = Input.GetAxisRaw ("Horizontal2");
			float moveVertical = Input.GetAxisRaw ("Vertical2");
			transform.Translate (new Vector3 (moveHorizontal, moveVertical) * speed * Time.deltaTime * 3);
		}

		// Monster AI movement
		if (!creator) {

			switch (name) {
				
			case "Warlock":
				break;

			case "Skull":
				// Move towards player (old) location
				float step = speed * Time.deltaTime;
				transform.position = Vector3.MoveTowards(transform.position, target.position, step);
				// Rotation
				transform.Rotate (0, 0, -2);

				break;

			default:
				player = GameObject.FindGameObjectWithTag ("Player").transform;
				float step2 = speed * Time.deltaTime;
				transform.position = Vector3.MoveTowards (transform.position, player.position, step2);
				break;

			}

		}

		// Take damage
		if (damage > 0) {
			health -= damage;
			damage = 0;
			if (blood != null)
				Instantiate (blood, transform.position, Quaternion.identity);
		}

		// Die
		if (health <= 0) {

			if (name != "Skull")
				SoundManagerScript.PlaySound ("GoblinDeath");
			if (blood != null)
				Instantiate (blood, transform.position, Quaternion.identity);
			Destroy (gameObject);
		}	
	
		/*
		float step = speed * Time.deltaTime;
		transform.position = Vector3.MoveTowards(transform.position, player.position, step);
		if (Time.time > nextFire) {
			nextFire = Time.time + fireRate;
			//anim.SetTrigger("Attack");
		}
		*/

		/*
		// Movement
		float moveHorizontal = Random.Range(-1f, 1f);
		float moveVertical = Input.GetAxisRaw ("Vertical");
		transform.Translate(new Vector3(moveHorizontal, moveVertical) * moveSpeed * Time.deltaTime);
		*/

		/*
		// Movement
		if (moving) {
			timeToMoveCounter -= Time.deltaTime;
			myRigidbody.velocity = moveDirection;

			if (timeToMoveCounter < 0f) {
				moving = false;
				//timeBetweenMoveCounter = timeBetweenMove;
				timeBetweenMoveCounter = Random.Range (timeBetweenMove * 0.75f, timeBetweenMove * 1.25f);
			}

		} else {
			timeBetweenMoveCounter -= Time.deltaTime;
			myRigidbody.velocity = Vector2.zero;

			if (timeBetweenMoveCounter < 0f) {
				moving = true;
				//timeToMoveCounter = timeToMove;
				timeToMoveCounter = Random.Range (timeToMove * 0.75f, timeBetweenMove * 1.25f);
				moveDirection = new Vector3 (Random.Range (-1f, 1f) * moveSpeed, Random.Range (-1f, 1f) * moveSpeed, 0f);
			}
		
		}
		*/

	}

	// Touch player to kill
	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			//Destroy (other.gameObject);
			//SoundManagerScript.PlaySound ("GoblinDeath");

			PlayerController c = other.gameObject.GetComponent<PlayerController> ();
			c.damage++;

		}
	}

}
