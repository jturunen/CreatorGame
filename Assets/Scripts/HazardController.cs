using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardController : MonoBehaviour {

	public float damage; // How much damage this hazard deals
    public float damageCooldown; // How long wait between attacks
    public float spriteAttackDuration; // Duration of attack sprite
    public Sprite spriteIdle; // What sprite should be used
    public Sprite spriteAttack; // What sprite should be used

    private float timeSinceLastAttack; // Time when was last attack
    private float spriteAttackDurationNow; // Duration of attack sprite has been on
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

        // Change sprite back to idle
        if (spriteAttack && mySpriteRenderer.sprite == spriteAttack && spriteAttackDurationNow >= spriteAttackDuration)
        {
            mySpriteRenderer.sprite = spriteIdle;
            spriteAttackDurationNow = 0;
        }
        else if (spriteAttack && mySpriteRenderer.sprite == spriteAttack)
        {
            spriteAttackDurationNow += Time.deltaTime;
        }

    }

    // Detect collision with Otters and deal damage to them
    private void OnTriggerStay2D(Collider2D other)
    {
        
        if (other.gameObject.tag == "Player" && timeSinceLastAttack >= damageCooldown)
        {

            if (other.gameObject.transform.position.y - transform.position.y > -0.1)
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

        if (other.gameObject.tag == "Container")
        {

            if (other.gameObject.transform.position.y - transform.position.y > -0.1)
            {
                timeSinceLastAttack = 0;
                other.gameObject.GetComponent<ContainerController>().health -= damage;
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

}
