using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageScript : MonoBehaviour {
    public PlayerWeapon weapon;
    // Use this for initialization
    public float lifetime = 30.0f;
    //private Transform target;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
      /*  lifetime--;

        if (lifetime < 0)
        {
            Destroy(gameObject);
        }*/

    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Respawn")
        {
            EnemyController enemy = other.GetComponent<EnemyController>();
            enemy.hitPoints -= weapon.damage;
        }
    }
}
