using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    #region Variables

    public string myName = ""; // Used for initializing different monsters
    public float moveSpeed = 1.0f; // Movement speed
    public float hitPoints = 1.0f;
    public float attackSpeed = 0.0f; // How low time between shots
    public float attackRange = 1.0f; // Distance how long attack will be started from
    public float fleeRange = 1.0f; // Distance how close Otters can be before fleeing 
    public bool isControlled = false;
    public bool ranged = false;
    public GameObject attackPrefab; // Attack animation/sprite/whateverthefuck
    public GameObject deathPrefab; // Death particle/effect
    public GameObject weaponPrefab; // Currently held weapon

    //Transform player;
    //Animator anim;

    private enum States {idle, chase, attack, flee, dead}; // Enum for state machine
    private States state = States.idle;
    //private string state = "idle"; // Start of the state machine, turn into enums later
    private Transform target; // Current target AI should chase
    private GameObject myAttack; // Current attack   
    private bool facingRight = true; // Direction to look at
    private SpriteRenderer spriterend;
    private float timeSinceLastAttack = 0f; // Time since last attack

    #endregion

    // Use this for initialization
    void Start()
    {
        //anim = GetComponent<Animator>();
        //player = GameObject.FindGameObjectWithTag("Player").transform;
        //target = GameObject.FindGameObjectWithTag("Player").transform;
        //spriterend = transform.Find("EnemySprite").GetComponent<SpriteRenderer>();

        //timeSinceLastAttack = attackSpeed;

    }

    // Update is called once per frame
    void Update()
    {
                      
        //Move();

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

    // Fixed Update
    private void FixedUpdate()
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
        }

        // Attack speed
        if (timeSinceLastAttack > 0)
        {
            timeSinceLastAttack -= Time.deltaTime;
        }

        // Health/Death
        if (hitPoints <= 0)
        {
            state = States.dead;
        }

    }

    // Stand idle if Otters do not exist
    private void Idle() 
    {

        //--- Behaviour
        Debug.Log("My state is Idle");

        //--- Trigger
        //if (Random.Range(0, 60) == 0)
        if (true)
        {
            if (GameObject.FindWithTag("Player"))
            {
                target = FindClosestEnemy("Player").transform;
                //target = GameObject.FindGameObjectWithTag("Player").transform;
                state = States.chase;
            }
        }
        //--- Audiovisual
        

    }

    // Chase Otters, if they exist outside attack range
    private void Chase() 
    {
        Debug.Log("My state is now chase");

        //--- Triggers
        if (!target)
        {
            state = States.idle;
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
            return;
        }

        //--- Behaviour
        
        if (target)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
            //state = States.idle;
        }

        //--- Audiovisual
        // Flip
        Flip();

        //state = States.idle;
    }

    // Attack Otters, if they exist inside attack range
    private void Attack() 
    {

        //--- Behaviour
        Debug.Log("My state is now attack");

        // Attack
        if (target && Vector2.Distance(transform.position, target.position) < attackRange) 
        {

            if (!ranged && !myAttack)
            {
                // Create attack
                float x2 = target.position.x;
                float y2 = target.position.y;
                myAttack = Instantiate(attackPrefab, new Vector3(x2, y2), Quaternion.identity);
            }
            else if (ranged && timeSinceLastAttack <= 0)
            {
                timeSinceLastAttack = attackSpeed;
                float x2 = transform.position.x;
                float y2 = transform.position.y;
                myAttack = Instantiate(attackPrefab, new Vector3(x2, y2), Quaternion.identity);
                myAttack.GetComponent<BulletController>().target = target;              
            }
            

        }

        //--- Trigger
        if (!ranged && !myAttack)
        {

            // No Otters?
            if (!target)
            {
                state = States.idle;
            }
            // Otters too far away?
            else if (Vector2.Distance(transform.position, target.position) > attackRange)
            {
                state = States.chase;
            }

        }
        else if (ranged)
        {
            // No Otters?
            if (!target)
            {
                state = States.idle;
            }
            // Otters too far away?
            else if (Vector2.Distance(transform.position, target.position) > attackRange)
            {
                state = States.chase;
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
        Instantiate(deathPrefab, transform.position, transform.rotation);
        Destroy(gameObject);
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

    /*
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

	#region Flip
    private void Flip()
    {
        Debug.Log("Flip");
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
	#endregion Flip
    */

}
