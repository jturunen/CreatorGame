using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectController : MonoBehaviour {

    public float lifetime; // How long this effect should last

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FixedUpdate()
    {

        lifetime -= 1 * Time.deltaTime;

        if (lifetime <= 0)
        {
            Destroy(gameObject);            
        }

    }

}
