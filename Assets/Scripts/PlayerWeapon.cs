using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour {

    public WeaponController weaponController; 
    public float damage { get; set; }
    public float attackSpeed { get; set; }
    public string usedWeaponName = "BaseballBat";
    bool weaponFound = false;
    // Use this for initialization
    void Start () {
        /*GameObject weaponControllerGameObject = GameObject.Find("WeaponController");
        weaponController = weaponControllerGameObject.GetComponent<WeaponController>();*/

    }
	
	// Update is called once per frame
	void Update () {
        if (!weaponFound)
        {
            GameObject result = null;
            Debug.Log("trying to find weapon" + result);
            result = weaponController.allWeapons.Find(weapon => weapon.name == usedWeaponName);
            Debug.Log("found weapon" + result);

            if (result != null)
            {
                WeaponStats stats = result.GetComponent<WeaponStats>();
                damage = stats.damage;
                attackSpeed = stats.attackSpeed;
                Debug.Log(damage + "<- Damage | attack speed -> " + attackSpeed);
                weaponFound = true;
            }

        }

    }
}
