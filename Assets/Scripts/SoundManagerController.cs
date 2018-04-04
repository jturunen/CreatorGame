﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerController : MonoBehaviour {

    public static AudioClip mainMusic, goblinDieSound1, goblinDieSound2, goblinDieSound3,
        goblinDieSound4, goblinDieSound5, goblinDieSound6, goblinDieSound7, swish1, swish2, swish3, gun,
        hit1, hit2, hit3, hit4, hit5;
    static AudioSource audioSrc;

    // Use this for initialization
    void Start()
    {

        mainMusic = Resources.Load<AudioClip>("trimmedTeknoaxe");

        goblinDieSound1 = Resources.Load<AudioClip>("aargh1");
        goblinDieSound2 = Resources.Load<AudioClip>("aargh2");
        goblinDieSound3 = Resources.Load<AudioClip>("aargh3");
        goblinDieSound4 = Resources.Load<AudioClip>("aargh4");
        goblinDieSound5 = Resources.Load<AudioClip>("aargh5");
        goblinDieSound6 = Resources.Load<AudioClip>("aargh6");
        goblinDieSound7 = Resources.Load<AudioClip>("aargh7");

        hit1 = Resources.Load<AudioClip>("hit1");
        hit2 = Resources.Load<AudioClip>("hit2");
        hit3 = Resources.Load<AudioClip>("hit3");
        hit4 = Resources.Load<AudioClip>("hit4");
        hit5 = Resources.Load<AudioClip>("hit5");

        swish1 = Resources.Load<AudioClip>("swish1");
        swish2 = Resources.Load<AudioClip>("swish2");
        swish3 = Resources.Load<AudioClip>("swish3");

        gun = Resources.Load<AudioClip>("chaingun");

        audioSrc = GetComponent<AudioSource>();

        audioSrc.clip = mainMusic;
        audioSrc.loop = true;
        audioSrc.Play();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public static void PlaySound(string clip)
    {
        switch (clip)
        {
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

            case "Gun":
                audioSrc.PlayOneShot(gun);
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

        }
    }

}