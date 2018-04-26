using UnityEngine;
using System.Collections;

public class BulletController : MonoBehaviour
{
    public Transform target;
    public Vector2 direction; // Ilpo direction test
    public float moveSpeed = 0;
    public float lifetime = 0f; // How long the attack will last
    public float damage; // How much damage to deal
    public float accuracy; // Accuracy how well the bullet travels
    public bool bottle; // Is this Trashys special bottle attack?
    public bool rotate; // Should this rotate while moving?
    public bool controlled; // Was this created by MobBoss?
    public GameObject loot; // What loot should this bullet drop when destroyed?
    public GameObject owner; // Who created this object?

    //Vector2 point = new Vector2();
    private Vector2 point;

    // Use this for initialization
    void Start()
    {
        if (!controlled)
        {
            if (!bottle)
            {
                float realAccuracy = accuracy - 100;
                realAccuracy = Random.Range(-realAccuracy, realAccuracy);
                point = new Vector2(target.position.x + realAccuracy, target.position.y + realAccuracy);

            }
            
            // Calculate new direction by creating new vector between current position and target, normalize
            direction = (target.position - transform.position).normalized; 

        }
        else
        {
            direction = direction.normalized;
        }


    }

    // Update is called once per frame
    void Update()
    {

    }

    // Fixed update
    private void FixedUpdate()
    {

        // Debug.Log("I am moving");

        // Move, add random variation derived from accuracy variable
        // transform.position = Vector2.MoveTowards(transform.position, point, moveSpeed * Time.deltaTime);
        
        if (bottle)
        {
            transform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);
        }
        else
        {
            transform.Translate(direction * moveSpeed * Time.deltaTime);

        }

        // Rotate depending on direction
        if (rotate && transform.position.x < point.x)
        {
            // Rotate clockwise
            transform.Rotate(new Vector3(0,0,-1), Time.deltaTime * 1800);
        }
        else if (rotate)
        {
            // Rotate anticlockwise
            transform.Rotate(new Vector3(0,0,1), Time.deltaTime * 1800);    
        }

        

        // Lifetime
        lifetime--;
        if (lifetime < 0)
        {
            // Destroy
            Destroy(gameObject);

            // Drop loot if possible
            if (loot)
            {
                float x = transform.position.x;
                float y = transform.position.y;
                Instantiate(loot, new Vector3(x, y), Quaternion.identity);
            }
        
        }

    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            //Destroy(other.gameObject);
            //other.gameObject.GetComponent<PlayerController>().hitPoints -= 1;
            // Deal damage to player/otter
            if (other.gameObject.GetComponent<PlayerController>())
            {
                other.gameObject.GetComponent<PlayerController>().damageTaken += damage;
            }
            
            // Destroy
            Destroy(gameObject);
            
            // Drop loot if possible
            if (loot)
            {
                float x = transform.position.x;
                float y = transform.position.y;
                Instantiate(loot, new Vector3(x, y), Quaternion.identity); 
            }
            
        }

    }

}
