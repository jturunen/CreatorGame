using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour {

    public WeaponController weaponController; 
    public float damage { get; set; }
    public float attackSpeed { get; set; }
    public string usedWeaponName = "BaseballBat";
    // Use this for initialization
    void Start () {
        /*GameObject weaponControllerGameObject = GameObject.Find("WeaponController");
        weaponController = weaponControllerGameObject.GetComponent<WeaponController>();
        Debug.Log(weaponControllerGameObject);
        Debug.Log(weaponController);
        Debug.Log(weaponController.allWeapons);*/

        GameObject result = weaponController.allWeapons.Find(weapon => weapon.name == usedWeaponName);
        WeaponStats stats = result.GetComponent<WeaponStats>();
        damage = stats.damage;
        attackSpeed = stats.attackSpeed;
        Debug.Log(damage + " " + attackSpeed);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
