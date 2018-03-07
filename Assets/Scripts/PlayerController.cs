using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    Animator anim;

    public float moveSpeed = 0.0f;
    public float hitPoints = 0.0f;
    public float damageDealt = 0.0f;
    public float victoryPoints = 0;
    //public Weapon usedWeapon = null; weapon that is used by the player
    float moveHorizontal;
    float moveVertical;
    bool facingRight = true;

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update () {
        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");

        Move();
        Attack();
        Defensive();
    }

    private void Move()
    {

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
    }

    private void Attack ()
    {
        if (Input.GetButton("Fire1"))
        {
            if (moveVertical > 0)
            {
                Debug.Log("Player Attacks Up");
                anim.SetTrigger("Attack");
            } else
            {
                Debug.Log("Player Attacks");
                anim.SetTrigger("Attack");
            }

        }
    }

    private void Defensive()
    {
        if (Input.GetButton("Fire2"))
        {
            Debug.Log("Player Defence");
        }
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
