﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour {

    public float lifetime; // How long attack will last
    public string myEnemy; // What character should attack damage
    public float myDamage; // How much damage to deal
    public float myStun; // How long time the attack stuns

    private Collider2D parentCollider;
    //private Transform target;

	// Use this for initialization
	void Start () {

        //transform.position = Vector2.MoveTowards(transform.position, target.position, 4 * Time.deltaTime);

        parentCollider = gameObject.GetComponentInParent<Collider2D>();

	}
	
	// Update is called once per frame
	void Update () {
       
	}

    private void FixedUpdate()
    {
        lifetime -= 1 * Time.deltaTime;
        //lifetime--;

        if (lifetime < 0)
        {
            Destroy(gameObject);
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.GetComponent<Collider2D>() != parentCollider)
        {

            switch (other.gameObject.tag)
            {

                case "Player":
                    other.gameObject.GetComponent<PlayerController>().damageTaken += myDamage;
                    break;

                case "Minion":
                    other.gameObject.GetComponent<EnemyController>().damageTaken += myDamage;
                    other.gameObject.GetComponent<EnemyController>().stun += myStun;
                    break;

                case "Container":
                    other.gameObject.GetComponent<ContainerController>().health -= myDamage;
                    break;

                case "PowerUp":

                    break;

            }

        }
                   
    }

}
