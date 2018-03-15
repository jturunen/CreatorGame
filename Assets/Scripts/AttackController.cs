using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour {

    public float lifetime = 60.0f;
    //private Transform target;

	// Use this for initialization
	void Start () {
        //transform.position = Vector2.MoveTowards(transform.position, target.position, 4 * Time.deltaTime);
	}
	
	// Update is called once per frame
	void Update () {

        lifetime--;

        if (lifetime<0)
        {
            Destroy(gameObject);
        } 

	}

    /*private void FixedUpdate()
    {
        
    }*/

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Destroy(other.gameObject);
        }

    }

}
