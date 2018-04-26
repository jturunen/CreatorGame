using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpController : MonoBehaviour {

    public float attackSpeedEffect;
    public float attackSpeedDuration;

    public float powerupSpeedLength;
    public float powerupSpeedEffect;

    public float powerupHealthEffect;

    public float powerupDamageLength;
    public float powerupDamageEffect;

    public int money;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // Someone picks up this power up
    private void OnTriggerEnter2D(Collider2D other)
    {
        
        switch (other.gameObject.tag)
        {

            // Give player speed boost
            case "Player":

                other.GetComponent<PlayerController>().powerup.attackSpeedDuration = attackSpeedDuration;
                other.GetComponent<PlayerController>().powerup.attackSpeedEffect = attackSpeedEffect;
                
                other.GetComponent<PlayerController>().powerup.speedDuration = powerupSpeedLength;
                other.GetComponent<PlayerController>().powerup.speedEffect = powerupSpeedEffect;
                
                other.GetComponent<PlayerController>().powerup.damageDuration = powerupDamageLength;
                other.GetComponent<PlayerController>().powerup.damageEffect = powerupDamageEffect;
                
                other.GetComponent<PlayerController>().hitPoints += powerupHealthEffect;
                
                other.GetComponent<PlayerController>().money += money;          

                // Destroy
                Destroy(gameObject);
                break;

        }

        

    }

}
