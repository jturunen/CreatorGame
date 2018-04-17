using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerController : MonoBehaviour {

    public float health; // Health until die
    public float lootChance; // Chance to drop loot

    public GameObject loot; // What to drop
    public GameObject deathPrefab; // Death animation

    private SpriteRenderer mySprite; // My sprite

	// Use this for initialization
	void Start () {
        mySprite = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {

        // Drawing order
        int i = (Mathf.RoundToInt(transform.position.y));
        mySprite.sortingOrder = -i;

        // Health and die
        if (health <= 0)
        {
            
            // Drop loot randomly
            if (Random.value * 100 <= lootChance)
            {
                Instantiate(loot, transform.position, transform.rotation);   
            }

            // Die
            Destroy(gameObject);
            Instantiate(deathPrefab, transform.position, transform.rotation);

        }

	}

}
