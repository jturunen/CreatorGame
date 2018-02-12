using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementBot : MonoBehaviour {

    Animator anim;
    Transform player;
    public float speed;
    public float fireRate = 1F;
    private float nextFire = 0.0F;

    // Use this for initialization
    void Start () {
		player = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponent<Animator>();
    }

    void Update () {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, player.position, step);
        if (Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            anim.SetTrigger("Attack");
        }
    }
}
