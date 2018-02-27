using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2Controller : MonoBehaviour {

	public float moveSpeed;
	public float speed;

	private bool facingRight = true;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
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

	// Flip player character
	void Flip() {
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

}
