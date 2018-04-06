using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    #region Variables

    Animator anim;

    public int playerNumber; // What is the player number
    public float moveSpeed = 0.0f; // Movement speed
    public float hitPoints = 0.0f; // Health
    public float attackRange; // How far can character attack
    public float flashLength; // How long hitflash will last
    public float damageTaken = 0f; // Amount of incoming damage
    public float dodgeDistance = 0.0f; // Distance to dodge roll   
    public float dodgeSpeed; // Speed of how fast to dodge;
    public bool dodgeSafety; // Can character take damage while while dodging   
    public GameObject attackPrefab; // The attack prefab this character uses
    public GameObject deathPrefab; // Death particle/effect

    private float moveHorizontal; // Horizontal movement, keyboard
    private float moveVertical; // Vertical movement, keyboard
    private float moveHorizontal_P1; // Horizontal movement, gamepad
    private float moveVertical_P1; // Vertical movement, gamepad
    private float flashCounter; // Duration of hitflash
    private float dodgeDistanceNow; // Distance dodge rolled so far
    private bool facingRight = true; // Is the character facing right?
    private bool flashActive;  // Is the hitflash active?   
    private bool attackDone; // Did character already do attack in attack state?
    private bool dodgeStarted; // Dodge roll started yet?
    private bool buttonAttack; // Attack button being pressed?
    private enum States { idle, move, attack, flee, dead, dodge, win }; // Enum for state machine
    private States state = States.idle; // State
    private GameObject myAttack; // Characters current attack, default should be null
    private SpriteRenderer mySprite;

    #endregion Variables

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update () {

        //this.transform.Translate(0, 1 * Time.deltaTime, 0, Space.World);
        //transform.Rotate(0, 0, Time.deltaTime * -10000);

        // Read input from the player
        readInput();

        // States
        switch (state)
        {
            case States.idle:
                Idle();
                break;
            case States.move:
                Move();
                break;
            case States.attack:
                Attack();
                break;
            case States.dodge:
                Dodge();
                break;
            case States.win:
                Win();
                break;
            /*
            case States.flee:
                Flee();
                break;
            case States.dead:
                Dead();
                break;
            */
        }       
        
        // Take damage
        if (damageTaken > 0) handleDamage();
        
        // Die
        if (hitPoints <= 0) Dead();

        /*
        // Game won?
        GameObject testi = GameObject.FindWithTag("GameController");
        if (testi.GetComponent<GameController>().winOtters)
        {
            state = States.win;
        }
        */

        
        

    }

    // Read input from the player
    private void readInput()
    {
        switch (playerNumber)
        {

            case 1:
                moveHorizontal = Input.GetAxis("Horizontal");
                moveVertical = Input.GetAxis("Vertical");
                //buttonAttack = Input.GetKey(KeyCode.F);
                buttonAttack = Input.GetButtonDown("Fire1_P1");
                break;

            case 2:
                moveHorizontal = Input.GetAxis("Horizontal_P2");
                moveVertical = Input.GetAxis("Vertical_P2");
                break;

            case 3:
                moveHorizontal = Input.GetAxis("Horizontal_P3");
                moveVertical = Input.GetAxis("Vertical_P3");
                break;

            case 4:
                moveHorizontal = Input.GetAxis("Horizontal_P4");
                moveVertical = Input.GetAxis("Vertical_P4");
                break;


        }

            

    }

    // Idle state
    private void Idle()
    {

        // Log
        Debug.Log("My state is now Idle");

        // Triggers
        #region Trigger

        // User inputs to move
        if (moveHorizontal != 0f || moveVertical != 0f)
        {
            state = States.move;
            anim.SetBool("Idle", false);
            anim.SetBool("Walk", true);
        }

        // User input to attack
        if (Input.GetKeyDown(KeyCode.Space))
        {
            state = States.attack;
            anim.SetBool("Idle", false);
            anim.SetBool("Attack", true);
        }

        #endregion Trigger

        #region Behaviour

        transform.localEulerAngles = new Vector3(0, 0, 0);

        #endregion Behaviour

    }

    // Movement state
    private void Move()
    {
        
        Debug.Log("My state is now move");

        #region Trigger

        // No user input
        if (moveHorizontal == 0.0f && moveVertical == 0.0f)
        {
            state = States.idle;
            anim.SetBool("Idle", true);
            anim.SetBool("Walk", false);
        }

        // User inputs attack
        if (Input.GetKeyDown(KeyCode.Space) || buttonAttack)
        {
            state = States.attack;
            anim.SetBool("Walk", false);
            anim.SetBool("Attack", true);          
        }

        // Dodge
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            state = States.dodge;
            anim.SetBool("Walk", false);
            anim.SetBool("Dodge", true);
        }

        #endregion Trigger

        #region Behaviour

        // Flip character
        Flip();

        // Keyboard movement
        #region Keyboard

        // Handle input, move character
        //Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0.0f);
        //GetComponent<Rigidbody2D>().velocity = movement * moveSpeed;

        //float moveHorizontal = Input.GetAxisRaw("Horizontal");
        //float moveVertical = Input.GetAxisRaw("Vertical");
        transform.Translate(new Vector3(moveHorizontal, moveVertical) * moveSpeed * Time.deltaTime, Space.World);

        #endregion Keyboard

        // Gamepad movement
        #region Gamepad
        /*
        // Get input
        moveHorizontal_P1 = Input.GetAxis("Horizontal_P1");
        moveVertical_P1 = Input.GetAxis("Vertical_P1");

        // Move
        Vector3 movement_p1 = new Vector3(moveHorizontal_P1, moveVertical_P1, 0.0f);
        GetComponent<Rigidbody2D>().velocity = movement_p1 * moveSpeed;
        */
        #endregion Gamepad    

        #endregion Behaviour

    }

    // Attack 
    private void Attack ()
    {
        
        // Log
        Debug.Log("My state is now attack");

        // Triggers
        #region Triggers
        
        // Go idle when attack is done
        if (!myAttack && attackDone)
        {
            attackDone = false;
            state = States.idle;
            anim.SetBool("Attack", false);
            anim.SetBool("Idle", true);
        }
        
        #endregion Triggers

        #region Behaviour

        // Create attack prefab left or right
        if (!myAttack && !attackDone && (Input.GetButton("Fire1") || Input.GetButton("Fire1_P1")))
        //if (!attackDone && !myAttack)
        {
            if (facingRight)
            {
                attackDone = true;
                float x2 = transform.position.x + attackRange;
                float y2 = transform.position.y;
                myAttack = Instantiate(attackPrefab, new Vector3(x2, y2), Quaternion.identity);
                myAttack.GetComponent<AttackController>().myEnemy = "Minion";
                SoundManagerController.PlaySound("Swish");
            }
            if (!facingRight)
            {
                attackDone = true;
                float x2 = transform.position.x - attackRange;
                float y2 = transform.position.y;
                myAttack = Instantiate(attackPrefab, new Vector3(x2, y2), Quaternion.identity);
                myAttack.GetComponent<AttackController>().myEnemy = "Minion";
                myAttack.GetComponent<SpriteRenderer>().flipX = true;
                SoundManagerController.PlaySound("Swish");
            }
        }

        #endregion Behaviour

    }

    // Flip character
    private void Flip()
    {

        // Flip character horizontally
        if (moveHorizontal > 0 && !facingRight)
        {
            facingRight = !facingRight;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
        else if (moveHorizontal < 0 && facingRight)
        {
            facingRight = !facingRight;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
     
    }

    // Die
    private void Dead()
    {
        Instantiate(deathPrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    // Win
    private void Win()
    {
        Debug.Log("My state is now win");
        transform.Rotate(new Vector3(0, 0, -1), Time.deltaTime * 1800, Space.Self);
    }

    // Dodge roll
    private void Dodge()
    {

        // Log
        Debug.Log("My state is now Dodge");

        // Trigger
        #region Trigger

        // Go idle if roll is done
        if (dodgeDistanceNow >= dodgeDistance)
        {
            state = States.idle;
            dodgeStarted = false;
            anim.SetBool("Dodge", false);
            anim.SetBool("Idle", true);
            //transform.localEulerAngles = new Vector3(0, 0, 0);
        }

        // User inputs attack
        if (Input.GetKeyDown(KeyCode.Space))
        {
            state = States.attack;
            anim.SetBool("Dodge", false);
            anim.SetBool("Attack", true);
            dodgeStarted = false;
        }

        #endregion Trigger

        // Behaviour
        #region Behaviour

        // Initialize dodge
        if (!dodgeStarted)
        {
            dodgeStarted = true;
            dodgeDistanceNow = 0;
        }

        // Dodge 
        if (facingRight)
        {          
            float x = moveHorizontal * dodgeSpeed * Time.deltaTime;
            float y = moveVertical * dodgeSpeed * Time.deltaTime;
            transform.Translate(new Vector3(x, y), Space.World);
            dodgeDistanceNow += (1 * dodgeSpeed * Time.deltaTime) + (1 * dodgeSpeed * Time.deltaTime);
            //transform.Rotate(0, 0, Time.deltaTime * -10000);
            transform.Rotate(new Vector3(0,0,-1), Time.deltaTime * 1800, Space.Self);
        }           
        else
        {
            float x = moveHorizontal * dodgeSpeed * Time.deltaTime;
            float y = moveVertical * dodgeSpeed * Time.deltaTime;
            transform.Translate(new Vector3(x, y), Space.World);
            dodgeDistanceNow += (1 * dodgeSpeed * Time.deltaTime) + (1 * dodgeSpeed * Time.deltaTime);
            transform.Rotate(new Vector3(0,0,1), Time.deltaTime * 1800, Space.Self);
            
        }

        #endregion Behaviour

    }

    // Handle incoming damage
    private void handleDamage()
    {
        
        hitPoints -= damageTaken;
        damageTaken = 0;
        StartCoroutine("HurtColor");
        Instantiate(deathPrefab, transform.position, transform.rotation);
        SoundManagerController.PlaySound("Hit");
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
