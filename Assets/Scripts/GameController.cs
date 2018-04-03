using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		// Restart scene
        if (Input.GetKeyDown (KeyCode.R))
        {
            Application.LoadLevel(Application.loadedLevel);
        }
	}
}
