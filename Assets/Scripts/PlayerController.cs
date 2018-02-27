using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public string name;
	public float moveSpeed;
	public float speed;
	public float health = 3;
	public float damage = 0;
	public GameObject blood;

	private Rigidbody2D rb2d;
	private Animator anim;
	private bool facingRight = true;
	private bool attacking = false;

	Collider2D bladecol;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		bladecol = GameObject.Find ("Blade").GetComponent<Collider2D> ();
		rb2d = GetComponent<Rigidbody2D> ();

	}
	
	// Update is called once per frame
	void Update () {

		// Take damage
		if (damage > 0) {
			health -= damage;
			damage = 0;
			Instantiate (blood, transform.position, Quaternion.identity);
		}

		// Die
		if (health <= 0) {
			Instantiate (blood, transform.position, Quaternion.identity);
			Destroy (gameObject);
		}	

		if (name== "Player1") {
			// Movement
			float moveHorizontal = Input.GetAxisRaw ("Horizontal");
			float moveVertical = Input.GetAxisRaw ("Vertical");
			transform.Translate(new Vector3(moveHorizontal, moveVertical) * moveSpeed * Time.deltaTime);

			// Flip 
			if (moveHorizontal > 0 && !facingRight) {
				Flip ();
			} else if (moveHorizontal < 0 && facingRight) {
				Flip ();
			}

			// Attack
			if (Input.GetKeyDown(KeyCode.Space) && !attacking) {
				//anim.Play ("PlayerAttack");
				anim.SetTrigger("attack");
				//SoundManagerScript.PlaySound ("GoblinDeath");
				SoundManagerScript.PlaySound("Swish");
			}

			// Dodge
			if (Input.GetKeyDown(KeyCode.LeftShift)) {

				//rb2d.velocity += new Vector2 (100 * moveHorizontal, 100 * moveVertical);

				transform.Translate(new Vector3(moveHorizontal, moveVertical) * 50 * Time.deltaTime);
			}

		}

		if (name == "Player2") {
			
			// Movement
			float moveHorizontal = Input.GetAxisRaw ("Horizontal3");
			float moveVertical = Input.GetAxisRaw ("Vertical3");
			transform.Translate(new Vector3(moveHorizontal, moveVertical) * moveSpeed * Time.deltaTime);

			// Flip 
			if (moveHorizontal > 0 && !facingRight) {
				Flip ();
			} else if (moveHorizontal < 0 && facingRight) {
				Flip ();
			}

		}






		if (Input.GetKeyUp (KeyCode.LeftShift)) {
			//rb2d.velocity = new Vector2 (0, 0);
		}

		//transform.position = new Vector2 (Mathf.Clamp(transform.position.x,0,0), Mathf.Clamp(transform.position.y, 0, 0) );
	}

	void Attack() {
		bladecol.enabled = true;
		attacking = true;
	}

	void NoAttack() {
		bladecol.enabled = false;
		anim.ResetTrigger ("attack");
		attacking = false;
	}

	// Flip player character
	void Flip() {
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

}
