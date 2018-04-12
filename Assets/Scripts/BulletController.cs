using UnityEngine;
using System.Collections;

public class BulletController : MonoBehaviour
{
    public Transform target;
    public float moveSpeed = 0;
    public float lifetime = 0f; // How long the attack will last
    public float damage; // How much damage to deal
    public float accuracy; // Accuracy how well the bullet travels 

    //Vector2 point = new Vector2();
    private Vector2 point;

    // Use this for initialization
    void Start()
    {
        //point = target.position;
        float testi = accuracy - 100;
        testi = Random.Range(-testi, testi);
        point = new Vector2(target.position.x + testi, target.position.y + testi);
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Fixed update
    private void FixedUpdate()
    {

        Debug.Log("I am moving");

        // Move, add random variation derived from accuracy variable
        transform.position = Vector2.MoveTowards(transform.position, point, moveSpeed * Time.deltaTime);
        
        // Lifetime
        lifetime--;
        if (lifetime < 0)
        {
            Destroy(gameObject);
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            //Destroy(other.gameObject);
            //other.gameObject.GetComponent<PlayerController>().hitPoints -= 1;
            if (other.gameObject.GetComponent<PlayerController>())
            {
                other.gameObject.GetComponent<PlayerController>().damageTaken += damage;
            }
            
            Destroy(gameObject);
        }

    }

}
