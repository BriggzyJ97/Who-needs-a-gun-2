using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sparksKiller : MonoBehaviour { //this script deletes the sparks after the particle system reaches its end


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	    Destroy(gameObject, gameObject.GetComponent<ParticleSystem>().main.duration);
	}
}
