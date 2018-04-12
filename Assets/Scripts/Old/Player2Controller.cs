using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2Controller : MonoBehaviour
{

    Animator anim;
    

    public float moveSpeed = 0.0f;
    public float hitPoints = 0.0f;
    public float damageDealt = 0.0f;
    public float victoryPoints = 0;


    float xMax = 10, xMin = -10, yMax = 5, yMin = 2;

    public GameObject deathPrefab; // Death particle/effect



    public float attackSpeed = 0.2F;

    private float nextFire = 0.0F;

    float moveHorizontal;
    float moveVertical;
    bool facingRight = true;

    public GameObject attackPrefab;
    private GameObject myAttack;

    public float flashLength;
    private bool flashActive;
    private float flashCounter;
    private SpriteRenderer mySprite;
    public float damageTaken = 0f;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        moveHorizontal = Input.GetAxis("Horizontal_P2");
        moveVertical = Input.GetAxis("Vertical_P2");

        if (!myAttack)
        {
            Move();
            Attack();
            Defensive();
        }

        /*Vector2 position = new Vector2(Mathf.Clamp(transform.position.x, xMin, xMax),
            Mathf.Clamp(transform.position.y, yMin, yMax));
        transform.position = position;*/
        if (hitPoints <= 0) Dead();

        if (damageTaken > 0)
        {
            hitPoints -= damageTaken;
            damageTaken = 0;
            StartCoroutine("HurtColor");
            Instantiate(deathPrefab, transform.position, transform.rotation);
            SoundManagerController.PlaySound("Hit");
        }



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

    private void Attack()
    {

        // My attack
        if (Input.GetButton("Fire1_P2") && !myAttack)
        {
            if (facingRight)
            {
                float x2 = transform.position.x + 0.5f;
                float y2 = transform.position.y;
                myAttack = Instantiate(attackPrefab, new Vector3(x2, y2), Quaternion.identity);
                myAttack.GetComponent<AttackController>().myEnemy = "Minion";
                SoundManagerController.PlaySound("Swish");
            }
            if (!facingRight)
            {
                float x2 = transform.position.x - 0.5f;
                float y2 = transform.position.y;
                myAttack = Instantiate(attackPrefab, new Vector3(x2, y2), Quaternion.identity);
                myAttack.GetComponent<AttackController>().myEnemy = "Minion";
                myAttack.GetComponent<SpriteRenderer>().flipX = true;
                SoundManagerController.PlaySound("Swish");
            }
        }

        #region Janne Attack

        /*
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
            
            if (moveHorizontal == 1 && moveVertical == 0)
            {
                anim.SetTrigger("Attack");
                //Debug.Log("Player Attacks right");
            }
            
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

        */

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
        }
        */

        #endregion Janne Attack

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

    private void Dead()
    {
        Instantiate(deathPrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    // Hit flash
    IEnumerator HurtColor()
    {
        for (int i = 0; i < 3; i++)
        {
            GetComponentInChildren<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.3f); //Red, Green, Blue, Alpha/Transparency
            yield return new WaitForSeconds(.1f);
            GetComponentInChildren<SpriteRenderer>().color = Color.white; //White is the default "color" for the sprite, if you're curious.
            yield return new WaitForSeconds(.1f);
        }
    }

}
