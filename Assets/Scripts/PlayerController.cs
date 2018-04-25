using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dodge {
    public float cooldown, distance, speed;
    public bool safety, sound;
    [System.NonSerialized]
    public float timeSinceLastDodge;
}

public class PlayerController : MonoBehaviour {

    #region Variables

    Animator anim;
    
    //[System.NonSerialized]
    public Dodge dodge; // Create variable that holds all dodge stats neatly
    public int playerNumber; // What is the player number
    public float moveSpeed = 0.0f; // Movement speed
    public float hitPoints = 0.0f; // Health
    public float attackRange; // How far can character attack
    public float flashLength; // How long hitflash will last
    public float damageTaken = 0f; // Amount of incoming damage
    public float dodgeDistance = 0.0f; // Distance to dodge roll   
    public float dodgeSpeed; // Speed of how fast to dodge;
    public float powerupSpeedLength; // How long the speed power up lasts
    public float powerupSpeedEffect; // How much speed the speed power up gives
    public float damage; // How much damage this character deals when attacking
    public float powerupDamageLength; // Power Up Damage Length
    public float powerupDamageEffect; // Power Up Damage Effect
    public bool dodgeSafety; // Can character take damage while while dodging
    public bool dodgeSound; // Sound effect for dodge?
    public bool gamepad; // Gamepad controlled instead of keyboard?
    public GameObject attackPrefab; // The attack prefab this character uses
    public GameObject deathPrefab; // Death particle/effect
    public GameObject bloodPrefab; // Blood particle/effect
    public GameObject fartPrefab; // Fart particle/effect

    private float moveHorizontal; // Horizontal movement, keyboard
    private float moveVertical; // Vertical movement, keyboard
    private float moveHorizontal_P1; // Horizontal movement, gamepad
    private float moveVertical_P1; // Vertical movement, gamepad
    private float flashCounter; // Duration of hitflash
    private float dodgeDistanceNow; // Distance dodge rolled so far
    private float moveSpeedBonus; // Bonus move speed from speed power up
    private float damageBonus; // Current damage bonus
    private bool facingRight = true; // Is the character facing right?
    private bool flashActive;  // Is the hitflash active?   
    private bool attackStarted = false; // Did character already start attack in attack state?
    private bool dodgeStarted; // Dodge roll started yet?
    private bool buttonAttack; // Attack button being pressed?
    private bool buttonDodge; // Dodge button pressed?
    private enum States { idle, move, attack, attackStart, flee, dead, dodgeStart, dodge, win }; // Enum for state machine
    private States state = States.idle; // State
    private GameObject myAttack; // Characters current attack, default should be null
    private SpriteRenderer mySprite;

