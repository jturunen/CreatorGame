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
    private float timeUntilNextRound = 3.0f;

    private bool mobBossWin = false; //To show correct win screen
    private bool ottersWin = false; //To show correct win screen
    private bool restart = false; //To restart

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
                RoundController.roundIndex = 0;
                SceneManager.LoadScene("creationPhase", LoadSceneMode.Single);
            }         
            
        }

    }

    private void FixedUpdate()
    {
        if (!myWin && SceneManager.GetActiveScene().name == "MobBossWins" || SceneManager.GetActiveScene().name == "OttersWin")
        {
            Debug.Log(SceneManager.GetActiveScene().name);
            myWin = Instantiate(new GameObject(), new Vector3(0, 0, 0), Quaternion.identity);
            mobBossWin = false;
            ottersWin = false;
            restart = true;

        }
        // If no Minions
        if (!myWin && allMinionsSpawned && !GameObject.FindWithTag("Minion"))
        {
            if (RoundController.roundIndex == 2)
            {
                Debug.Log(RoundController.roundIndex);
                ottersWin = true;
            }
            myWin = Instantiate(winOtters, new Vector3(0, 0, 0), Quaternion.identity);
            timeUntilNextRound = timeBetweenRounds;
            // Sound
            SoundManagerController.instance.PlaySound("Win", 1f);
        }

        // If no Otters
        if (!myWin && !GameObject.FindWithTag("Player"))
        {
            mobBossWin = true;
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
                
                Debug.Log(mobBossWin + "" + ottersWin);
                GameObject.Destroy(GameObject.Find("SpawnController"));

            
                if (mobBossWin)
                {
                    Debug.Log("launch MobBossWins");
                    RoundController.roundIndex = 0;
                    SceneManager.LoadScene("MobBossWins", LoadSceneMode.Single);
                }
                else if (ottersWin)
                {
                    Debug.Log("launch ottersWin");
                    RoundController.roundIndex = 0;
                    SceneManager.LoadScene("OttersWin", LoadSceneMode.Single);
                }
                else
                {
                    Debug.Log(RoundController.roundIndex);
                    if (restart)
                    {
                        RoundController.roundIndex = 0;
                        Debug.Log("launch creationPhase because no one wins?");
                        SceneManager.LoadScene("creationPhase", LoadSceneMode.Single);
                    } else
                    {
                        Debug.Log("RoundController.roundIndex < 2 so launch creationPhase");
                        RoundController.roundIndex++;
                        SceneManager.LoadScene("creationPhase", LoadSceneMode.Single);

                    }

                }


            }

        }
        else if (myWin && timeUntilNextRound > 0)
        {
            timeUntilNextRound -= 1 * Time.deltaTime;
        }

    }
    
}
