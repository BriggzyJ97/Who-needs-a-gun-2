using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boxRandomiser : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
	    int random = Random.Range(0, 100);
	    if (random>65)
	    {
            Destroy(gameObject);
	    }
	        
	}
	
}