    #endregion Variables

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        mySprite = GetComponent<SpriteRenderer>();
        
    }

    // Update is called once per frame
    void Update () {
        //Debug.Log(state);
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
            case States.attackStart:
                AttackStart();
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

        // Power ups
        if (powerupSpeedLength > 0)
        {
            powerupSpeedLength -= 1 * Time.deltaTime;
            moveSpeedBonus = powerupSpeedEffect;
            //transform.localScale = new Vector3(2f, 2f, 0);
        }
        else
        {
            moveSpeedBonus = 0;
            //transform.localScale = new Vector3(1, 1, 0);
        }

        if (powerupDamageLength > 0)
        {
            powerupDamageLength -= 1 * Time.deltaTime;
            damageBonus = powerupDamageEffect;
        }
        else
        {
            damageBonus = 0;
        }

        /*
        // Game won?
        GameObject testi = GameObject.FindWithTag("GameController");
        if (testi.GetComponent<GameController>().winOtters)
        {
            state = States.win;
        }
        */

        // Drawing order
        int i = (Mathf.RoundToInt(transform.position.y));
        mySprite.sortingOrder = -i;
        
    }

    // State: Idle
    private void Idle()
    {

        // Log
        Debug.Log("My state is now Idle");

        // Triggers
        #region Trigger
        
        // User inputs to move
        if (moveHorizontal != 0f || moveVertical != 0f )
        {
            //Debug.Log("Move button pressed");
            state = States.move;
        }

        // User input to attack
        if (buttonAttack)
        {
            //Debug.Log("Attack button pressed");
            state = States.attackStart;
            
        }

        #endregion Trigger

        #region Behaviour

        // Visual
        ChangeToAnimation("Idle");

        // Reset rotation
        transform.localEulerAngles = new Vector3(0, 0, 0);

        #endregion Behaviour

    }

    // State: Move
    private void Move()
    {
        
        //Debug.Log("My state is now move");

        #region Trigger

        // No user input
        if (moveHorizontal == 0.0f && moveVertical == 0.0f)
        {
            state = States.idle;            
        }

        // User inputs attack
        if (buttonAttack)
        {
            attackStarted = false;
            state = States.attackStart;        
        }

        // Dodge
        if (buttonDodge)
        {
            state = States.dodgeStart;
            if (dodge.sound) SoundManagerController.PlaySound("Fart");
            if (dodge.sound) Instantiate(fartPrefab, transform.position, transform.rotation);
        }

        #endregion Trigger

        #region Behaviour

        // Visual
        ChangeToAnimation("Walk");

        // Flip character
        Flip();

        // Keyboard movement
        #region Keyboard

        // Handle input, move character
        //Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0.0f);
        //GetComponent<Rigidbody2D>().velocity = movement * moveSpeed;

        //float moveHorizontal = Input.GetAxisRaw("Horizontal");
        //float moveVertical = Input.GetAxisRaw("Vertical");
        transform.Translate(new Vector3(moveHorizontal, moveVertical) * (moveSpeed + moveSpeedBonus) * Time.deltaTime, Space.World);

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

    // State: Attack Start/Init
    private void AttackStart()
    {
        #region Behaviour

        // Create attack prefab left or right
        if (!myAttack && !attackStarted)
        //if (!attackStarted && !myAttack)
        {

            float y2 = transform.position.y;

            if (facingRight)
            {
        
                float x2 = transform.position.x + attackRange;
                //myAttack = Instantiate(attackPrefab, new Vector3(x2, y2), Quaternion.identity, transform);
                myAttack = Instantiate(attackPrefab, new Vector3(x2, y2), Quaternion.identity);
                myAttack.GetComponent<AttackController>().myDamage = damage + damageBonus;
                myAttack.GetComponent<AttackController>().owner = gameObject;
            }
            else if (!facingRight)
            {
                
                float x2 = transform.position.x - attackRange;
                //myAttack = Instantiate(attackPrefab, new Vector3(x2, y2), Quaternion.identity, transform);
                myAttack = Instantiate(attackPrefab, new Vector3(x2, y2), Quaternion.identity);
                myAttack.GetComponent<SpriteRenderer>().flipX = true;
                myAttack.GetComponent<AttackController>().myDamage = damage + damageBonus;

            }

            myAttack.GetComponent<AttackController>().myEnemy = "Minion";
            attackStarted = true;
            // Sound
            SoundManagerController.PlaySound("Swish");
            // State
            state = States.attack;
            // Visual
            ChangeToAnimation("Attack");

        }

        #endregion Behaviour
    }

    // State: Attack
    private void Attack ()
    {
        
        // Log
        //Debug.Log("My state is now attack");

        // Triggers
        #region Triggers
        //Debug.Log(myAttack);
        //Debug.Log(attackStarted);
        // Go idle when attack is done
        if (!myAttack && attackStarted)
        {
            attackStarted = false;
            state = States.idle;
            //anim.SetBool("Attack", false);
            //anim.SetBool("Idle", true);
        }

        #endregion Triggers

        #region Behaviour


        #endregion Behaviour

    }

    // State: Dead
    private void Dead()
    {

        // Visual effect
        Instantiate(bloodPrefab, transform.position, transform.rotation);
        Instantiate(deathPrefab, transform.position, transform.rotation);

        // Sound
        SoundManagerController.PlaySound("GoblinDeath");

        // Destroy
        Destroy(gameObject);

    }

    // State: Win
    private void Win()
    {
        Debug.Log("My state is now win");
        transform.Rotate(new Vector3(0, 0, -1), Time.deltaTime * 1800, Space.Self);
    }

    // State: Dodge Start/Initialization
    private void DodgeStart()
    {

        if (0 <= 0) {

        }

    }

    // State: Dodge
    private void Dodge()
    {
        
        // Log
        Debug.Log("My state is now Dodge");

        // Trigger
        #region Trigger

        // Go idle if roll is done
        if (dodgeDistanceNow >= dodge.distance)
        {
            state = States.idle;
            dodgeStarted = false;
            //transform.localEulerAngles = new Vector3(0, 0, 0);
        }

        // User inputs attack
        if (buttonAttack)
        {
            state = States.attackStart;
            dodgeStarted = false;
        }

        #endregion Trigger

        // Behaviour
        #region Behaviour

        // Visual
        ChangeToAnimation("Dodge");

        // Initialize dodge
        if (!dodgeStarted)
        {
            dodgeStarted = true;
            dodgeDistanceNow = 0;
            
        }

        // Dodge 
        if (facingRight)
        {          
            float x = moveHorizontal * dodge.speed * Time.deltaTime;
            float y = moveVertical * dodge.speed * Time.deltaTime;
            transform.Translate(new Vector3(x, y), Space.World);
            dodgeDistanceNow += (1 * dodge.speed * Time.deltaTime) + (1 * dodge.speed * Time.deltaTime);
            //transform.Rotate(0, 0, Time.deltaTime * -10000);
            transform.Rotate(new Vector3(0,0,-1), Time.deltaTime * 1800, Space.Self);
        }           
        else
        {
            float x = moveHorizontal * dodge.speed * Time.deltaTime;
            float y = moveVertical * dodge.speed * Time.deltaTime;
            transform.Translate(new Vector3(x, y), Space.World);
            dodgeDistanceNow += (1 * dodge.speed * Time.deltaTime) + (1 * dodge.speed * Time.deltaTime);
            transform.Rotate(new Vector3(0,0,1), Time.deltaTime * 1800, Space.Self);
            
        }

        #endregion Behaviour

    }

    // Function: Flip character
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

    // Function: Read input
    private void readInput()
    {
        switch (playerNumber)
        {

            case 1:

                if (gamepad)
                {
                    moveHorizontal = Input.GetAxis("Gamepad Horizontal 1");
                    moveVertical = Input.GetAxis("Gamepad Vertical 1");
                    buttonAttack = Input.GetButtonDown("Gamepad Attack 1");
                    buttonDodge = Input.GetButtonDown("Gamepad Dodge 1");
                }
                else
                {
                    moveHorizontal = Input.GetAxis("Keyboard Horizontal 1");
                    moveVertical = Input.GetAxis("Keyboard Vertical 1");
                    buttonAttack = Input.GetButtonDown("Keyboard Attack 1");
                    buttonDodge = Input.GetButtonDown("Keyboard Dodge 1");
                }

                break;

            case 2:
                moveHorizontal = Input.GetAxis("Gamepad Horizontal 2");
                moveVertical = Input.GetAxis("Gamepad Vertical 2");
                buttonAttack = Input.GetButtonDown("Gamepad Attack 2");
                buttonDodge = Input.GetButtonDown("Gamepad Dodge 2");
                break;

            case 3:
                moveHorizontal = Input.GetAxis("Gamepad Horizontal 3");
                moveVertical = Input.GetAxis("Gamepad Vertical 3");
                buttonDodge = Input.GetButtonDown("Gamepad Dodge 3");
                buttonAttack = Input.GetButtonDown("Gamepad Attack 3");
                break;

            case 4:
                moveHorizontal = Input.GetAxis("Horizontal_P4");
                moveVertical = Input.GetAxis("Vertical_P4");
                break;

        }

    }

    // Function: Handle incoming damage
    private void handleDamage()
    {
        
        if (state == States.dodge && dodge.safety)
        {
            damageTaken = 0;
        }
        else
        {

            // Take damage
            hitPoints -= damageTaken;
            damageTaken = 0;

            // Special effects
            StartCoroutine("HurtColor");
            Instantiate(bloodPrefab, transform.position, transform.rotation);
            SoundManagerController.PlaySound("Hit");

        }         

    }

    // Function: Change animation
    private void ChangeToAnimation(string newAnimation)
    {

        //Debug.Log("Animation changed");

        anim.SetBool("Idle", false);
        anim.SetBool("Attack", false);
        anim.SetBool("Walk", false);
        anim.SetBool("Dodge", false);

        anim.SetBool(newAnimation, true);

    }

    // Function: Hit flash
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
