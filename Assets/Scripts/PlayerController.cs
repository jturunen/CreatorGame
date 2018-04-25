// using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// using Random=UnityEngine.Random;

// Class that holds all attack related things
[System.Serializable]
public class Attack
{
    public float range; // How far attack lands
    public float minDamage; // Minimum damage to deal
    public float maxDamage; // Maximum damage to deal
    public float speed; // How many attacks per second

    public float damage()
    {
        return UnityEngine.Random.Range(minDamage, maxDamage);
    }

}

// Class that holds all dodge related things
[System.Serializable]
public class Dodge
{
    public float cooldown; // Time character has to wait between dodges
    public float distance; // How long distance dodge will last
    public float speed; // How fast dodge moves
    public bool safety; // Does character have invulnerability during dodge?
    public bool sound; // Play sound effect for dodge?
    public bool visual; // Show visual effect for dodge?
    public GameObject visualEffect; // Visual effect for dodge
    [System.NonSerialized]
    public float timeSinceLastDodge; // Time since last dodge
}

// Class that holds all powerup related things
[System.Serializable]
public class PowerUp
{
    
    public float speedDuration; // How long the speed power up lasts
    public float speedEffect; // How much speed the speed power up gives
    
    public float damageDuration; // Power Up Damage Length
    public float damageEffect; // Power Up Damage Effect

    public float armorDuration; // PowerUp Armor Duration
    public float armorEffect; // PowerUp Armor Effect Strength

}

public class PlayerController : MonoBehaviour {

    #region Variables

    Animator anim;
    
    //[System.NonSerialized]
    [Header("header test from ilpo")]
    public Attack attack; // Create variable that holds all attack stats neatly
    public Dodge dodge; // Create variable that holds all dodge stats neatly
    public PowerUp powerup; // Create variable that holds all powerup stats neatly
    public int playerNumber; // What is the player number
    public float moveSpeed = 0.0f; // Movement speed
    public float hitPoinsMax; // Maximum hit points of character
    public float hitPoints = 0.0f; // Health
    public float flashLength; // How long hitflash will last
    [HideInInspector] public float damageTaken = 0f; // Amount of incoming damage
    [Header("header test from ilpo")]
    public bool jumpKick; // Should character "jump kick" coming out of dodge?
    public bool gamepad; // Gamepad controlled instead of keyboard?
    public bool goldyAim; // Is Goldy aiming at this character?
    public GameObject attackPrefab; // The attack prefab this character uses
    public GameObject deathPrefab; // Death particle/effect
    public GameObject bloodPrefab; // Blood particle/effect
    public ParticleSystem bloodParticles;
    public GameObject fartPrefab; // Fart particle/effect
    public Image healthBar; // Health bar of this character
    public Canvas healthBarCanvas; // Health bar canvas for this character
    public GameObject weapon; // What weapon is this character using
    public GameObject dodgeIndicator; // Indicator to show if dodge is ready to use
    public GameObject goldyAimReticle; // Indicator to show if Goldy is shooting this character


    private float moveHorizontal; // Horizontal movement, keyboard
    private float moveVertical; // Vertical movement, keyboard
    private float moveHorizontal_P1; // Horizontal movement, gamepad
    private float moveVertical_P1; // Vertical movement, gamepad
    private float flashCounter; // Duration of hitflash
    private float dodgeDistanceNow; // Distance dodge rolled so far
    private float moveSpeedBonus; // Bonus move speed from speed power up
    private float damageBonus; // Current damage bonus
    private float m_MySliderValue; //Value from the slider, and it converts to speed level
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
        dodge.timeSinceLastDodge = dodge.cooldown;
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
            case States.dodgeStart:
                DodgeStart();
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
        updatePowerUps();

        /*
        // Game won?
        GameObject testi = GameObject.FindWithTag("GameController");
        if (testi.GetComponent<GameController>().winOtters)
        {
            state = States.win;
        }
        */
        if (GameObject.FindGameObjectWithTag("winOtters"))
        {
            state = States.win;
        }

        // Drawing order
        DrawOrder();

        // Update dodge timer
        if (dodge.timeSinceLastDodge < dodge.cooldown)
        {
            dodge.timeSinceLastDodge += 1 * Time.deltaTime;
        }
        else if (dodge.timeSinceLastDodge >= dodge.cooldown)
        {
            dodgeIndicator.GetComponent<SpriteRenderer>().enabled = true;
        }
        
        // Update health bar
        if (state == States.idle || state == States.move) 
        {
            
            if (healthBarCanvas.enabled == false)
            {
                healthBarCanvas.enabled = true;
            }
            
            healthBar.fillAmount = hitPoints / hitPoinsMax;
            
        }

        // Update Goldy aim reticle
        if (goldyAim)
        {
            goldyAimReticle.GetComponent<SpriteRenderer>().enabled = true;
        }
        else
        {
            goldyAimReticle.GetComponent<SpriteRenderer>().enabled = false;
        }
        if (true)
        {
            goldyAim = false;
        }

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
        
        Debug.Log("My state is now move");

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
        if (buttonDodge && dodge.timeSinceLastDodge >= dodge.cooldown)
        {
            state = States.dodgeStart;
            if (dodge.sound) SoundManagerController.instance.PlaySound("Fart", 1f);
            if (dodge.sound) Instantiate(dodge.visualEffect, transform.position, transform.rotation);
        }

        #endregion Trigger

        #region Behaviour

