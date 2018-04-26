using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Bigsy
{
    public int specialCount; // How many specials can Bigsy do
    public float chargeUpDuration; // Duration of charge up
    public float specialAttackDuration; // Duration of special attack
    public float specialAttackDamage; // Damage that is dealt with special attack
    public GameObject specialAttack; // Bigsys special attack
    // public GameObject attackDamageSprite; // Sprite that is up when damage is dealt

    public float chargeUpDurationNow; // How long current charge up has lasted
    
}

public class EnemyController : MonoBehaviour {
    
    #region Variables
    public Bigsy bigsy; // Bigsy related variable class
    public string myName = ""; // Used for initializing different monsters
    public float moveSpeed = 1.0f; // Movement speed
    public float hitPoints = 1.0f;
    public float hitPointsMax; // Maximum amount of health
    public float safetyDuration; // Duration of safety/invulnerability/damage immunity
    public float safetyDurationHotswap; // Duration of safety when minion control is changed
    public float attackCooldown; // How low time between attacks
    public float attackRange = 1.0f; // Distance how long attack will be started from
    public float fleeRange = 1.0f; // Distance how close Otters can be before fleeing 
    public float damageTaken = 0f; // Damage incoming from outside
    public float magazineSize; // Size of the magazine
    public float reloadTime; // Time to reload
    public float stun; // Time character is stunned
    public float accuracy; // How accurate does the character shoot
    public float damage; // How much damage to deal;
    public float attackSpeed; // Attack speed of this character
    public float gunSoundVolume; // Sound volume level
    public bool isControlled = false;
    public bool ranged = false;
    public bool friendlyFire; // Should character damage friendly targets
    public GameObject attackPrefab; // Attack animation/sprite/whateverthefuck
    public GameObject attackPrefabMobBoss; // Attack object for melee mob boss
    public GameObject deathPrefab; // Death particle/effect
    public GameObject bloodPrefab; // Blood particle/effect
    public GameObject controlledPrefab; // Visual indicator for character being controlled
    public GameObject bottlePrefab; // Bottle prefab for special attack
    public GameObject weapon; // What weapon this character uses
    public GameObject gunTip; // Where is tip of the gun for projectile start place
    public GameObject loot; // What loot this character drops
    public GameObject feets; // Where is feets of this character located at
    public bool spawnChoise = false;

    private enum States {idle, chase, attack, flee, dead, reload, stun, hurt,
    chargeUp, specialAttack}; // Enum for state machine
    private States state = States.idle;
    //private string state = "idle"; // Start of the state machine, turn into enums later
    private Transform target; // Current target AI should chase
    private GameObject myAttack; // Current attack   
    private GameObject myBullet; // Current attack bullet   
    private SpriteRenderer mySprite; // My sprite
    private bool facingRight = true; // Direction to look at
    private float timeSinceLastAttack = 0f; // Time since last attack
    private float bullets;
    private float timeSpentReloading;
    private Animator myAnimator;
    private float moveHorizontal; // Horizontal movement, keyboard
    private float moveVertical; // Vertical movement, keyboard
    private float moveHorizontal_P1; // Horizontal movement, gamepad
    private float moveVertical_P1; // Vertical movement, gamepad
    private bool buttonAttack; // Horizontal movement, gamepad
    private bool buttonDodge; // Vertical movement, gamepad
    
    #endregion
    
