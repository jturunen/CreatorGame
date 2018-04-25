using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardController : MonoBehaviour {

	public float damage; // How much damage this hazard deals
    public float damageCooldown; // How long wait between attacks
    public Sprite spriteIdle; // What sprite should be used
    public Sprite spriteAttack; // What sprite should be used

    private float timeSinceLastAttack; // Time when was last attack
    private SpriteRenderer mySpriteRenderer;

	// Use this for initialization
	void Start () {
		mySpriteRenderer = GetComponentInParent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FixedUpdate()
    {
        if (timeSinceLastAttack < damageCooldown)
        {
            timeSinceLastAttack += Time.deltaTime;
        }
    }

    // Detect collision with Otters and deal damage to them
    private void OnTriggerStay2D(Collider2D other)
    {
        
        if (other.gameObject.tag == "Player" && timeSinceLastAttack >= damageCooldown)
        {
            timeSinceLastAttack = 0;
            other.gameObject.GetComponent<PlayerController>().damageTaken += damage;
            //other.attachedRigidbody.AddForce(Vector3.up * 1);
            //other.gameObject.transform.Translate(new Vector3(0.1f, 0f, 0f));

            // Change sprite
            if (spriteAttack && mySpriteRenderer.sprite != spriteAttack)
            {
                mySpriteRenderer.sprite = spriteAttack;
            }

        }   
     
    }

}
