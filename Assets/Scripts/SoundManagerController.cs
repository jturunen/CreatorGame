using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerController : MonoBehaviour {

    // Mega cool stuff Ilpo told me, no need to use static void bullshit
    public static SoundManagerController instance;
    public AudioClip mainMusic, goblinDieSound1, goblinDieSound2, goblinDieSound3,
        goblinDieSound4, goblinDieSound5, goblinDieSound6, goblinDieSound7, swish1, swish2, swish3, gun,
        hit1, hit2, hit3, hit4, hit5, woahLong, woah1, woah2, woah3, woah4, woah5, pumpReload2, explosion1,
        fart1, fart2, fart3, denied;
    static AudioSource audioSrc;
    public bool playMusic;
    public int health = 5;
    //public AudioClip gunSound;
    //public float gunVolume; // Sound volume for gun shooting sound
    //public float swishVolume; // Sound volume for swish/melee attack sound

    // Use this for initialization
    void Start()
    {
        
        instance = this;

        mainMusic = Resources.Load<AudioClip>("Audio/trimmedTeknoaxe");

        goblinDieSound1 = Resources.Load<AudioClip>("Audio/aargh1");
        goblinDieSound2 = Resources.Load<AudioClip>("Audio/aargh2");
        goblinDieSound3 = Resources.Load<AudioClip>("Audio/aargh3");
        goblinDieSound4 = Resources.Load<AudioClip>("Audio/aargh4");
        goblinDieSound5 = Resources.Load<AudioClip>("Audio/aargh5");
        goblinDieSound6 = Resources.Load<AudioClip>("Audio/aargh6");
        goblinDieSound7 = Resources.Load<AudioClip>("Audio/aargh7");

        hit1 = Resources.Load<AudioClip>("Audio/hit1");
        hit2 = Resources.Load<AudioClip>("Audio/hit2");
        hit3 = Resources.Load<AudioClip>("Audio/hit3");
        hit4 = Resources.Load<AudioClip>("Audio/hit4");
        hit5 = Resources.Load<AudioClip>("Audio/hit5");

        swish1 = Resources.Load<AudioClip>("Audio/swish1");
        swish2 = Resources.Load<AudioClip>("Audio/swish2");
        swish3 = Resources.Load<AudioClip>("Audio/swish3");

        gun = Resources.Load<AudioClip>("Audio/chaingun");

        pumpReload2 = Resources.Load<AudioClip>("Audio/pumpReload2");

        explosion1 = Resources.Load<AudioClip>("Audio/explosion1");

        fart1 = Resources.Load<AudioClip>("Audio/fart1");
        fart2 = Resources.Load<AudioClip>("Audio/fart2");
        fart3 = Resources.Load<AudioClip>("Audio/fart3");

        woahLong = Resources.Load<AudioClip>("Audio/woahLong");
        woah1 = Resources.Load<AudioClip>("Audio/woah1");
        woah2 = Resources.Load<AudioClip>("Audio/woah2");
        woah3 = Resources.Load<AudioClip>("Audio/woah3");
        woah4 = Resources.Load<AudioClip>("Audio/woah4");
        woah5 = Resources.Load<AudioClip>("Audio/woah5");

        denied = Resources.Load<AudioClip>("sound_denied");


        audioSrc = GetComponent<AudioSource>();

        // Play background music
        if (playMusic)
        {
            audioSrc.clip = mainMusic;
            audioSrc.loop = true;
            audioSrc.Play();
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlaySound(string clip, float volume)
    {
        switch (clip)
        {

            // Play background music
            case "Music":
                audioSrc.clip = mainMusic;
                audioSrc.loop = true;
                audioSrc.Play();
                break;

            case "GoblinDeath":
                //audioSrc.PlayOneShot(goblinDieSound);

                int i = Random.Range(1, 7);

                switch (i)
                {
                    case 0:
                        //goblinDieSound = Resources.Load<AudioClip> ("aargh0");
                        //audioSrc.PlayOneShot(goblinDieSound0);
                        break;
                    case 1:
                        audioSrc.PlayOneShot(goblinDieSound1);
                        break;
                    case 2:
                        audioSrc.PlayOneShot(goblinDieSound2);
                        break;
                    case 3:
                        audioSrc.PlayOneShot(goblinDieSound3);
                        break;
                    case 4:
                        audioSrc.PlayOneShot(goblinDieSound4);
                        break;
                    case 5:
                        audioSrc.PlayOneShot(goblinDieSound5);
                        break;
                    case 6:
                        audioSrc.PlayOneShot(goblinDieSound6);
                        break;
                    case 7:
                        audioSrc.PlayOneShot(goblinDieSound7);
                        break;

                }

                break;

            case "Swish":

                int iswish = Random.Range(1, 3);

                switch (iswish)
                {
                    case 1:
                        audioSrc.PlayOneShot(swish1);
                        break;
                    case 2:
                        audioSrc.PlayOneShot(swish2);
                        break;
                    case 3:
                        audioSrc.PlayOneShot(swish3);
                        break;
                }

                break;

            case "Fart":

                int ifart = Random.Range(1, 4);

                switch (ifart)
                {
                    case 1:
                        audioSrc.PlayOneShot(fart1);
                        break;
                    case 2:
                        audioSrc.PlayOneShot(fart2);
                        break;
                    case 3:
                        audioSrc.PlayOneShot(fart3);
                        break;
                }

                break;

                
            case "Gun":
                //audioSrc.volume = 0.1f;
                audioSrc.PlayOneShot(gun, volume);
                break;

            case "Explosion":
                audioSrc.PlayOneShot(explosion1);
                break;

            
                
            case "Hit":

                int ihit = Random.Range(1, 4);

                switch (ihit)
                {
                    case 1:
                        audioSrc.PlayOneShot(hit1);
                        break;
                    case 2:
                        audioSrc.PlayOneShot(hit2);
                        break;
                    case 3:
                        audioSrc.PlayOneShot(hit3);
                        break;
                    case 4:
                        audioSrc.PlayOneShot(hit4);
                        break;
                    case 5:
                        audioSrc.PlayOneShot(hit4);
                        break;
                }

                break;

            case "Win":

                int iwoah = Random.Range(1, 5) + 1;

                switch (iwoah)
                {
                    case 1:
                        audioSrc.PlayOneShot(woah1);
                        break;
                    case 2:
                        audioSrc.PlayOneShot(woah2);
                        break;
                    case 3:
                        audioSrc.PlayOneShot(woah3);
                        break;
                    case 4:
                        audioSrc.PlayOneShot(woah4);
                        break;
                    case 5:
                        audioSrc.PlayOneShot(woah5);
                        break;
                }
                break;

            case "Reload":
                audioSrc.PlayOneShot(pumpReload2);
                break;

            case "Denied":
                audioSrc.PlayOneShot(denied);
                break;
        }
    }

}
