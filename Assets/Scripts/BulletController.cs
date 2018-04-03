using UnityEngine;
using System.Collections;

public class BulletController : MonoBehaviour
{
    public Transform target;
    public float moveSpeed = 0;
    public float lifetime = 0f; // How long the attack will last
    public float myDamage = 1f; // How much damage to deal

    //Vector2 point = new Vector2();
    private Vector2 point;

    // Use this for initialization
    void Start()
    {
        //point = target.position;
        point = new Vector2(target.position.x, target.position.y);
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Fixed update
    private void FixedUpdate()
    {

        Debug.Log("I am moving");

        // Move
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
                other.gameObject.GetComponent<PlayerController>().damageTaken += myDamage;
            }
            else if (other.gameObject.GetComponent<Player2Controller>())
            {
                other.gameObject.GetComponent<Player2Controller>().damageTaken += myDamage;
            }
            else if (other.gameObject.GetComponent<Player3Controller>())
            {
                other.gameObject.GetComponent<Player3Controller>().damageTaken += myDamage;
            }


            Destroy(gameObject);
        }

    }

}
