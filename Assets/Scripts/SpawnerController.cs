using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour {

	public GameObject spawneri;
	public GameObject enemy4, enemy8, enemy6, enemy2;

	private GameObject currentMonster;
	private List<GameObject> monsters;

	private int currentSpawn = 0;

	// Use this for initialization
	void Start () {
		monsters = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {

		/*
		// Attack
		if (Input.GetKeyDown(KeyCode.Space) && !attacking) {
			//anim.Play ("PlayerAttack");
			anim.SetTrigger("attack");
			//SoundManagerScript.PlaySound ("GoblinDeath");
			SoundManagerScript.PlaySound("Swish");
		}
		*/
		if (Input.GetKeyDown(KeyCode.LeftArrow)) {
			for (int i = 0; i < 8; i++) {
				currentMonster = Instantiate (enemy4, transform.position, Quaternion.identity);
				monsters.Add (currentMonster);
			}
			SpawnNext ();

		}

		if (Input.GetKeyDown(KeyCode.UpArrow)) {
			currentMonster = Instantiate (enemy8, transform.position, Quaternion.identity);
			monsters.Add (currentMonster);
			SpawnNext ();
		}

		if (Input.GetKeyDown(KeyCode.RightArrow)) {
			currentMonster = Instantiate (enemy6, transform.position, Quaternion.identity);
			monsters.Add (currentMonster);
			SpawnNext ();
		}

		if (Input.GetKeyDown(KeyCode.DownArrow)) {
			currentMonster = Instantiate (enemy2, transform.position, Quaternion.identity);
			monsters.Add (currentMonster);
			SpawnNext ();
		}

	}

	void SpawnNext() {
		currentSpawn += 1;

		switch (currentSpawn) {
		case 1:
			transform.position = new Vector3 (10, 4, 0);
			break;
		case 2:
			transform.position = new Vector3 (10, -4, 0);
			break;
		case 3:
			transform.position = new Vector3 (-10, -4, 0);
			break;
		case 4:

			GameObject enemy = monsters [Random.Range (0, monsters.Count - 1)]; //GameObject.FindGameObjectWithTag ("Enemy");
			EnemyController c = enemy.GetComponent<EnemyController> ();
			c.creator = true;

			//enemy.AddComponent<CreatorController> ();
			//ChestController c = other.gameObject.GetComponent<ChestController>();
			//c.health -= 1;

			Destroy(gameObject);
			//transform.position new Vector3( 100, 0, 0);
			break;
		default:

			break;
		}


	}

	public void removeFromList(GameObject monster)
	{
		monsters.Remove (monster);
	}
}
