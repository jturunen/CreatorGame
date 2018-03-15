using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    #region Variables

    public float moveSpeed = 0.0f; // Movement speed
    public float hitPoints = 0.0f;
    public float attackSpeed = 0.0f;
    public float attackRange = 0.0f; // Distance how long attack will be started
    public bool isControlled = false;
    public GameObject attackPrefab; // Attack animation/sprite/whateverthefuck

    bool facingRight = true;
    Transform player;
    Animator anim;

    private enum States {idle, chase, attack}; // Enum for state machine
    private States state = States.idle;
    //private string state = "idle"; // Start of the state machine, turn into enums later
    private Transform target; // Current target AI should chase
    private GameObject myAttack; // Current attack   

    #endregion

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        //target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
        switch(state)
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
        }
        
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

    // Stand idle if Otters do not exist
    private void Idle() 
    {

        //--- Behaviour
        Debug.Log("My state is Idle");

        //--- Trigger
        if(GameObject.FindWithTag("Player"))
        {
            target = FindClosestEnemy("Player").transform;
            //target = GameObject.FindGameObjectWithTag("Player").transform;
            state = States.chase;
        }

        //--- Audiovisual
        

    }

    // Chase Otters, if they exist outside attack range
    private void Chase() 
    {
        //--- Behaviour
        Debug.Log("My state is now chase");
        if (target)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        }

        //--- Trigger
        if (!target)
        {
            state = States.idle;
        }
        else if (Vector2.Distance(transform.position, target.position) < attackRange)
        {
            state = States.attack;
        }

        //--- Audiovisual
    }

    // Attack Otters, if they exist inside attack range
    private void Attack() 
    {

        //--- Behaviour
        Debug.Log("My state is now attack");

        // Attack
        if (target && Vector2.Distance(transform.position, target.position) < attackRange) 
        {
            if (myAttack == null)
            {

                // Find X position
                float xx = gameObject.transform.position.x; 
                if (target.position.x < transform.position.x - 1f)
                {
                    xx -= 1f;
                }
                else if (target.position.x > transform.position.x + 1f)
                {
                    xx += 1f;
                }

                // Find Y position
                float yy = gameObject.transform.position.y; 
                if (target.position.y < transform.position.y - 1f)
                {
                    yy -= 1f;
                }
                else if (target.position.y > transform.position.y + 1f)
                {
                    yy += 1f;
                }              
                
                // Create attack animation
                myAttack = Instantiate(attackPrefab, new Vector3(xx, yy), Quaternion.identity);
    

            }
        }

        //--- Trigger
        if (myAttack == null)
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

        //--- Audiovisual

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
