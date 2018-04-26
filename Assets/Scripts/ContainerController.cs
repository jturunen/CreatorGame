using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerController : MonoBehaviour {

    public float health; // Health until die
    public float lootChance; // Chance to drop loot

    public GameObject loot1; // What to drop
    public GameObject loot2; // What to drop
    public GameObject loot3; // What to drop
    public GameObject loot4; // What to drop

    public GameObject deathPrefab; // Death animation
    private SpriteRenderer mySprite; // My sprite

	// Use this for initialization
	void Start () {
        mySprite = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {

        // Drawing order
        int i = (Mathf.RoundToInt(transform.position.y*1000));
        mySprite.sortingOrder = -i;

        // Health and die
        if (health <= 0)
        {
            
            // Drop loot randomly
            if (Random.value * 100 <= lootChance)
            {
                GameObject[] lootlist = { loot1, loot2, loot3, loot4 };
                GameObject randomLoot = lootlist[Random.Range(0, lootlist.Length)];
                Instantiate(randomLoot, transform.position, transform.rotation);   
            }

            // Die
            Destroy(gameObject);
            Instantiate(deathPrefab, transform.position, transform.rotation);

        }

	}

}
