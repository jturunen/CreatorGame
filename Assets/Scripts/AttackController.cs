using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour {

    public float lifetime = 0.5f;
    public string myEnemy;
    public float myDamage = 1.0f;
    //private Transform target;

	// Use this for initialization
	void Start () {
        //transform.position = Vector2.MoveTowards(transform.position, target.position, 4 * Time.deltaTime);
	}
	
	// Update is called once per frame
	void Update () {
       
	}

    private void FixedUpdate()
    {
        //lifetime -= Time.deltaTime;
        lifetime--;

        if (lifetime < 0)
        {
            Destroy(gameObject);
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (myEnemy == "Player" && other.gameObject.tag == "Player")
        {
            //Destroy(other.gameObject);
            //other.gameObject.GetComponent<PlayerController>().hitPoints -= myDamage;
            //other.gameObject.GetComponent<PlayerController>().damageTaken += myDamage;
            if (other.gameObject.GetComponent<PlayerController>())
            {
                other.gameObject.GetComponent<PlayerController>().damageTaken += myDamage;
            }
            else if (other.gameObject.GetComponent<Player2Controller>())
            {
                other.gameObject.GetComponent<Player2Controller>().damageTaken += myDamage;
            }
            else if (other.gameObject.GetComponent<Player3Controller>())
            {
                other.gameObject.GetComponent<Player3Controller>().damageTaken += myDamage;
            }
        }

        if (myEnemy == "Minion" && other.gameObject.tag == "Minion")
        {
            //Destroy(other.gameObject);
            other.gameObject.GetComponent<EnemyController>().damageTaken += myDamage;
        }

    }

}