    // Use this for initialization
    void Start()
    {
        //Debug.Log("AAAAAAAAAAAAAAAAAAAAAAAAAA");
        mySprite = GetComponent<SpriteRenderer>();
        bullets = magazineSize;
        myAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.instance.gamepadMobBoss)
        {
            moveHorizontal = Input.GetAxis("Gamepad Horizontal 2");
            moveVertical = Input.GetAxis("Gamepad Vertical 2");
            buttonAttack = Input.GetButtonDown("Gamepad Attack 2");
            buttonDodge = Input.GetButtonDown("Gamepad Dodge 2");
        }
        else 
        {
            moveHorizontal = Input.GetAxis("Keyboard Horizontal 2");
            moveVertical = Input.GetAxis("Keyboard Vertical 2");
            buttonAttack = Input.GetButtonDown("Keyboard Attack 2");
            buttonDodge = Input.GetButtonDown("Keyboard Dodge 2");
        }

    }

    // Fixed Update
    private void FixedUpdate()
    {       

        if (!isControlled)
        {
            // States
            switch (state)
            {
                case States.idle:
                    Idle();
                    break;
                case States.chase:
                    Chase();
                    break;
                case States.attack:
                    Attack();
                    break;
                case States.flee:
                    Flee();
                    break;
                case States.dead:
                    Dead();
                    break;
                case States.reload:
                    Reload();
                    break;
                case States.hurt:
                    Hurt();
                    break;
                case States.stun:
                    Stun();
                    break;
                case States.chargeUp:
                    ChargeUp();
                    break;
                case States.specialAttack:
                    SpecialAttack();
                    break;
                
            }
        }
        else if (isControlled)
        {
            // Movement
            if (!myAttack)
            {
                ChangeToAnimation("Walk");
                transform.Translate(new Vector3(moveHorizontal, moveVertical) * moveSpeed * Time.deltaTime * 3);

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
            
            // Health
            if (hitPoints <= 0)
            {
                //isControlled = false;
                //state = States.dead;
                Dead();
            }
            else if (damageTaken > 0)
            {
                if (safetyDuration > 0)
                {
                    damageTaken = 0;
                }
                else
                {
                    Hurt();
                }
            }

            // My attack
            if (buttonAttack && !myAttack)
            {
                
                // Melee attack
                if (!ranged)
                {
                    // Create attack object
                    float x2 = transform.position.x;
                    float y2 = transform.position.y;
                    myAttack = Instantiate(attackPrefabMobBoss, new Vector3(x2, y2), transform.rotation);                      
                    
                    // Send stats to attack
                    if (facingRight) myAttack.GetComponent<AttackController>().facingRight = true;
                    myAttack.GetComponent<AttackController>().owner = gameObject;
                    myAttack.GetComponent<AttackController>().myEnemy = "Player";
                    myAttack.GetComponent<AttackController>().myDamage = damage;
                    myAttack.GetComponent<AttackController>().lifetime = 1/attackSpeed;

                    // Visual
                    ChangeToAnimation("Attack");
                    
                    // Sound
                    SoundManagerController.instance.PlaySound("Swish", 1f);
                    
                }

                // Ranged attack
                if (ranged && timeSinceLastAttack <= 0)
                {
                    bullets -= 1;
                    timeSinceLastAttack = attackCooldown;
                    float x2 = gunTip.transform.position.x;
                    float y2 = gunTip.transform.position.y;
                    myBullet = Instantiate(attackPrefab, new Vector3(x2, y2), Quaternion.identity);
                    myBullet.GetComponent<BulletController>().owner = gameObject;
                    myBullet.GetComponent<BulletController>().accuracy = accuracy;
                    myBullet.GetComponent<BulletController>().damage = damage;
                    myBullet.GetComponent<BulletController>().controlled = true;

                    if (moveHorizontal != 0 || moveVertical != 0)
                    {
                        myBullet.GetComponent<BulletController>().direction = new Vector2(moveHorizontal, moveVertical);
                    }
                    else 
                    {
                        if (facingRight)
                            myBullet.GetComponent<BulletController>().direction = new Vector2(1, 0);
                        else
                            myBullet.GetComponent<BulletController>().direction = new Vector2(-1, 0);

                    }

                    SoundManagerController.instance.PlaySound("Gun", gunSoundVolume);
                
                }
                
            }

            // Change controlled minion, hotswap
            if (Input.GetKeyDown(KeyCode.Keypad1))
            {             
                GameObject[] minionList;
                minionList = GameObject.FindGameObjectsWithTag("Minion");
                
                if (minionList.Length > 1)
                {                   
                    isControlled = false;
                    GameObject newControlled = minionList[Random.Range(0, minionList.Length)];                        
                    newControlled.GetComponent<EnemyController>().isControlled = true;
                } 

            }

        }
        
        // Attack speed
        if (timeSinceLastAttack > 0)
        {
            timeSinceLastAttack -= Time.deltaTime;
        }

        if (damageTaken > 0)
        {
            if (safetyDuration > 0)
            {
                damageTaken = 0;
            }
            else if (safetyDuration <= 0)
            {
                state = States.hurt;
            }
        }

        // Health/Death
        if (hitPoints <= 0)
        {
            state = States.dead;
        }
        /*
        // Drawing order
        int i = (Mathf.RoundToInt(transform.position.y));
        mySprite.sortingOrder = -i;
        */

        // Visual indicator for being controlled
        if (!isControlled)
        {
            controlledPrefab.SetActive(false);
        }
        else if (isControlled)
        {
            controlledPrefab.SetActive(true);
        }

        // Update safety duration
        if (safetyDuration > 0.0f)
        {
            safetyDuration -= 1 * Time.deltaTime;
        }

        // Throw special attack bottle
        if (bottlePrefab && !isControlled && target)
        {
            if (Random.Range(0,60) == 0)
            {
                GameObject myBottle;
                float x2 = transform.position.x;
                float y2 = transform.position.y;
                myBottle = Instantiate(bottlePrefab, new Vector3(x2, y2), Quaternion.identity);
                myBottle.GetComponent<BulletController>().target = target;
                bottlePrefab = null;

            }

        }

        // Roll after win
        if (GameController.instance.winDance && GameObject.FindGameObjectWithTag("winMobBoss"))
        {
            transform.Rotate(new Vector3(0, 0, -1), Time.deltaTime * 1800, Space.Self);
        }

        // Keep searching for closest enemy as Goldy
        if (ranged && Random.Range(0, 60) == 0)
        {
            if (GameObject.FindWithTag("Player"))
            {
                target = FindClosestEnemy("Player").transform;
                // Update goldy aim reticle
                // target.GetComponent<PlayerController>().goldyAim = true;
            }
        }

        // Drawing order
        int i = (Mathf.RoundToInt(transform.position.y*1000));
        mySprite.sortingOrder = -i;

    }

    // Stand idle if Otters do not exist
    private void Idle() 
    {
        //--- Behaviour
        Debug.Log("My state is Idle");
        
        //--- Trigger
        if (Random.Range(0, 10) == 0)
        {
            if (GameObject.FindWithTag("Player"))
            {
                target = FindClosestEnemy("Player").transform;
                //target = GameObject.FindGameObjectWithTag("Player").transform;
                state = States.chase;
                ChangeToAnimation("Walk");

            }

        }       

    }

    // Chase Otters, if they exist outside attack range
    private void Chase() 
    {
        Debug.Log("My state is now chase");

        //--- Triggers
        if (!target)
        {
            state = States.idle;
            ChangeToAnimation("Idle");
            return;
        }
        else if (Vector2.Distance(transform.position, target.position) < fleeRange)
        {
            state = States.flee;
            return;
        }
        else if (Vector2.Distance(transform.position, target.position) < attackRange)
        {
            state = States.attack;
            ChangeToAnimation("Attack");
            return;
        }

        //--- Behaviour
        
        if (target)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
            //state = States.idle;
        }

        //--- Audiovisual
        Flip();
        
    }

    // Attack Otters, if they exist inside attack range
    private void Attack() 
    {

        //--- Behaviour
        Debug.Log("My state is now attack");

        // Attack
        if (target && Vector2.Distance(transform.position, target.position) < attackRange) 
        {

            // Melee attack
            if (!ranged && !myAttack)
            {
                // Create attack
                float x2 = target.position.x;
                float y2 = target.position.y;
                myAttack = Instantiate(attackPrefab, new Vector3(x2, y2), Quaternion.identity);
                myAttack.GetComponent<AttackController>().owner = gameObject;
                myAttack.GetComponent<AttackController>().myEnemy = "Player";
                myAttack.GetComponent<AttackController>().myFriend = "Minion";
                myAttack.GetComponent<AttackController>().myDamage = damage;
                myAttack.GetComponent<AttackController>().friendlyFire = friendlyFire;
                myAttack.GetComponent<AttackController>().lifetime = 1/attackSpeed;
                if (facingRight)
                    myAttack.GetComponent<AttackController>().facingRight = true;
                SoundManagerController.instance.PlaySound("Swish", 1f);
                
            }
            // Ranged attack
            else if (ranged && timeSinceLastAttack <= 0 && bullets > 0)
            {
                bullets -= 1;
                timeSinceLastAttack = attackCooldown;
                float x2 = gunTip.transform.position.x;
                float y2 = gunTip.transform.position.y;
                myAttack = Instantiate(attackPrefab, new Vector3(x2, y2), Quaternion.identity);
                myAttack.GetComponent<BulletController>().owner = gameObject;
                myAttack.GetComponent<BulletController>().target = target;
                myAttack.GetComponent<BulletController>().accuracy = accuracy;
                myAttack.GetComponent<BulletController>().damage = damage;
                SoundManagerController.instance.PlaySound("Gun", gunSoundVolume);
            }
            else if (ranged && bullets <= 0)
            {
                state = States.reload;
                SoundManagerController.instance.PlaySound("Reload", 1f);
            }

        }
        
        //--- Trigger
        
        // Bigsy special attack trigger
        if (myName == "Bigsy" && bigsy.specialCount >= 1 && hitPoints < (hitPointsMax/2))
        {
            state = States.chargeUp;
            ChangeToAnimation("ChargeUp");
        }

        if (!ranged && !myAttack)
        {

            // No Otters?
            if (!target)
            {
                state = States.idle;
                ChangeToAnimation("Idle");
            }
            // Otters too far away?
            else if (Vector2.Distance(transform.position, target.position) > attackRange)
            {
                state = States.chase;
                ChangeToAnimation("Walk");
                
            }

        }
        else if (ranged)
        {
            // No Otters?
            if (!target)
            {
                state = States.idle;    
                ChangeToAnimation("Idle");           
            }
            // Otters too far away?
            else if (Vector2.Distance(transform.position, target.position) > attackRange)
            {
                state = States.chase;
                ChangeToAnimation("Walk");
            }
            // Otters too close?
            else if (Vector2.Distance(transform.position, target.position) < fleeRange)
            {
                state = States.flee;
            }

        }

        //--- Audiovisual
        Flip();

        //state = States.idle;
    }

    // Reload state
    private void Reload()
    {

        ChangeToAnimation("Idle");

        // Log
        Debug.Log("My state is now reload");

        // Reload done, go idle
        if (ranged && bullets >= magazineSize)
        {
            state = States.idle;
            ChangeToAnimation("Idle");          
        }

        // Reload
        if (ranged && bullets <= 0)
        {

            if (timeSpentReloading < reloadTime)
            {
                timeSpentReloading += 1 * Time.deltaTime;
            }
            else
            {
                timeSpentReloading = 0;
                bullets = magazineSize;
            }

        }

    }

    // Flee from Otters, if they exist inside flee range
    private void Flee()
    {
        //--- Behaviour
        Debug.Log("My state is now flee");
        if (target && Vector2.Distance(transform.position, target.position) < fleeRange)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, -moveSpeed * Time.deltaTime);
        }
        else
        {
            state = States.idle;
        }

        Flip();
    }

    // Dead if dead
    private void Dead()
    {

        // Tell GameController about death
        //GameObject testi = GameObject.FindWithTag("GameController");
        //testi.GetComponent<GameController>().minionsKilled += 1;

        // Visual effect
        Instantiate(bloodPrefab, transform.position, transform.rotation);
        float z = Random.Range(1f, 360f);
        float xy = Random.Range(0.0f, 1.0f);
        Instantiate(deathPrefab, transform.position, Quaternion.Euler(xy, xy, z));

        // Sound
        SoundManagerController.instance.PlaySound("GoblinDeath", 1f);
        //SoundManagerController.instance.PlaySound("Explosion");

        // Choose new minion to control
        if (isControlled)
        {
            GameObject[] minionList;
            minionList = GameObject.FindGameObjectsWithTag("Minion");
            if (minionList.Length > 1)
            {
                GameObject newControlled = minionList[Random.Range(0, minionList.Length)];
                do 
                {
                    newControlled = minionList[Random.Range(0, minionList.Length)];
                }
                while (newControlled.GetComponent<EnemyController>().isControlled == true);
                newControlled.GetComponent<EnemyController>().isControlled = true;
                newControlled.GetComponent<EnemyController>().safetyDuration += safetyDurationHotswap;
            }
            
        }

        // Destroy
        Instantiate(loot, transform.position, transform.rotation);
        Destroy(gameObject);

    }

    // Take damage if hurt
    private void Hurt()
    {

        state = States.stun;
        hitPoints -= damageTaken;
        damageTaken = 0;
        
        // Special effects
        Instantiate(bloodPrefab, transform.position, transform.rotation);
        SoundManagerController.instance.PlaySound("Hit", 1f);
        StartCoroutine("HurtColor");

    }

    // Stun state
    private void Stun()
    {

        #region Triggers

        // Taking damage
        if (damageTaken > 0)
        {
            state = States.hurt;
        }

        // Stun ends
        if (stun <= 0)
        {
            state = States.idle;
            ChangeToAnimation("Idle");
        }

        #endregion Triggers

        #region Behaviour

        // Lower stun duration
        if (stun > 1) stun = 1;
        stun -= 1 * Time.deltaTime;

        #endregion Behaviour

    }

    // State: Bigsy charge up
    void ChargeUp()
    {

        #region Triggers
        
        // Start special attack when charge up is done
        if (bigsy.chargeUpDurationNow >= bigsy.chargeUpDuration)
        {
            // bigsy.chargeUpDurationNow = 0;
            state = States.specialAttack;
            ChangeToAnimation("SpecialAttack");
            return;
        }

        #endregion Triggers

        #region Behaviour

        // Log
        Debug.Log("My state is now ChargeUp");

        // Update charge up duration
        bigsy.chargeUpDurationNow += Time.deltaTime;

        // Safety during charge up
        safetyDuration += 60;

        #endregion Behaviour

    }

    // State: Bigsy special attack
    void SpecialAttack()
    {
        #region Triggers

        if (!myAttack && bigsy.specialCount <= 0)
        {
            state = States.idle;
            ChangeToAnimation("Idle");
        }

        #endregion Triggers

        #region Behaviour

        // Log
        Debug.Log("My state is now SpecialAttack");

        // Special Attack
        if (bigsy.specialCount >= 1)
        {
            bigsy.specialCount--;
            myAttack = Instantiate(bigsy.specialAttack, feets.transform.position, transform.rotation);
            myAttack.GetComponent<AttackController>().lifetime = bigsy.specialAttackDuration;
            myAttack.GetComponent<AttackController>().myDamage = bigsy.specialAttackDamage;
            myAttack.GetComponent<AttackController>().owner = gameObject;

        }

        #endregion Behaviour

    }

    // Find closest player
    public GameObject FindClosestEnemy(string tag)
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag(tag);
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }

    // Flip if necessary
    private void Flip()
    {
        if (target)
        {
            if (target.position.x < transform.position.x && facingRight)
            {
                facingRight = false;
                Vector3 theScale = transform.localScale;
                theScale.x *= -1;
                transform.localScale = theScale;
            }
            else if (target.position.x > transform.position.x && !facingRight)
            {
                facingRight = true;
                Vector3 theScale = transform.localScale;
                theScale.x *= -1;
                transform.localScale = theScale;
            }
        }
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

    // Function: Change animation
    private void ChangeToAnimation(string newAnimation)
    {

        //Debug.Log("Animation changed");

        myAnimator.SetBool("Idle", false);
        myAnimator.SetBool("Attack", false);
        myAnimator.SetBool("Walk", false);
        myAnimator.SetBool("Dodge", false);
        myAnimator.SetBool("SpecialAttack", false);
        myAnimator.SetBool("ChargeUp", false);


        myAnimator.SetBool(newAnimation, true);

    }

    // Template
    void Template()
    {
        #region Triggers

        #endregion Triggers

        #region Behaviour

        #endregion Behaviour

    }

}
