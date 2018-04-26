using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpController : MonoBehaviour {


    public float powerupSpeedLength;
    public float powerupSpeedEffect;

    public float powerupHealthEffect;

    public float powerupDamageLength;
    public float powerupDamageEffect;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // Someone picks up this power up
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        switch (collision.gameObject.tag)
        {

            // Give player speed boost
            case "Player":
                collision.GetComponent<PlayerController>().powerup.speedDuration = powerupSpeedLength;
                collision.GetComponent<PlayerController>().powerup.speedEffect = powerupSpeedEffect;
                collision.GetComponent<PlayerController>().powerup.damageDuration = powerupDamageLength;
                collision.GetComponent<PlayerController>().powerup.damageEffect = powerupDamageEffect;
                collision.GetComponent<PlayerController>().hitPoints += powerupHealthEffect;
                // Destroy
                Destroy(gameObject);
                break;

        }

        

    }

}
