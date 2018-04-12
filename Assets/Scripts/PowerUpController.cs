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
                collision.GetComponent<PlayerController>().powerupSpeedLength = powerupSpeedLength;
                collision.GetComponent<PlayerController>().powerupSpeedEffect = powerupSpeedEffect;
                collision.GetComponent<PlayerController>().powerupDamageLength = powerupDamageLength;
                collision.GetComponent<PlayerController>().powerupDamageEffect = powerupDamageEffect;
                collision.GetComponent<PlayerController>().hitPoints += powerupHealthEffect;
                // Destroy
                Destroy(gameObject);
                break;

        }

        

    }

}
