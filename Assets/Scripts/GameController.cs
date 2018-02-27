using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	//public AudioClip music;

	// Use this for initialization
	void Start () {
		//AudioSource audio = GetComponent<AudioSource> ();
		//audio.Play();

	}
	
	// Update is called once per frame
	void Update () {

		// Restart scene
		if (Input.GetKeyDown (KeyCode.R)) {
			//SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
			Application.LoadLevel(Application.loadedLevel);
		}

	}
}
