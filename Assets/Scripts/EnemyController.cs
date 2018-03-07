using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    public float moveSpeed = 0.0f;
    public float hitPoints = 0.0f;
    public float attackSpeed = 0.0f;

    bool facingRight = true;
    public bool isControlled = false;

    Transform player;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        //TODO: get real inputs
        /*if (Input.GetButton("Fire2"))
        {
            Attack();
        }*/

        //TODO: Get real inputs
        /* if (Input.GetButton("Fire2"))
         {
            Defensive();
         }*/

        //TODO: Get real inputs
        /*if (Input.GetButton("Fire3"))
        {
           SpecialAttack();
        }*/

    }

    private void Move()
    {
        if (isControlled)
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");
            Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0.0f);
            GetComponent<Rigidbody2D>().velocity = movement * moveSpeed;

            if (moveHorizontal > 0 && !facingRight)
            {
                Flip();
            }
            else if (moveHorizontal < 0 && facingRight)
            {
                Flip();
            }
        } else
        {
            float step = moveSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, player.position, step);
        }
    }

    private void Attack()
    {
        if (isControlled)
        {
            //TODO: Get real inputs
            if (Input.GetButton("Fire1"))
            {
                Debug.Log("Enemy Attacks");
            }
        }
    }

    private void Defensive()
    {
        Debug.Log("Enemy Defence");
    }

    private void SpecialAttack()
    {
        Debug.Log("Enemy Special Attacks");
    }

    private void Flip()
    {
        Debug.Log("Flip");
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
