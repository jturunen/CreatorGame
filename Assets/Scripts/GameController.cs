using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public GameObject winOtters;
    public GameObject winMobboss;
    public bool allMinionsSpawned;
    public float timeBetweenRounds;

    private GameObject myWin;
    private float timeBetweenRoundsNow = 0.0f;
    private float timeUntilNextRound = 0.0f;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
        // Restart scene
        if (Input.GetKeyDown (KeyCode.R))
        {
            //Application.LoadLevel(Application.loadedLevel);
            //Destroy the spawn controller so that game can be restarted properly
            GameObject.Destroy(GameObject.Find("SpawnController"));
            SceneManager.LoadScene("creatingPhaseScene", LoadSceneMode.Single);
        }

    }

    private void FixedUpdate()
    {

        // If no Minions
        if (!myWin && allMinionsSpawned && !GameObject.FindWithTag("Minion"))
        {
            myWin = Instantiate(winOtters, new Vector3(0, 0, 0), Quaternion.identity);
            timeUntilNextRound = timeBetweenRounds;
            // Sound
            SoundManagerController.PlaySound("Win");
        }

        // If no Otters
        if (!myWin && !GameObject.FindWithTag("Player"))
        {
            myWin = Instantiate(winMobboss, new Vector3(0, 0, 0), Quaternion.identity);
            timeUntilNextRound = timeBetweenRounds;
            // Sound
            SoundManagerController.PlaySound("Win");
        }

        // Handle time between rounds
        if (myWin && timeUntilNextRound <= 0)
        {
            //Application.LoadLevel(Application.loadedLevel);

            GameObject.Destroy(GameObject.Find("SpawnController"));
            SceneManager.LoadScene("creatingPhaseScene", LoadSceneMode.Single);

        }
        else if (myWin && timeUntilNextRound > 0)
        {
            timeUntilNextRound -= 1 * Time.deltaTime;
        }

    }

}