        // Visual
        ChangeToAnimation("Walk");

        // Flip character
        Flip();

        // Movement
        transform.Translate(new Vector3(moveHorizontal, moveVertical) * (moveSpeed + moveSpeedBonus) * Time.deltaTime, Space.World);

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
        
                float x2 = transform.position.x + attack.range;
                //myAttack = Instantiate(attackPrefab, new Vector3(x2, y2), Quaternion.identity, transform);
                myAttack = Instantiate(attackPrefab, new Vector3(x2, y2), Quaternion.identity);
                myAttack.GetComponent<AttackController>().myDamage = attack.damage() + damageBonus;
                myAttack.GetComponent<AttackController>().lifetime = 1/attack.speed;
            }
            else if (!facingRight)
            {
                
                float x2 = transform.position.x - attack.range;
                //myAttack = Instantiate(attackPrefab, new Vector3(x2, y2), Quaternion.identity, transform);
                myAttack = Instantiate(attackPrefab, new Vector3(x2, y2), Quaternion.identity);
                //myAttack.GetComponent<SpriteRenderer>().flipX = true;
                myAttack.transform.localScale = new Vector3(-1, 1, 1);
                myAttack.GetComponent<AttackController>().myDamage = attack.damage() + damageBonus;
                myAttack.GetComponent<AttackController>().lifetime = 1/attack.speed;

            }

            myAttack.GetComponent<AttackController>().myEnemy = "Minion";
            myAttack.GetComponent<AttackController>().owner = gameObject;
            
            attackStarted = true;
            // Sound
            SoundManagerController.instance.PlaySound("Swish", 1f);
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

        // Go idle when attack is done
        if (!myAttack && attackStarted)
        {
            attackStarted = false;
            state = States.idle;
            // Reset animation speed back to normal
            anim.speed = 1;
        }

        #endregion Triggers

        #region Behaviour

        // Change animation speed to match attack speed
        anim.speed = attack.speed;

        #endregion Behaviour

    }

    // State: Dead
    private void Dead()
    {

        // Visual effect
        Instantiate(bloodPrefab, transform.position, transform.rotation);
        Instantiate(deathPrefab, transform.position, transform.rotation);

        // Sound
        SoundManagerController.instance.PlaySound("GoblinDeath", 1f);

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
        // Log
        Debug.Log("My state is now Dodge Start");

        // Start dodge
        if (dodge.timeSinceLastDodge >= dodge.cooldown) 
        {
            dodge.timeSinceLastDodge = 0;
            state = States.dodge;
        }
        else
        {
            state = States.idle;
        }

        // Disable health bar
        healthBarCanvas.enabled = false;

        // Disable dodge indicator
        dodgeIndicator.GetComponent<SpriteRenderer>().enabled = false;

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
            
            // Reset rotation
            if (!jumpKick) transform.localEulerAngles = new Vector3(0, 0, 0);

        }

        // User inputs attack
        if (buttonAttack)
        {
            state = States.attackStart;
            dodgeStarted = false;

            // Reset rotation
            if (!jumpKick) transform.localEulerAngles = new Vector3(0, 0, 0);

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
            // Rotate clockwise
            transform.Rotate(new Vector3(0,0,-1), Time.deltaTime * 1800, Space.Self);
        }           
        else
        {
            float x = moveHorizontal * dodge.speed * Time.deltaTime;
            float y = moveVertical * dodge.speed * Time.deltaTime;
            transform.Translate(new Vector3(x, y), Space.World);
            dodgeDistanceNow += (1 * dodge.speed * Time.deltaTime) + (1 * dodge.speed * Time.deltaTime);
            // Rotate anticlockwise
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
            StartCoroutine(HurtColor());
            // Instantiate(bloodPrefab, transform.position, transform.rotation);
            // bloodParticles.Play();
            // GameController.instance.bloodParticles.Play();
            GameController.instance.CreateParticle(transform.position);
            SoundManagerController.instance.PlaySound("Hit", 1f);

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

    // Function: Update PowerUp things
    private void updatePowerUps()
    {
        if (powerup.speedDuration > 0)
        {
            powerup.speedDuration -= 1 * Time.deltaTime;
            moveSpeedBonus = powerup.speedEffect;
            //transform.localScale = new Vector3(2f, 2f, 0);
        }
        else
        {
            moveSpeedBonus = 0;
            //transform.localScale = new Vector3(1, 1, 0);
        }

        if (powerup.damageDuration > 0)
        {
            powerup.damageDuration -= 1 * Time.deltaTime;
            damageBonus = powerup.damageEffect;
        }
        else
        {
            damageBonus = 0;
        }
    }

    // Function: Drawing order
    void DrawOrder()
    {

        int i = (Mathf.RoundToInt(transform.position.y*1000));
        mySprite.sortingOrder = -i;

        if (weapon) 
        {
            weapon.GetComponent<Weapon>().mySprite.sortingOrder = -i;
        }

    }

    /*
    void OnGUI()
    {
        //Create a Label in Game view for the Slider
        GUI.Label(new Rect(0, 25, 40, 60), "Speed");
        //Create a horizontal Slider to control the speed of the Animator. Drag the slider to 1 for normal speed.

        m_MySliderValue = GUI.HorizontalSlider(new Rect(45, 25, 200, 60), m_MySliderValue, 0.0F, 1.0F);
        //Make the speed of the Animator match the Slider value
        anim.speed = m_MySliderValue * 10;
    }
    
     */

}
