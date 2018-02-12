using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    Animator anim;
    public float speed;
    bool facingRight = true;
    bool playerIsAttacking = false;

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        if (Input.GetButton("Fire1") && !playerIsAttacking)
        {
            playerIsAttacking = true;
            Vector3 movement = new Vector3(0, 0, 0.0f);
            GetComponent<Rigidbody2D>().velocity = movement;
            if (moveVertical > 0)
            {
                anim.SetTrigger("AttackUpAnimation");
            }
            else if (moveVertical < 0)
            {
                anim.SetTrigger("AttackDownAnimation");
            }
            else
            {
                anim.SetTrigger("AttackAnimation");
            }
        }
    }

    void FixedUpdate()
    {

        if (!playerIsAttacking)
        {

            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");
            Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0.0f);
            GetComponent<Rigidbody2D>().velocity = movement * speed;

            if (moveHorizontal > 0 && !facingRight)
            {
                Flip();
            }
            else if (moveHorizontal < 0 && facingRight)
            {
                Flip();
            }
        } 

    }
    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    void AlertAnimationEnd(string message)
    {
        if (message.Equals("AttackAnimationEnd"))
        {
            playerIsAttacking = false;
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        }
    }

}
