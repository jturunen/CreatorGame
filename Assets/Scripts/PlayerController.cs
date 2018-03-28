using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    Animator anim;
    PlayerWeapon playerWeapon;

    public float moveSpeed = 0.0f;
    public float hitPoints = 0.0f;
    public float damageDealt = 0.0f;
    public float victoryPoints = 0;

    public float attackSpeed = 0.2F;
    private float nextFire = 0.0F;

    float moveHorizontal;
    float moveVertical;
    bool facingRight = true;

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        playerWeapon = GetComponentInChildren(typeof(PlayerWeapon)) as PlayerWeapon;
        attackSpeed = playerWeapon.attackSpeed;
    }

    // Update is called once per frame
    void Update () {
        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");

        Move();
        Attack();
        Defensive();
        SpecialAttack();
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
        if (Input.GetButton("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + attackSpeed;
            if (moveHorizontal < 0 && moveVertical > 0)
            {
                anim.SetTrigger("AttackUpSide");
                //Debug.Log("Player Attacks Up Side left");
            }
            if (moveHorizontal == -1 || moveHorizontal == 1 && moveVertical == 0)
            {
                anim.SetTrigger("Attack");
                //Debug.Log("Player Attacks left");
            }
            if (moveHorizontal < 0 && moveVertical < 0)
            {
                anim.SetTrigger("AttackDownSide");
                //Debug.Log("Player Down Side left");
            }
            if (moveHorizontal == 0 && moveVertical == -1)
            {
                anim.SetTrigger("AttackDown");
                //Debug.Log("Player Attacks Down");
            }
            if (moveHorizontal > 0 && moveVertical < 0)
            {
                anim.SetTrigger("AttackDownSide");
                //Debug.Log("Player Down Side right");
            }
            /*if (moveHorizontal == 1 && moveVertical == 0)
            {
                anim.SetTrigger("Attack");
                //Debug.Log("Player Attacks right");
            }*/
            if (moveHorizontal > 0 && moveVertical > 0)
            {
                anim.SetTrigger("AttackUpSide");
                //Debug.Log("Player Attacks upside right");
            }
            if (moveHorizontal == 0 && moveVertical == 1)
            {
                anim.SetTrigger("AttackUp");
                //Debug.Log("Player Attacks up");

            } else
            {
                anim.SetTrigger("Attack");
            }
        }

        /*
                    float attackX = gameObject.transform.position.x + moveHorizontal;
            float attackY = gameObject.transform.position.y + moveVertical;

             nextFire = Time.time + attackSpeed;
             if (moveHorizontal < 0 && moveVertical > 0)
             {
                //Debug.Log("Player Attacks Up Side left");
                spawnHitBox(attackX, attackY);
             }
             if (moveHorizontal == -1 || moveHorizontal == 1 && moveVertical == 0)
             {
                //Debug.Log("Player Attacks left");
                spawnHitBox(attackX, attackY);
            }
            if (moveHorizontal < 0 && moveVertical < 0)
             {
                //Debug.Log("Player Down Side left");
                spawnHitBox(attackX, attackY);
            }
            if (moveHorizontal == 0 && moveVertical == -1)
             {
                //Debug.Log("Player Attacks Down");
                spawnHitBox(attackX, attackY);
            }
            if (moveHorizontal > 0 && moveVertical < 0)
             {
                //Debug.Log("Player Down Side right");
                spawnHitBox(attackX, attackY);
            }
            if (moveHorizontal == 0 && moveVertical == 1)
            {
                //Debug.Log("Player Attacks up");
                spawnHitBox(attackX, attackY);
            }
            if (moveHorizontal > 0 && moveVertical > 0)
            {
                //Debug.Log("Player Attacks upside right");
                spawnHitBox(attackX, attackY);
            }
            /*if (moveHorizontal == 1 && moveVertical == 0)
            {
                anim.SetTrigger("Attack");
                //Debug.Log("Player Attacks right");
            }
            if (moveHorizontal > 0 && moveVertical > 0)
            {
                anim.SetTrigger("AttackUpSide");
                //Debug.Log("Player Attacks upside right");
            }
             else
            {
                anim.SetTrigger("Attack");
            }*/

        /*// Find X position
        float xx = gameObject.transform.position.x;
        if (moveHorizontal < transform.position.x - 1f)
        {
            xx -= 1f;
        }
        else if (moveHorizontal > transform.position.x + 1f)
        {
            xx += 1f;
        }

        // Find Y position
        float yy = gameObject.transform.position.y;
        if (moveVertical < transform.position.y - 1f)
        {
            yy -= 1f;
        }
        else if (moveVertical > transform.position.y + 1f)
        {
            yy += 1f;
        }*/
        

    }

    private void Defensive()
    {
        if (Input.GetButton("Fire2"))
        {
            Debug.Log("Player Defence");
        }
    }
    private void SpecialAttack()
    {
        if (Input.GetButton("Fire3"))
        {
            Debug.Log("Player Special attack");
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
