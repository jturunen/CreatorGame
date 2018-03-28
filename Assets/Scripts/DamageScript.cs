﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageScript : MonoBehaviour {
    public PlayerWeapon weapon;
    // Use this for initialization
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.name == "Enemy")
        {
            EnemyController enemy = other.GetComponent<EnemyController>();
            enemy.hitPoints -= weapon.damage;
        }
    }
}
