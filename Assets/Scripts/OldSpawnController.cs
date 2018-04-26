using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldSpawnController : MonoBehaviour {

    public GameObject spawneri;
    public GameObject enemy4, enemy8, enemy6, enemy2;
    public int trashyAmount, goldyAmount, bigsyAmount;

    private GameObject currentMonster;
    private List<GameObject> monsters;

    private int currentSpawn = 0;

    bool button0;
    bool button1;
    bool button2;
    bool button3;

    // Use this for initialization
    void Start()
    {
        monsters = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.instance.gamepadMobBoss)
        {
            button0 = Input.GetButtonDown("Gamepad Attack 2");
            button1 = Input.GetButtonDown("Gamepad Dodge 2");
            button2 = Input.GetButtonDown("Gamepad 2 Button 2");
            button3 = Input.GetButtonDown("Gamepad 2 Button 3");
        }

        /*
		// Attack
		if (Input.GetKeyDown(KeyCode.Space) && !attacking) {
			//anim.Play ("PlayerAttack");
			anim.SetTrigger("attack");
			//SoundManagerScript.PlaySound ("GoblinDeath");
			SoundManagerScript.PlaySound("Swish");
		}
		*/
        if (button2)
        {
            for (int i = 0; i < trashyAmount; i++)
            {
                currentMonster = Instantiate(enemy4, transform.position, Quaternion.identity);
                monsters.Add(currentMonster);
            }
            SpawnNext();

        }

        if (button3)
        {
            for (int i = 0; i < goldyAmount; i++)
            {
                currentMonster = Instantiate(enemy8, transform.position, Quaternion.identity);
                monsters.Add(currentMonster);
            }
            SpawnNext();
        }

        if (button1)
        {
            for (int i = 0; i < bigsyAmount; i++)
            {
                currentMonster = Instantiate(enemy6, transform.position, Quaternion.identity);
                monsters.Add(currentMonster);
            }
            SpawnNext();
        }

    }

    void SpawnNext()
    {
        currentSpawn += 1;

        switch (currentSpawn)
        {
            case 1:
                transform.position = new Vector3(6, 1, 0);
                break;
            case 2:
                transform.position = new Vector3(6, -1, 0);
                break;
            case 3:
                transform.position = new Vector3(-6, -1, 0);
                break;
            case 4:


                //GameObject enemy = monsters[Random.Range(0, monsters.Count - 1)]; //GameObject.FindGameObjectWithTag ("Enemy");
                GameObject enemy = monsters[3];
                EnemyController c = enemy.GetComponent<EnemyController>();
                c.isControlled = true;

                //enemy.AddComponent<CreatorController> ();
                //ChestController c = other.gameObject.GetComponent<ChestController>();
                //c.health -= 1;

                GameObject testi = GameObject.FindWithTag("GameController");
                testi.GetComponent<GameController>().allMinionsSpawned = true;

                Destroy(gameObject);
                //transform.position new Vector3( 100, 0, 0);
                break;
            default:

                break;
        }


    }

    public void removeFromList(GameObject monster)
    {
        monsters.Remove(monster);
    }

}
