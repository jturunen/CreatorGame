using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public int minionIndex = 0; // Index for minion naming
    public GameObject winOtters;
    public GameObject winMobboss;
    public bool allMinionsSpawned;
    public bool testMode; // Test mode for spawning?
    public float timeBetweenRounds;
    public ParticleSystem bloodPrefab; // Blood visual effect
    public ParticleSystem bloodParticles; // Ilpo shit for blood, i dunno

    public static GameController instance; // Create static instance for others to use this

    private GameObject myWin;
    private float timeBetweenRoundsNow = 0.0f;
    private float timeUntilNextRound = 0.0f;

    public void CreateParticle(Vector3 position)
    {
        bloodParticles.transform.position = position;
        bloodParticles.Play();
    }

    // Use this for initialization
    void Start () {
		instance = this;
        bloodParticles = Instantiate(bloodPrefab, transform.position, Quaternion.identity);
	}
	
	// Update is called once per frame
	void Update () {
		
        // Restart scene
        if (Input.GetKeyDown (KeyCode.R) || Input.GetKeyDown(KeyCode.Joystick1Button3))
        {

            if (testMode)
            {
                Application.LoadLevel(Application.loadedLevel);
            }
            else
            {
                //Destroy the spawn controller so that game can be restarted properly
                GameObject.Destroy(GameObject.Find("SpawnController"));
                SceneManager.LoadScene("creatingPhaseScene", LoadSceneMode.Single);
            }         
            
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
            SoundManagerController.instance.PlaySound("Win", 1f);
        }

        // If no Otters
        if (!myWin && !GameObject.FindWithTag("Player"))
        {
            myWin = Instantiate(winMobboss, new Vector3(0, 0, 0), Quaternion.identity);
            timeUntilNextRound = timeBetweenRounds;
            // Sound
            SoundManagerController.instance.PlaySound("Win", 1f);
        }

        // Handle time between rounds
        if (myWin && timeUntilNextRound <= 0)
        {

            if(testMode)
            {
                Application.LoadLevel(Application.loadedLevel);
            }
            else
            {
                GameObject.Destroy(GameObject.Find("SpawnController"));
                SceneManager.LoadScene("creatingPhaseScene", LoadSceneMode.Single);
            }

        }
        else if (myWin && timeUntilNextRound > 0)
        {
            timeUntilNextRound -= 1 * Time.deltaTime;
        }

    }
    
}
